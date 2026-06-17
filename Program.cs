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

        // Cambia la firma para devolver el código Y el array de bytes resultante
        public static (CypherReturnCode ReturnCode, List<byte> ResultData) Cypher(byte[] Input, byte[] key)
        {
            using (AESCypher cypher = new(Input))
            {
                var r = cypher.Cypher(key);
                // Devolvemos una copia directa de los bytes antes de destruir el objeto
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
            byte IVLen = InData[0];
            InData.RemoveAt(0);
            byte[] IV = InData.Take(IVLen).ToArray();
            InData.RemoveRange(0, IVLen);

            OutData.Clear();

            if(!aes.ValidKeySize(key.Length * 8))
                return CypherReturnCode.InvalidKey;

            
            aes.Key = key;
            try
            {
                OutData.AddRange(aes.DecryptCbc(InData.ToArray(), IV));
            }
            catch (CryptographicException)
            {
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