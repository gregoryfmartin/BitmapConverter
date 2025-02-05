using System.Drawing;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;

namespace BitmapConverter {
    [SupportedOSPlatform("windows")]
    internal class Program {
        static internal void PopulateColorData(Bitmap bmp, ref StringBuilder sb, ref int lc, int dlimit) {
            for(int y = 0; y < bmp.Height; y++) {
                for(int x = 0; x < bmp.Width; x++) {
                    lc++;
                    Color c = bmp.GetPixel(x, y);

                    if(lc < dlimit) {
                        sb.Append($"[{c.R},{c.G},{c.B}],");
                    } else {
                        sb.Append($"[{c.R},{c.G},{c.B}]");
                    }
                }
            }
        }

        static void Main(string[] args) {
            int i            = 0;
            const int dims   = 864;
            StringBuilder sb = new();

            if(args.Length > 1) {
                foreach(string sourceBitmap in args) {
                    try {
                        if(Path.GetExtension(sourceBitmap).Substring(1).ToLower() == "bmp") {
                            using(Bitmap img = new(sourceBitmap, false)) {
                                sb.Append("{")
                                    .Append("\"ColorData\": [");

                                PopulateColorData(img, ref sb, ref i, dims);

                                sb.Append("]")
                                    .Append("}");

                                JsonDocument jd  = JsonDocument.Parse(sb.ToString());
                                string jsonFinal = JsonSerializer.Serialize(jd.RootElement, new JsonSerializerOptions {
                                    WriteIndented = true
                                });

                                File.WriteAllText($"{Path.GetDirectoryName(sourceBitmap)}\\{Path.GetFileNameWithoutExtension(sourceBitmap)}.json", jsonFinal.ToString());
                                Console.WriteLine($"{Path.GetDirectoryName(sourceBitmap)}\\{Path.GetFileNameWithoutExtension(sourceBitmap)}.json written successfully.");
                            }
                        } else {
                            Console.WriteLine($"{sourceBitmap} is not a valid Bitmap image.");
                        }

                        i = 0;
                        sb = new();
                    } catch(Exception e) {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
            } else { // args.Length !> 1
                try {
                    FileAttributes fattrs = File.GetAttributes(args[0]);
                    
                    if(fattrs.HasFlag(FileAttributes.Directory)) {
                        foreach(string fileName in Directory.GetFiles(args[0])) {
                            if(Path.GetExtension(fileName).Substring(1).ToLower() == "bmp") {
                                using(Bitmap img = new(fileName, false)) {
                                    string? pathRoot = Path.GetDirectoryName(fileName);
                                    string? fname    = Path.GetFileNameWithoutExtension(fileName);

                                    sb.Append("{")
                                        .Append("\"ColorData\": [");

                                    PopulateColorData(img, ref sb, ref i, dims);
                                    

                                    sb.Append("]")
                                        .Append("}");

                                    JsonDocument jd  = JsonDocument.Parse(sb.ToString());
                                    string jsonFinal = JsonSerializer.Serialize(jd.RootElement, new JsonSerializerOptions {
                                        WriteIndented = true
                                    });

                                    File.WriteAllText($"{pathRoot}\\{fname}.json", jsonFinal);
                                    Console.WriteLine($"{pathRoot}\\{fname}.json has been written successfully.");
                                }

                                i  = 0;
                                sb = new();
                            }
                        }
                    } else {
                        using(Bitmap img = new(args[0], false)) {
                            string? pathRoot = Path.GetDirectoryName(args[0]);
                            string? fname    = Path.GetFileNameWithoutExtension(args[0]);

                            sb.Append("{")
                                .Append("\"ColorData\": [");

                            PopulateColorData(img, ref sb, ref i, dims);

                            sb.Append("]")
                                .Append("}");

                            JsonDocument jd  = JsonDocument.Parse(sb.ToString());
                            string jsonFinal = JsonSerializer.Serialize(jd.RootElement, new JsonSerializerOptions {
                                WriteIndented = true
                            });

                            File.WriteAllText($"{pathRoot}\\{fname}.json", jsonFinal);
                            Console.WriteLine($"{pathRoot}\\{fname}.json has been written successfully.");
                        }
                    }
                } catch(Exception e) {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            return;
        }
    }
}
