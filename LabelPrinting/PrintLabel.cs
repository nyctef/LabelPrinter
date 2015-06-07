﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net;

namespace LabelPrinting
{
    public class PrintLabel
    {
        public static void PrintText(string text)
        {
            var document = new PrintDocument();
            document.DefaultPageSettings = new PageSettings
            {
                Margins = new Margins(mmToHundredthsOfInches(1.2), mmToHundredthsOfInches(1), mmToHundredthsOfInches(1), mmToHundredthsOfInches(1)),
                Landscape = true,
            };
            document.PrinterSettings = new PrinterSettings
            {
                PrintRange = PrintRange.SomePages,
                FromPage = 0,
                ToPage = 0,
                PrinterName = "Brother QL-570",
                //PrinterName = "Microsoft XPS Document Writer",
                //PrintToFile = true,
                //PrintFileName = @"C:\Users\mark\Desktop\temp\test.xps",
            };

            document.DefaultPageSettings.PaperSize = document.PrinterSettings.PaperSizes.Cast<PaperSize>().Where(x => x.PaperName == "62mm x 100mm").Single();
            //document.DefaultPageSettings.PaperSize = new PaperSize("custom label", mmToHundredthsOfInches(60), mmToHundredthsOfInches(60));

            var images = GetImages();

            document.PrintPage += (sender, args) =>
            {
                var printableArea = args.MarginBounds;
                var textWidth = printableArea.Width - 90;
                var titleArea = new Rectangle(printableArea.Location, new Size(textWidth, 80));
                var textArea = new Rectangle(new Point(printableArea.X, printableArea.Y+80), new Size(textWidth, printableArea.Height - 90));
                var iconsLeft = printableArea.Right - 90;
                var iconsArea = new Rectangle(new Point(iconsLeft, printableArea.Top), new Size(80, printableArea.Height));
                var stringFormat = new StringFormat(StringFormatFlags.LineLimit);
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                args.Graphics.DrawString(text, new Font("Arial", 10), Brushes.Black, textArea, stringFormat);
                args.Graphics.DrawString(text, new Font("Arial Black", 12), Brushes.Black, titleArea, stringFormat);
                int iconOffset = 0;
                var iconSize = new Size(70, 70);
                foreach (var image in images)
                {
                    args.Graphics.DrawImage(image, new Rectangle(new Point(iconsArea.Left, iconsArea.Top + iconOffset), iconSize));
                    iconOffset += 85;
                }
                args.HasMorePages = false;
            };
            document.Print();
        }

        private static List<Image> GetImages()
        {
            var result = new List<Image>();

            result.Add(GetImage("https://assets-cdn.github.com/images/modules/logos_page/GitHub-Mark.png"));
            result.Add(GetImage("https://s.gravatar.com/avatar/6e631016bde0b5f8a77b4be683242964?s=80"));

            return result;
        }

        private static Image GetImage(string url)
        {
            using (var wc = new WebClient())
            {
                byte[] bytes = wc.DownloadData(url);
                var ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
        }

        private static int mmToHundredthsOfInches(double mm)
        {
            return (int)(0.254 * mm);
        }
    }
}