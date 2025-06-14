using DinkToPdf;
using DinkToPdf.Contracts;

namespace Carpenter.Services
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GenerateReceiptPdf(string userName, string plan, string subscriptionId)
        {
            var htmlContent = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; padding: 20px; }}
                        h1 {{ color: #333; }}
                        .details {{ margin-top: 20px; }}
                    </style>
                </head>
                <body>
                    <h1>Subscription Receipt</h1>
                    <p>Thank you, <strong>{userName}</strong>, for subscribing to our service.</p>

                    <div class='details'>
                        <p><strong>Subscription ID:</strong> {subscriptionId}</p>
                        <p><strong>Plan:</strong> {plan}</p>
                        <p><strong>Date:</strong> {DateTime.Now.ToString("MMMM dd, yyyy")}</p>
                        <p><strong>Status:</strong> Active</p>
                    </div>
                </body>
                </html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = htmlContent
                    }
                }
            };

            return _converter.Convert(doc);
        }
    }
}
