using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace GDSharp {
    public class GDShare {
        private static string _GDSaveData;

        private static string DecryptXOR(string istr, string ikey) {
            byte[] str = Encoding.UTF8.GetBytes(istr);
            byte[] key = Encoding.UTF8.GetBytes(ikey);

            byte[] xor = new byte[str.Length];
            for (int i = 0; i < str.Length; i++) {
                xor[i] = (byte)(str[i] ^ key[i % key.Length]);
            }
            return Encoding.UTF8.GetString(xor);
        }

        private static string DecryptBase64(string istr) {
            return Encoding.UTF8.GetString(Convert.FromBase64String(istr.Replace("-", "+").Replace("_", "/")));
        }

        private static string DecompressZLib(string idata) {
            byte[] data = Encoding.UTF8.GetBytes(idata);

            var outputStream = new MemoryStream();
            using (var compressedStream = new MemoryStream(data))
            using (var inputStream = new InflaterInputStream(compressedStream)) {
                inputStream.CopyTo(outputStream);
                outputStream.Position = 0;
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }

        public static string DecodeCCFile(string path) {
            string data;

            Stopwatch watch = new Stopwatch();

            watch.Start();
            string file = File.ReadAllText($"{path}");
            watch.Stop();

            Console.WriteLine($"Reading data file took {watch.ElapsedMilliseconds}ms");

            if (!file.StartsWith("<?xml version=\"1.0\"?>")) {

                watch.Reset();
                watch.Start();
                data = DecryptXOR(file, "11");
                watch.Stop();
                Console.WriteLine($"Decrypting XOR took {watch.ElapsedMilliseconds}ms");

                watch.Reset();
                watch.Start();
                data = DecryptBase64(data);
                watch.Stop();
                Console.WriteLine($"Decrypting Base64 took {watch.ElapsedMilliseconds}ms");

                watch.Reset();
                watch.Start();
                data = DecompressZLib(data);
                watch.Stop();
                Console.WriteLine($"Decompressing ZLib took {watch.ElapsedMilliseconds}ms");

                _GDSaveData = data;
                return data;
            } else {
                Console.WriteLine("File is already decoded");

                _GDSaveData = file;
                return file;
            }
        }
 
        public static string GetCCPath(string which) {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\..\\Local\\GeometryDash\\CC{which}.dat";
        }
    }
}