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
        public static CypherReturnCode Cypher(byte[] Input, List<byte> Output, out byte[] key)
        {
            using (AESCypher cypher = new(Input))
            {
                var r = cypher.Cypher(out key);
                Output.AddRange(cypher.OutData);
                return r;
            }
        }

        public static (CypherReturnCode ReturnCode, List<byte> ResultData) Cypher(byte[] Input, byte[] key)
        {
            using (AESCypher cypher = new(Input))
            {
                var r = cypher.Cypher(key);
                return (r, cypher.OutData);
            }
        }

        public static CypherReturnCode Cypher(byte[] Input, List<byte> Output, byte[] key)
        {
            using (AESCypher cypher = new(Input))
            {
                var r = cypher.Cypher(key);
                Output.AddRange(cypher.OutData);
                return r;
            }
        }

        public static CypherReturnCode Decypher(byte[] Input, List<byte> Output, byte[] key)
        {
            using (AESCypher cypher = new(Input))
            {
                var r = cypher.Decypher(key);
                Output.AddRange(cypher.OutData);
                return r;
            }
        }

        // Format will be: AABB, being AA the OP1, BB the OP2
    }

    public class AESCypher : System.IDisposable
    {
        List<byte> InData;
        Aes aes;
        public List<byte> OutData;
        public AESCypher(List<byte> data)
        {
            InData = data;
            OutData = new();
            aes = Aes.Create();
        }

        public AESCypher(byte[] data)
        {
            InData = new(data);
            OutData = new();
            aes = Aes.Create();
        }

        ~AESCypher()
        {
            this.Dispose();
        }

        public CypherReturnCode Cypher(out byte[] key)
        {
            OutData.Clear();
            aes.GenerateKey();
            aes.GenerateIV();

            OutData.Add((byte)aes.IV.Length);
            OutData.AddRange(aes.IV);

            OutData.AddRange(aes.EncryptCbc(InData.ToArray(), aes.IV));
            key = aes.Key;
            return CypherReturnCode.OK;
        }

        public CypherReturnCode Cypher(byte[] key)
        {
            if (!aes.ValidKeySize(key.Length * 8))
                return CypherReturnCode.InvalidKey;

            OutData.Clear();
            aes.Key = key;
            aes.GenerateIV();

            OutData.Add((byte)aes.IV.Length);
            OutData.AddRange(aes.IV);

            OutData.AddRange(aes.EncryptCbc(InData.ToArray(), aes.IV));
            return CypherReturnCode.OK;
        }

        public CypherReturnCode Decypher(byte[] key)
        {
            if (!aes.ValidKeySize(key.Length * 8))
                return CypherReturnCode.InvalidKey;

            aes.Key = key;
            OutData.Clear();

            try
            {
                // 1. Convertimos la lista a un array para trabajar sobre seguro
                byte[] inputBytes = InData.ToArray();

                // 2. Leer la longitud del IV (primer byte)
                byte IVLen = inputBytes[0];

                // 3. Extraer el IV usando LINQ (Sáltate 1 byte, toma IVLen)
                byte[] IV = inputBytes.Skip(1).Take(IVLen).ToArray();

                // 4. Extraer SOLO los datos cifrados (Sáltate el byte de longitud + el IV)
                byte[] cipheredData = inputBytes.Skip(1 + IVLen).ToArray();

                // 5. Descifrar el bloque limpio
                OutData.AddRange(aes.DecryptCbc(cipheredData, IV));
            }
            catch (CryptographicException)
            {
                return CypherReturnCode.InvalidInput;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Por si los datos de entrada están corruptos y el índice no existe
                return CypherReturnCode.InvalidInput;
            }

            return CypherReturnCode.OK;
        }

        public void Dispose()
        {
            OutData.Clear();
            aes.Dispose();
        }
    }
    public enum CypherReturnCode
    {
        OK,
        InvalidKey,
        InvalidInput
    }
}