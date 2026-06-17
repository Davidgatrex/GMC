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

        public static CypherReturnCode Cypher(byte[] Input, List<byte> Output, byte[] key)
        {
            using (AESCypher cypher = new(Input))
            {
                var r = cypher.Cypher(key);
                Output.AddRange(cypher.OutData);
                return r;
            }
        }

        public static CypherReturnCode Cypher(Stream Input, Stream Output, out byte[] key)
        {
            var r = AESCypher.Cypher(Input, Output, out key);
            return r;
        }

        public static CypherReturnCode Cypher(Stream Input, Stream Output, byte[] key)
        {
            var r = AESCypher.Cypher(Input, Output, key);
            return r;
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

        public static CypherReturnCode Decypher(Stream Input, Stream Output, byte[] key)
        {
                var r = AESCypher.Decypher(key, Input, Output);
                return r;
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

        public static CypherReturnCode Cypher(Stream inStream, Stream outStream, out byte[] key)
        {
            Aes aes = Aes.Create();
            // 1. Generar clave e IV nuevos
            aes.GenerateKey();
            aes.GenerateIV();
            key = aes.Key;

            try
            {
                // 2. Escribir la longitud del IV (1 byte) directamente en el stream de salida
                outStream.WriteByte((byte)aes.IV.Length);

                // 3. Escribir el IV completo justo después
                outStream.Write(aes.IV, 0, aes.IV.Length);

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // 4. Crear el transformador para CIFRAR
                ICryptoTransform encryptor = aes.CreateEncryptor();

                // 5. Vincular el CryptoStream al stream de SALIDA (escribe cifrado sobre la marcha)
                using (CryptoStream cs = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[65536];
                    int bytesLeidos;

                    // 6. Bucle de streaming: lee limpio de inStream y escribe en el CryptoStream
                    while ((bytesLeidos = inStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cs.Write(buffer, 0, bytesLeidos);
                    }

                    // Asegurar que se procese el último bloque con el padding PKCS7
                    cs.FlushFinalBlock();
                }
            }
            catch (CryptographicException)
            {
                return CypherReturnCode.InvalidInput;
            }
            catch (IOException)
            {
                return CypherReturnCode.InvalidInput;
            }

            return CypherReturnCode.OK;
        }

        public static CypherReturnCode Cypher(Stream inStream, Stream outStream, byte[] key)
        {
            Aes aes = Aes.Create();
            // Validar el tamaño de la clave provista antes de operar
            if (!aes.ValidKeySize(key.Length * 8))
            {
                return CypherReturnCode.InvalidKey;
            }

            aes.Key = key;
            // Generar un IV nuevo y único para esta sesión de cifrado
            aes.GenerateIV();

            try
            {
                // 1. Escribir la longitud del IV (1 byte) en el stream de salida
                outStream.WriteByte((byte)aes.IV.Length);

                // 2. Escribir el IV completo
                outStream.Write(aes.IV, 0, aes.IV.Length);

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // 3. Crear el transformador para CIFRAR
                ICryptoTransform encryptor = aes.CreateEncryptor();

                // 4. Vincular el CryptoStream al stream de SALIDA
                using (CryptoStream cs = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[65536];
                    int bytesLeidos;

                    // 5. Bucle de streaming
                    while ((bytesLeidos = inStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cs.Write(buffer, 0, bytesLeidos);
                    }

                    cs.FlushFinalBlock();
                }
            }
            catch (CryptographicException)
            {
                return CypherReturnCode.InvalidInput;
            }
            catch (IOException)
            {
                return CypherReturnCode.InvalidInput;
            }

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

        public static CypherReturnCode Decypher(byte[] key, Stream inStream, Stream outStream)
        {
            Aes aes = Aes.Create();
            // Validar el tamaño de la clave antes de tocar nada
            if (!aes.ValidKeySize(key.Length * 8))
                return CypherReturnCode.InvalidKey;

            aes.Key = key;

            try
            {
                // 1. Leer el primer byte que indica la longitud del IV
                int ivLenByte = inStream.ReadByte();
                if (ivLenByte == -1) // Stream vacío o corrupto
                    return CypherReturnCode.InvalidInput;

                byte ivLen = (byte)ivLenByte;

                // 2. Leer el IV del stream basándonos en esa longitud
                byte[] iv = new byte[ivLen];
                int bytesReadIv = inStream.Read(iv, 0, ivLen);
                if (bytesReadIv != ivLen) // No se pudieron leer todos los bytes esperados del IV
                    return CypherReturnCode.InvalidInput;

                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // 3. Crear el transformador para DESCIFRAR
                ICryptoTransform decryptor = aes.CreateDecryptor();

                // 4. Vincular el CryptoStream al stream de ENTRADA (le aplica el decryptor al leer)
                using (CryptoStream cs = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                {
                    // Búfer eficiente en RAM para ir volcando al outStream (64 KB)
                    byte[] buffer = new byte[65536];
                    int bytesLeidos;

                    // 5. El bucle de streaming: lee cifrado del CryptoStream y escribe limpio en outStream
                    while ((bytesLeidos = cs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, bytesLeidos);
                    }
                }
            }
            catch (CryptographicException)
            {
                // Salta aquí si el padding PKCS7 falla (clave incorrecta o archivo alterado)
                return CypherReturnCode.InvalidInput;
            }
            catch (IOException)
            {
                // Error de lectura/escritura en el disco o stream cortado abruptamente
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