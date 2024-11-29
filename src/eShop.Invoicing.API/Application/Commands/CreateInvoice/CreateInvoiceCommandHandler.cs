using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Invoicing.API.Application.GuardClauses;
using eShop.Invoicing.API.Application.Storage;
using eShop.Ordering.Contracts.GetOrder;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using Svg.Skia;

namespace eShop.Invoicing.API.Application.Commands.CreateInvoice;

internal class CreateInvoiceCommandHandler(ILogger<CreateInvoiceCommandHandler> logger,
    IOrderingApiClient apiClient, IFileStorage fileStorage) : IRequestHandler<CreateInvoiceCommand, Result>
{
    private OrderDto? order;

    public async Task<Result> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.order = await apiClient.GetOrder(request.OrderId);

            Result foundResult = Guard.Against.OrderNull(this.order, logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            QuestPDF.Settings.License = LicenseType.Community;

            byte[] pdf = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.MarginTop(1, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.DefaultTextStyle(_ => _.FontSize(9).FontFamily("Roboto"));

                        page.Content().Element(this.ComposeContent);
                        page.Footer().Element(this.ComposeFooter);
                    });
                })
                .GeneratePdf();

            string fileName = $"INV{this.order.OrderNumber.ToString().PadLeft(8, '0')}.pdf";
            await fileStorage.UploadFile(fileName, pdf);

            //Document.Create(container =>
            //{
            //    container.Page(page =>
            //    {
            //        page.Size(PageSizes.A4);
            //        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
            //        page.MarginTop(1, QuestPDF.Infrastructure.Unit.Centimetre);
            //        page.DefaultTextStyle(_ => _.FontSize(9).FontFamily("Roboto"));

            //        page.Content().Element(this.ComposeContent);
            //        page.Footer().Element(this.ComposeFooter);
            //    });
            //})
            //.GeneratePdf(@"C:\temp\InvoiceTest.pdf");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create invoice.";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    private void ComposeContent(IContainer container)
    {
        ConvertSvgToPng("logo-header.svg", "logo-header.png");

        container.PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre).Column(column =>
        {
            column.Item().Row(row =>
            {
                IContainer item = row.RelativeItem();
                item.Column(column =>
                {
                    column.Item().Width(100).PaddingBottom(1, QuestPDF.Infrastructure.Unit.Centimetre).Image("logo-header.png");
                    column.Spacing(5);
                    column.Item().Text(this.order!.BuyerName).FontSize(10);
                    column.Item().Text(this.order.Address.Street).FontSize(10);
                    column.Item().Text($"{this.order.Address.ZipCode} {this.order.Address.City}").FontSize(10);
                    column.Item()
                        .PaddingBottom(20)
                        .Text(this.order.Address.Country).FontSize(10);

                    column.Item().Text($"Order number {this.order.OrderNumber}");
                    column.Item().Text($"Order date {this.order.OrderDate.ToShortDateString()}");
                });

                item = row.ConstantItem(150).AlignRight();
                item.Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text("AdventureWorks Ltd.").Bold();
                    column.Item().Hyperlink("https://www.adventureworks.com")
                        .Text("https://www.adventureworks.com")
                        .FontColor(Colors.Blue.Medium);
                });

            });

            column.Item().PaddingVertical(3, QuestPDF.Infrastructure.Unit.Centimetre).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(); // Description
                    columns.ConstantColumn(40); // Quantity                
                    columns.ConstantColumn(80); // Unit Price
                    columns.ConstantColumn(80); // Total Price
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Description");
                    header.Cell().Element(CellStyle).Text("Qty");
                    header.Cell().Element(CellStyle).Text("Unit Price");
                    header.Cell().Element(CellStyle).Text("Total Price");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1);
                    }
                });

                foreach (OrderItemDto line in this.order!.OrderItems)
                {
                    decimal lineTotal = line.UnitPrice * line.Units;

                    table.Cell().Element(CellStyle).Text(line.ProductName);
                    table.Cell().Element(CellStyle).Text(line.Units.ToString());
                    table.Cell().Element(CellStyle).Text(line.UnitPrice.ToString("C"));
                    table.Cell().Element(CellStyle).Text(lineTotal.ToString("C"));

                    // Define a style for regular cells
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.Padding(5);
                    }
                }

                table.Footer(footer =>
                {
                    footer.Cell().Element(TableFooterLineStyle).BorderBottom(2).BorderColor(Colors.Black);
                    footer.Cell().BorderBottom(2).BorderColor(Colors.Black);
                    footer.Cell().BorderBottom(2).BorderColor(Colors.Black);
                    footer.Cell().BorderBottom(2).BorderColor(Colors.Black);

                    footer.Cell().Element(TotalStyle).Text("");
                    footer.Cell().Element(TotalStyle).Text("");
                    footer.Cell().Element(TotalsLabelStyle).Text("Subtotal").AlignRight();
                    footer.Cell().Element(TotalStyle).Text(this.order.Total.ToString("C"));

                    footer.Cell().Text("");
                    footer.Cell().Text("");
                    footer.Cell().Text("");
                    footer.Cell().BorderBottom(2).BorderColor(Colors.Black);

                    footer.Cell().Element(TotalStyle).Text("");
                    footer.Cell().Element(TotalStyle).Text("");
                    footer.Cell().Element(TotalsLabelStyle).Text("Total").Bold().AlignRight();
                    footer.Cell().Element(TotalStyle).Text(this.order.Total.ToString("C")).Bold();

                    static IContainer TableFooterLineStyle(IContainer container)
                    {
                        return container.PaddingTop(10);
                    }

                    static IContainer TotalsLabelStyle(IContainer container)
                    {
                        return container.Padding(5).PaddingTop(10).PaddingRight(10);
                    }

                    static IContainer TotalStyle(IContainer container)
                    {
                        return container.Padding(5).PaddingTop(10);
                    }
                });
            });
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Text(text =>
        {
            text.Span("Page ");
            text.CurrentPageNumber();
            text.Span(" of ");
            text.TotalPages();
        });
    }

    private static void ConvertSvgToPng(string svgPath, string pngPath)
    {
        SKSvg svg = new();
        svg.Load(svgPath);

        SKBitmap bitmap = new((int)svg.Picture!.CullRect.Width, (int)svg.Picture.CullRect.Height);
        SKCanvas canvas = new(bitmap);
        canvas.Clear(SKColors.Transparent);
        canvas.DrawPicture(svg.Picture);

        using SKImage image = SKImage.FromBitmap(bitmap);
        using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
        using FileStream stream = File.OpenWrite(pngPath);
        data.SaveTo(stream);
    }
}
