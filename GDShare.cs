using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;

namespace GDSharp {
    public class GDShare {
        private static string _GDSaveData;

        private static string DecryptXOR(string str, int key) {
            byte[] xor = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < str.Length; i++) {
                xor[i] = (byte)(xor[i] ^ key);
            }
            return Encoding.UTF8.GetString(xor);
        }

        private static Byte[] DecryptBase64(string istr) {
            return Convert.FromBase64String(istr);
        }

        private static string DecompressZLib(Byte[] data) {
            // i would once again like to thank https://github.com/gd-edit/GDAPI for being open source
            MemoryStream compressedStream = new MemoryStream(data);
            MemoryStream resultStream = new MemoryStream();
            GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);

            zipStream.CopyTo(resultStream);
            return Encoding.UTF8.GetString(resultStream.ToArray());
        }

        public static string DecodeCCFile(string path) {
            string data;

            Console.WriteLine($"Decoding {path.Split("\\").Last()}");

            Stopwatch watch = new Stopwatch();

            watch.Start();
            string file = File.ReadAllText($"{path}");
            watch.Stop();

            Console.WriteLine($"Reading data file took {watch.ElapsedMilliseconds}ms");

            if (!file.StartsWith("<?xml version=\"1.0\"?>")) {

                watch.Reset();
                watch.Start();
                data = DecryptXOR(file, 11);
                data = data.Replace("-", "+").Replace("_", "/").Replace("\0", string.Empty);
                int remaining = data.Length % 4;
                if (remaining > 0) data += new string('=', 4 - remaining);  // thank you to GDEdit / GDAPI for being open source
                watch.Stop();
                Console.WriteLine($"Decrypting XOR took {watch.ElapsedMilliseconds}ms");

                watch.Reset();
                watch.Start();
                Byte[] gzib64 = DecryptBase64(data);
                watch.Stop();
                Console.WriteLine($"Decrypting Base64 took {watch.ElapsedMilliseconds}ms");

                watch.Reset();
                watch.Start();
                data = DecompressZLib(gzib64);
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