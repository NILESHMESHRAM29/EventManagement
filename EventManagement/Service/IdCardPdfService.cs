using EventManagement.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCoder;
using System.Drawing;

namespace EventManagement.Service
{
    public class IdCardPdfService
    {
        public byte[] GenerateA3IdCards(IEnumerable<(IdCardDto dto, byte[]? photo)> items, byte[]? logo = null)
        {
            var list = items.ToList();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A3);
                    page.Margin(28);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Grid(grid =>
                    {
                        grid.Columns(4);

                        foreach (var item in list)
                        {
                            var qrBytes = GenerateQrCode(item.dto);

                            grid.Item().Padding(8).AlignCenter().Element(card =>
                            {
                                card.Border(1).CornerRadius(4).Padding(10).Column(col =>
                                {
                                    // Logo
                                    if (logo != null)
                                        col.Item().AlignCenter().Image(logo, ImageScaling.FitWidth);

                                    // Photo


                                    // Name & Role
                                    col.Item().PaddingTop(6).Text(item.dto.Name).Bold().FontSize(18).AlignCenter();
                                    if (!string.IsNullOrEmpty(item.dto.Role))
                                        col.Item().Text(item.dto.Role).Italic().FontSize(10).AlignCenter();






                                    // QR Code Section
                                    col.Item().PaddingTop(10).AlignCenter().Row(row =>
                                    {
                                        row.Spacing(5);
                                        row.ConstantItem(80).Image(qrBytes); // QR Code
                                    });

                                    col.Item().PaddingTop(8).Text($"Issued: {DateTime.UtcNow:yyyy-MM-dd}")
                                        .FontSize(8).AlignCenter();
                                });
                            });
                        }
                    });
                });
            });

            return pdf.GeneratePdf();
        }

        // ------------------------ QR CODE GENERATOR ------------------------
        private byte[] GenerateQrCode(IdCardDto dto)
        {
            string qrContent =
                $"Name: {dto.Name}\n" +
                $"Role: {dto.Role}\n" +
                $"ID: {dto.IdNumber}\n" +
                $"Event: {dto.EventName}";

            var generator = new QRCoder.QRCodeGenerator();
            var qrData = generator.CreateQrCode(qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);

            var pngQr = new QRCoder.PngByteQRCode(qrData);
            return pngQr.GetGraphic(20); // pixels per module (adjust size)
        }
    }
}
