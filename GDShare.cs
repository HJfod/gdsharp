using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;

namespace GDSharp {
    public class GDShare {
        private static string _GMSaveData;
        private static string _LLSaveData;
        private static string _CCDirPath;
        private static List<dynamic> _LevelList;

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

        public static string DecodeCCFile(string path, Action<string, int> callback) {
            string data;

            Console.WriteLine($"% Decoding {path.Split("\\").Last()}");

            bool isLL = (path.Split("\\").Last() == "CCGameManager.dat") ? false : true;

            callback($"Loading {path.Split("\\").Last()}", 0 + (isLL ? 50 : 0));

            Stopwatch watch = new Stopwatch();

            watch.Start();
            string file = File.ReadAllText($"{path}");
            watch.Stop();

            Console.WriteLine($"Reading data file took {watch.ElapsedMilliseconds}ms");

            if (!file.StartsWith("<?xml version=\"1.0\"?>")) {

                callback("Decrypting XOR...", 12 + (isLL ? 50 : 0));

                watch.Reset();
                watch.Start();
                data = DecryptXOR(file, 11);
                data = data.Replace("-", "+").Replace("_", "/").Replace("\0", string.Empty);
                int remaining = data.Length % 4;
                if (remaining > 0) data += new string('=', 4 - remaining);  // thank you to GDEdit / GDAPI for being open source
                watch.Stop();
                Console.WriteLine($"Decrypting XOR took {watch.ElapsedMilliseconds}ms");

                callback("Decrypting Base64...", 25 + (isLL ? 50 : 0));

                watch.Reset();
                watch.Start();
                Byte[] gzib64 = DecryptBase64(data);
                watch.Stop();
                Console.WriteLine($"Decrypting Base64 took {watch.ElapsedMilliseconds}ms");

                callback("Decompressing GZip...", 38 + (isLL ? 50 : 0));

                watch.Reset();
                watch.Start();
                data = DecompressZLib(gzib64);
                watch.Stop();
                Console.WriteLine($"Decompressing ZLib took {watch.ElapsedMilliseconds}ms");

                Console.WriteLine($"+ Decoded {path.Split("\\").Last()}");
                
                _CCDirPath = path.Substring(0, path.LastIndexOf("\\"));
                Console.WriteLine(_CCDirPath);
                if (isLL) _LLSaveData = data; else _GMSaveData = data;
                return data;
            } else {
                Console.WriteLine($"+ {path.Split("\\").Last()} is already decoded");
                
                _CCDirPath = path.Substring(0, path.LastIndexOf("\\"));
                if (isLL) _LLSaveData = file; else _GMSaveData = file;
                return file;
            }
        }
 
        public static string GetCCPath(string which) {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\..\\Local\\GeometryDash\\CC{which}.dat";
        }

        private static string GetKey(string savedata, string key, string type = ".*?", bool legacy = false) {
            if (type == null) {
                Match match = Regex.Match(savedata, $"<k>{key}</k>.*?>", RegexOptions.None, Regex.InfiniteMatchTimeout);
                if (match.Value != "") {
                    return match.Value.Substring(match.Value.LastIndexOf("<")).IndexOf("t") > -1 ? "True" : "False";
                } else {
                    return "False";
                }
            }
            string matcher = $"<k>{key}</k><{type}>.*?</{type}>";
            Match m = Regex.Match(savedata, matcher, RegexOptions.None, Regex.InfiniteMatchTimeout);
            return m.Value == "" ? "" : m.Value.Substring($"<k>{key}</k><A>".Length, m.Value.Length - $"<k>{key}</k><A>".Length - $"</A>".Length);
        }
        
        public static dynamic GetGDUserInfo(string savedata) {
            if (savedata == null) savedata = _GMSaveData;

            string statdata = GetKey(savedata, "GS_value", "d");
            return new {
                Name = GetKey(savedata, "playerName", "s"),
                UserID = GetKey(savedata, "playerUserID", "i"),
                Stats = new {
                    Jumps = GetKey(statdata, "1", "s"),
                    Total_Attempts = GetKey(statdata, "2", "s"),
                    Completed_Online_Levels = GetKey(statdata, "4", "s"),
                    Demons = GetKey(statdata, "5", "s"),
                    Stars = GetKey(statdata, "6", "s"),
                    Diamonds = GetKey(statdata, "13", "s"),
                    Orbs = GetKey(statdata, "14", "s"),
                    Coins = GetKey(statdata, "8", "s"),
                    User_Coins = GetKey(statdata, "12", "s"),
                    Killed_Players = GetKey(statdata, "9", "s")
                }
            };
        }

        public static List<dynamic> GetLevelList(string savedata = null, Action<string, int> callback = null) {
            if (savedata == null) savedata = _LLSaveData;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            List<dynamic> levels = new List<dynamic>();
            string matcher = @"<k>k_\d+<\/k>.+?<\/d>\n? *<\/d>";
            
            foreach (Match lvl in Regex.Matches(savedata, matcher, RegexOptions.Singleline, Regex.InfiniteMatchTimeout)) {
                if (lvl.Value != "") {
                    string Name = GetKey(lvl.Value, "k2", "s");

                    if (callback != null) callback($"Loaded {Name}", 100);

                    levels.Add(new { Name = Name, Data = lvl.ToString() });
                }
            }

            watch.Stop();
            Console.WriteLine($"+ Got levels in {watch.ElapsedMilliseconds}ms");

            _LevelList = levels;

            return levels;
        }

        private static string ReplaceOfficialSongName(string Song) {
            return (new Dictionary<string, string> {
                { "0", "Stereo Madness" },
                { "1", "Back on Track" },
                { "2", "Polargeist" },
                { "3", "Dry Out" },
                { "4", "Base After Base" },
                { "5", "Cant Let Go" },
                { "6", "Jumper" },
                { "7", "Time Machine" },
                { "8", "Cycles" },
                { "9", "xStep" },
                { "10", "Clutterfunk" },
                { "11", "Theory of Everything" },
                { "12", "Electroman Adventures" },
                { "13", "Clubstep" },
                { "14", "Electrodynamix" },
                { "15", "Hexagon Force" },
                { "16", "Blast Processing" },
                { "17", "Theory of Everything 2" },
                { "18", "Geometrical Dominator" },
                { "19", "Deadlocked" },
                { "20", "Fingerdash" },
            })[Song];
        }

        public static dynamic GetLevelInfo(string name) {
            dynamic lvl = null;

            foreach (dynamic x in _LevelList) {
                if (x.Name == name) {
                    lvl = x;
                    break;
                }
            }

            if (lvl == null) {
                return null;
            } else {
                int editorTime = Int32.Parse(GetKey(lvl.Data, "k80"));
                string P = GetKey(lvl.Data, "k41");
                string Song = GetKey(lvl.Data, "k8");
                string Desc = Encoding.UTF8.GetString(DecryptBase64(GetKey(lvl.Data, "k3")));
                string Rev = GetKey(lvl.Data, "k46");
                string Copy = GetKey(lvl.Data, "k42");

                return new {
                    Name = GetKey(lvl.Data, "k2"),
                    Length = GetKey(lvl.Data, "k23"),
                    Creator = GetKey(lvl.Data, "k5"),
                    Version = GetKey(lvl.Data, "k16"),
                    Password = P == "1" ? "Free to Copy" : P == "" ? "No copy" : P.Substring(1),
                    Song = Song != "" ? ReplaceOfficialSongName(Song) : GetKey(lvl.Data, "k45"),
                    Description = Desc,
                    Object_count = GetKey(lvl.Data, "k48"),
                    Editor_time = editorTime > 3600 ? $"{Math.Round((float)editorTime / 3600F, 2)}h" : $"{Math.Round((float)editorTime / 60F, 2)}m",
                    Verified = GetKey(lvl.Data, "k14", null),
                    Attempts = GetKey(lvl.Data, "k18"),
                    Revision = Rev == "" ? "None" : Rev,
                    Copied_from = Copy == "" ? "None" : Copy
                };
            }
        }

        public static string ExportLevel(string name, string path = "") {
            try {
                dynamic lvl = null;

                foreach (dynamic x in _LevelList) {
                    if (x.Name == name) {
                        lvl = x;
                        break;
                    }
                }

                if (lvl == null) {
                    return $"Level {name} not found.";
                } else {
                    string output = $@"{path}\{name}.gmd";

                    string NewData = Regex.Replace(lvl.Data, @"<k>k_\d+<\/k>", "");

                    File.WriteAllText(output, NewData);

                    return null;
                }
            } catch (Exception e) {
                return $"Error exporting {name}: {e}.";
            }
        }

        public static string ImportLevel(string path) {
            if (File.Exists(path)) {
                try {
                    string lvl = File.ReadAllText(path);
                    string data = _LLSaveData;
                    
                    data = Regex.Replace(data, @"<k>k1<\/k><i>\d+?<\/i>", "");
                    string[] splitData = data.Split("<k>_isArr</k><t />");
                    splitData[1] = Regex.Replace(splitData[1], @"<k>k_(\d+)<\/k><d><k>kCEK<\/k>",
                    (Match m) => $"<k>k_{(Int32.Parse((Regex.Match(m.Value, @"k_\d+").Value.Substring(2))) + 1)}</k><d><k>kCEK</k>");
                    data = splitData[0] + "<k>_isArr</k><t /><k>k_0</k>" + lvl + splitData[1];

                    File.WriteAllText($"{_CCDirPath}\\CCLocalLevels.dat", data);

                    return null;
                } catch (Exception e) {
                    return e.ToString();
                }
            } else {
                return "File doesn't exist!";
            }
        }
    }
}