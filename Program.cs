using System.Security.Cryptography;

namespace GMC
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new UMC_UI_1((args.Length > 0) ? args[0] : ""));
        }
    }

public class CypherCapsule
    {
        public static void Main(byte[] key, string InputPath, string OutputPath, bool Decypher, int rounds)
        {
            byte[] Input = File.ReadAllBytes(InputPath);
            byte[] Output = new byte[Input.Length];

            Generic(key, Input, ref Output, Decypher, rounds);
            File.WriteAllBytes(OutputPath, Output);
        }

        public static void Generic(byte[] key, byte[] Input, ref byte[] Output, bool Decypher, int rounds)
        {
            byte[] keyInt = SHA256.HashData(key);

            using var hmac = new HMACSHA256(key);
            byte[] buf = hmac.ComputeHash(BitConverter.GetBytes((int)((long)DateTime.Now.TimeOfDay.TotalSeconds ^ DateTime.Now.Date.DayOfYear)));

            byte[] IV = SHA256.HashData(buf);

            if (!Decypher)
            {
                Console.WriteLine("Cyphering");
                for (int i = 0; i < rounds; i++)
                {
                    Cypherer cypherer = new(Input, keyInt, IV, i);
                    cypherer.Cypher();
                    Input = cypherer.outPack.ToArray();
                    GC.Collect();
                }
            }
            else
            {
                Console.WriteLine("Decyphering");

                for (int i = 0; i < rounds; i++)
                {
                    Decypherer decypherer = new(Input, keyInt);
                    decypherer.Decypher();
                    Input = decypherer.outData.ToArray();
                    GC.Collect();
                }
            }

            for (int i = 0; i < Input.Length; i++)
                Output[i] = Input[i];
        }

        // Format will be: AABB, being AA the OP1, BB the OP2
    }

    public static class CUtils
    {
        public static byte[] KDF(byte[] key, byte[] salt, byte[] IV)
        {
            byte[] bytes = new byte[key.Length];
            byte[] saltf = SHA256.HashData(salt);

            for (int i = 0; i < key.Length; i++)
                bytes[i] = (byte)(key[i] ^ saltf[i] & IV[i]);

            return bytes;
        }

        public static byte[] CalcPermKeyXor(byte[] key, byte[] iv, int round)
        {
            byte[] bytes = new byte[key.Length];
            if (key.Length != iv.Length)
                throw new ArgumentException();

            for (int i = 0; i < key.Length; i++)
                bytes[i] = (byte)(key[i] ^ iv[i]);

            bytes[0] ^= (byte)round;
            return bytes;
        }

        public static byte Accum(byte[] bytes)
        {
            byte acc = 0;
            foreach (byte b in bytes)
                acc += b;
            return acc;
        }

        public static uint AccumUint(byte[] bytes)
        {
            uint acc = 0;
            foreach (byte b in bytes)
                acc += b;
            return acc;
        }
    }

    public class Cypherer
    {
        public byte[] inData;
        public List<byte> outData;
        public List<byte> outPack = new();
        public List<byte> key;
        public byte[] IV;
        public int round;
        public Cypherer(byte[] _in, byte[] _key, byte[] iV, int _round)
        {
            outData = new();
            key = _key.ToList();
            inData = _in;
            IV = iV;
            round = _round;
        }

        public byte FindPosKey(byte Target, byte Prev)
        {
            int k = SecureRandom(outData.Count, CUtils.KDF(key.ToArray(), System.Text.Encoding.ASCII.GetBytes($"PosKey_Salt{round}"), IV));
            byte b = key[k % key.Count];
            byte o = (outData.Count > 0) ? outData.Last() : (byte)0;

            byte r = (byte)(((b ^ Target) + (o ^ Prev)) % (1 << 8));

            return r;
        }

        private int SecureRandom(int i, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            byte[] buf = hmac.ComputeHash(BitConverter.GetBytes(i));
            return Math.Abs(BitConverter.ToInt32(buf, 0));
        }

        public void SBoxify()
        {
            // Inicializar SBox con 0..255
            byte[] sbox = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();

            // Fisher-Yates con PRF(key, i)
            for (int i = 255; i > 0; i--)
            {
                int j = SecureRandom(i, CUtils.KDF(key.ToArray(), System.Text.Encoding.ASCII.GetBytes($"SBOX_SALT{round}"), IV)) % (i + 1);
                (sbox[i], sbox[j]) = (sbox[j], sbox[i]);
            }

            // Aplicar SBox
            foreach (byte b in outData)
                outPack.Add(sbox[b]);
        }

        public void Cypher()
        {
            byte last = 0;
            byte prev = 0;
            foreach (byte b in inData)
            {
                byte pk = FindPosKey(b, prev);

                int n = key[outData.Count % key.Count] % 6;
                byte n2 = (byte)(last ^ key[outData.Count % key.Count]);
                last = n2;

                pk = (byte)((pk + CUtils.Accum(CUtils.KDF(key.ToArray(), System.Text.Encoding.ASCII.GetBytes($"Cypher_SALT{round}"), IV))) % (uint)Math.Pow(2, 16));

                outData.Add(pk);

                prev = b;
            }

            if ((round % 2) == (round % 3))
                if ((round % 2) == 0)
                    RShift_H();
                else
                    LShift_H();

            if ((round % 3) == 0)
            {
                byte[] PermKey = CUtils.KDF(key.ToArray(), System.Text.Encoding.ASCII.GetBytes($"Perm1_SALT{round}"), IV);

                for (int i = 0; i < outData.Count / 2; i++)
                {
                    byte t = outData[i];
                    int dst = outData.Count - 1 - (PermKey[i % PermKey.Length] % (outData.Count - 1));
                    outData[i] = outData[dst];
                    outData[dst] = t;
                }
            }

            if ((round % 2) == 0)
                RShift();
            else
                LShift();

            if ((round % 3) == 1)
            {
                byte[] PermKey = CUtils.KDF(key.ToArray(), System.Text.Encoding.ASCII.GetBytes($"Perm2_SALT{round}"), IV);

                for (int i = 0; i < outData.Count / 2; i++)
                {
                    byte t = outData[i];
                    int dst = outData.Count - 1 - (PermKey[i % PermKey.Length] % (outData.Count - 1));
                    outData[i] = outData[dst];
                    outData[dst] = t;
                }
            }

            SBoxify();

            if((round % 3) == 2)
            {
                byte[] PermKey = CUtils.KDF(key.ToArray(), System.Text.Encoding.ASCII.GetBytes($"Perm3_SALT{round}"), IV);

                for (int i = 0; i < outPack.Count / 2; i++)
                {
                    byte t = outPack[i];
                    int dst = outPack.Count - 1 - (PermKey[i % PermKey.Length] % (outPack.Count - 1));
                    outPack[i] = outPack[dst];
                    outPack[dst] = t;
                }
            }
        }

        private void RShift()
        {
            int count = key[round] % 4;
            byte CarryMask = (byte)(0xFF << (8 - count));
            byte carry = (byte)(outData.Last() & CarryMask);
            for (int i = 0; i < outData.Count; i++)
            {
                byte t = (byte)(outData[i] & CarryMask);
                outData[i] = (byte)((outData[i] >> count) | carry);
                carry = t;
            }
        }

        private void LShift()
        {
            int count = key[round] % 4;
            byte CarryMask = (byte)(0xFF >> (8 - count));
            byte carry = (byte)(outData[0] & CarryMask);
            for (int i = outData.Count - 1; i >= 0; i--)
            {
                byte t = (byte)(outData[i] & CarryMask);
                outData[i] = (byte)((outData[i] << count) | carry);
                carry = t;
            }
        }

        private void RShift_H()
        {
            int count = key[round] % 4;
            byte CarryMask = (byte)(0xFF << (8 - count));
            byte carry = (byte)(outData[(outData.Count - 1) & ~0b1] & CarryMask);
            for (int i = 0; i < outData.Count; i+=2)
            {
                if ((i & 1) == 1)
                    continue;
                byte t = (byte)(outData[i] & CarryMask);
                outData[i] = (byte)((outData[i] >> count) | carry);
                carry = t;
            }
        }

        private void LShift_H()
        {
            int count = (key[round] + IV[round]) % 4;
            byte CarryMask = (byte)(0xFF >> (8 - count));
            byte carry = (byte)(outData[0] & CarryMask);
            for (int i = (outData.Count - 1) & ~0b111; i >= 0; i-=7)
            {
                byte t = (byte)(outData[i] & CarryMask);
                outData[i] = (byte)((outData[i] << count) | carry);
                carry = t;
            }
        }
    }

    public class Decypherer
    {
        public byte[] inPack;
        public List<byte> inData = new();
        public List<byte> outData;
        public List<byte> key;
        public Decypherer(byte[] _in, byte[] _key)
        {
            outData = new();
            key = _key.ToList();
            inPack = _in;
        }

        private int SecureRandom(int i, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            byte[] buf = hmac.ComputeHash(BitConverter.GetBytes(i));
            return Math.Abs(BitConverter.ToInt32(buf, 0));
        }
        public void UnSBoxify()
        {
            // Inicializar SBox con 0..255
            byte[] sbox = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();

            // Fisher-Yates con PRF(key, i)
            for (int i = 255; i > 0; i--)
            {
                int j = SecureRandom(i, key.ToArray()) % (i + 1);
                (sbox[i], sbox[j]) = (sbox[j], sbox[i]);
            }

            byte[] inv = new byte[256];

            for (int i = 0; i < 256; i++)
                inv[sbox[i]] = (byte)i;

            foreach (byte b in inPack)
                inData.Add(inv[b]);
        }


        public void Decypher()
        {
            UnSBoxify();
            Console.WriteLine("Deboxed");
            byte last = 0;
            for (int i = 0; i < inData.Count; i++)
            {
                byte n2 = (byte)(last ^ key[i % key.Count]);
                last = n2;

                int k = SecureRandom(i, key.ToArray());
                byte b = key[k % key.Count];
                byte Prev = (i > 0) ? inData[i - 1] : (byte)0;
                byte O = (outData.Count > 0) ? outData.Last() : (byte)0;

                try
                {
                    outData.Add((byte)(inData[i] ^ b ^ Prev ^ O ^ n2));
                }
                catch
                {
                    outData.Add((byte)SecureRandom(i, key.ToArray()));
                }
            }
        }
    }

    public struct PosKey
    {
        public byte X;
        public byte Pos1;
        public byte Diff;

        public PosKey(byte _Pos1, byte _X, byte _Diff)
        {
            Pos1 = _Pos1;
            X = _X;
            Diff = _Diff;
        }
    }

    public class CRC32
    {
        private static readonly uint[] Table;

        // El polinomio estándar de IEEE 802.3 invertido (0xEDB88320)
        private const uint Polynomial = 0xEDB88320;

        // Inicializa la tabla de búsqueda una sola vez
        static CRC32()
        {
            Table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (int j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ Polynomial;
                    else
                        crc >>= 1;
                }
                Table[i] = crc;
            }
        }

        // Calcula el hash CRC32 a partir de un array de bytes
        public static uint Compute(byte[] bytes)
        {
            uint crc = 0xFFFFFFFF;

            foreach (byte b in bytes)
            {
                byte index = (byte)((crc ^ b) & 0xFF);
                crc = (crc >> 8) ^ Table[index];
            }

            return ~crc; // Invierte los bits al final
        }
    }

}