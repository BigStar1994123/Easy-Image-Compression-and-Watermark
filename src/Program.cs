using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ImageCompression
{
    public class Program
    {
        private static readonly string GeekyTripName = "Geekytrip";
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.WriteLine("大星的圖片壓縮程式==!!");
            var jsonString = File.ReadAllText("config.json");
            _config = JsonSerializer.Deserialize<Config>(jsonString);
            Console.WriteLine(_config);

            Console.Write("請先選擇圖片所在的 Folder: ");

            var select = new Select
            {
                InitialFolder = "C:\\"
            };
            select.ShowDialog();
            Console.WriteLine($"{select.Folder}");

            var filePaths = Directory.GetFiles(select.Folder, "*");
            Console.WriteLine("列出所有檔案: ");
            foreach (var filepath in filePaths)
            {
                Console.WriteLine(filepath);
            }
            Console.WriteLine("--------------------------------");

            var imageFolder = select.Folder;
            var targetFolder = imageFolder + "/CompressionAndWatermark";
            Directory.CreateDirectory(targetFolder);

            foreach (var filePath in filePaths)
            {
                try
                {
                    Console.WriteLine($"{filePath} 處理中...");

                    var fileName = Path.GetFileName(filePath);
                    var bmp = new Bitmap(filePath);
                    RotateImage(bmp);

                    if (_config.Image.Resize.Enabled)
                    {
                        bmp = ResizeImage(bmp);
                    }

                    DrawWaterMark(bmp);

                    RotateImage(bmp, IsReverse: true);

                    if (_config.Image.Compression.Enabled)
                    {
                        var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        var myEncoder = Encoder.Quality;
                        var myEncoderParameters = new EncoderParameters(1);
                        myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, _config.Image.Compression.Level);

                        bmp.Save($"{targetFolder}\\{fileName}", jpgEncoder, myEncoderParameters);
                    }
                    else
                    {
                        bmp.Save($"{targetFolder}\\{fileName}");
                    }
                }
                catch (Exception ex)
                {
                    // 不處理
                }
            }

            Console.WriteLine("大星的圖片壓縮程式跑完了88");
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private static void RotateImage(Bitmap bmp, bool IsReverse = false)
        {
            // see: https://www.impulseadventure.com/photo/exif-orientation.html
            var propertie = bmp.PropertyItems.FirstOrDefault(p => p.Id == 274);
            if (propertie != null)
            {
                var orientation = propertie.Value[0];

                if (IsReverse)
                {
                    if (orientation == 6)
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    if (orientation == 8)
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                }
                else
                {
                    if (orientation == 6)
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    if (orientation == 8)
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                }
            }
        }

        private static Bitmap ResizeImage(Bitmap bmp)
        {
            var width = bmp.Width * _config.Image.Resize.Percentage;
            var height = bmp.Height * _config.Image.Resize.Percentage;

            var resizedbitmap = new Bitmap((int)width, (int)height);
            Graphics g = Graphics.FromImage(resizedbitmap);
            g.DrawImage(bmp, 0, 0, (int)width, (int)height);
            return resizedbitmap;
        }

        private static void DrawWaterMark(Bitmap bmp)
        {
            var font = new Font("Arial", bmp.Height * (float)_config.WaterMark.Font.HeightSizePercentage, FontStyle.Bold, GraphicsUnit.Pixel);
            var color = Color.FromArgb(_config.WaterMark.Color.A, _config.WaterMark.Color.R, _config.WaterMark.Color.G, _config.WaterMark.Color.B);
            var graphic = Graphics.FromImage(bmp);

            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            graphic.DrawString(GeekyTripName, font, new SolidBrush(color),
                new Point(
                    (int)(bmp.Width * _config.WaterMark.Font.WidthPositionPercentage),
                    (int)(bmp.Height * _config.WaterMark.Font.HeightPositionPercentage)),
                    stringFormat);
        }
    }
}