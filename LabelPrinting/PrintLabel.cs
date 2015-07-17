using System;
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
        public static void Print(string title, string text, List<string> imageUrls)
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
                //PrintFileName = @"C:\Users\mark.jordan.red-gate\Desktop\temp\test.xps",
            };

            document.DefaultPageSettings.PaperSize = document.PrinterSettings.PaperSizes.Cast<PaperSize>().Where(x => x.PaperName == "62mm x 100mm").Single();
            //document.DefaultPageSettings.PaperSize = new PaperSize("custom label", mmToHundredthsOfInches(60), mmToHundredthsOfInches(60));;

            document.PrintPage += (sender, args) =>
            {
                var printableArea = args.MarginBounds;
                var textWidth = printableArea.Width - 95;
                var titleArea = new Rectangle(printableArea.Location, new Size(textWidth, 80));
                var textArea = new Rectangle(new Point(printableArea.X, printableArea.Y+80), new Size(textWidth, printableArea.Height - 90));
                var iconsLeft = printableArea.Right - 95;
                var iconsArea = new Rectangle(new Point(iconsLeft, printableArea.Top), new Size(80, printableArea.Height));
                var stringFormat = new StringFormat(StringFormatFlags.LineLimit);
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                args.Graphics.DrawString(title, new Font("Arial Black", 12), Brushes.Black, titleArea, stringFormat);
                args.Graphics.DrawString(text, new Font("Arial", 10), Brushes.Black, textArea, stringFormat);
                int iconOffset = 0;
                var iconSize = new Size(70, 70);
                foreach (var image in imageUrls.Select(GetImage))
                {
                    args.Graphics.DrawImage(image, new Rectangle(new Point(iconsArea.Left, iconsArea.Top + iconOffset), iconSize));
                    iconOffset += 75;
                }
                args.HasMorePages = false;
            };
            document.Print();
        }

        public static void Print(string title, List<string> imageUrls)
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
                //PrintFileName = @"C:\Users\mark.jordan.red-gate\Desktop\temp\test.xps",
            };

            document.DefaultPageSettings.PaperSize = document.PrinterSettings.PaperSizes.Cast<PaperSize>().Where(x => x.PaperName == "62mm x 100mm").Single();
            //document.DefaultPageSettings.PaperSize = new PaperSize("custom label", mmToHundredthsOfInches(60), mmToHundredthsOfInches(60));;;

            document.PrintPage += (sender, args) =>
            {
                var printableArea = args.MarginBounds;
                var titleArea = new Rectangle(new Point(printableArea.Left, printableArea.Top+80), new Size(printableArea.Width, printableArea.Height-80));
                var iconsArea = new Rectangle(printableArea.Location, new Size(printableArea.Width, 80));

                var stringFormat = new StringFormat(StringFormatFlags.LineLimit);
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;

                args.Graphics.DrawString(title, new Font("Arial Black", 24), Brushes.Black, titleArea, stringFormat);
                int iconOffset = 0;
                var iconSize = new Size(70, 70);
                foreach (var image in imageUrls.Select(GetImage))
                {
                    args.Graphics.DrawImage(image, new Rectangle(new Point(iconsArea.Left + iconOffset, iconsArea.Top), iconSize));
                    iconOffset += 75;
                }
                args.HasMorePages = false;
            };
            document.Print();
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
