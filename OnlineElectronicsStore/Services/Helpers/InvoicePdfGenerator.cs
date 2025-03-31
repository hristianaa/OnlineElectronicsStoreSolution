using OnlineElectronicsStore.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OnlineElectronicsStore.Services.Helpers
{
    public static class InvoicePdfGenerator
    {
        public static byte[] GeneratePdf(Invoice invoice)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"INVOICE #{invoice.Id}").FontSize(24).Bold();
                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Date: {invoice.InvoiceDate:yyyy-MM-dd}");
                        col.Item().Text($"Billing Email: {invoice.BillingEmail}");
                        col.Item().Text($"Total: ${invoice.TotalAmount}");

                        col.Item().LineHorizontal(1);
                        col.Item().Text("Items:").FontSize(16).Bold();

                        foreach (var item in invoice.Order.OrderItems)
                        {
                            col.Item().Text($"• {item.Product.Name} - Qty: {item.Quantity} - Price: ${item.UnitPrice}");
                        }
                    });

                    page.Footer().AlignCenter().Text("Thank you for your order!").FontSize(12).Italic();
                });
            }).GeneratePdf();
        }
    }
}
