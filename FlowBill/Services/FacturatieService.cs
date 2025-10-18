using FlowBill.Data;
using FlowBill.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MailKit.Net.Smtp;
using MimeKit;

namespace FlowBill.Services
{
    public class FacturatieService : IFacturatieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public FacturatieService(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;

            // QuestPDF License
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<Factuur> GenereerFactuur(int bestellingId)
        {
            var bestelling = await _context.Bestellingen
                .Include(b => b.Klant)
                .Include(b => b.BestellingItems)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.Id == bestellingId);

            if (bestelling == null)
                throw new ArgumentException("Bestelling niet gevonden");

            // Check of er al een factuur bestaat
            var bestaandeFactuur = await _context.Facturen
                .FirstOrDefaultAsync(f => f.BestellingId == bestellingId);

            if (bestaandeFactuur != null)
                return bestaandeFactuur;

            // Genereer uniek factuurnummer
            var jaar = DateTime.Now.Year;
            var laatsteFactuur = await _context.Facturen
                .Where(f => f.FactuurNummer.StartsWith($"FAC{jaar}"))
                .OrderByDescending(f => f.FactuurNummer)
                .FirstOrDefaultAsync();

            int volgNummer = 1;
            if (laatsteFactuur != null)
            {
                var laatsteNummer = laatsteFactuur.FactuurNummer.Substring(7);
                if (int.TryParse(laatsteNummer, out int nummer))
                {
                    volgNummer = nummer + 1;
                }
            }

            var factuur = new Factuur
            {
                FactuurNummer = $"FAC{jaar}{volgNummer:D4}",
                BestellingId = bestellingId,
                FactuurDatum = DateTime.Now,
                VervalDatum = DateTime.Now.AddDays(30),
                SubTotaal = bestelling.SubTotaal,
                BTWBedrag = bestelling.BTWBedrag,
                Totaal = bestelling.Totaal,
                Status = FactuurStatus.Openstaand
            };

            _context.Facturen.Add(factuur);
            await _context.SaveChangesAsync();

            // Genereer PDF
            var pdfBytes = await GenereerPDF(factuur);

            // Sla PDF op
            var pdfFolder = Path.Combine(_environment.WebRootPath, "facturen");
            if (!Directory.Exists(pdfFolder))
                Directory.CreateDirectory(pdfFolder);

            var pdfPath = Path.Combine(pdfFolder, $"{factuur.FactuurNummer}.pdf");
            await File.WriteAllBytesAsync(pdfPath, pdfBytes);

            factuur.PDFPad = $"/facturen/{factuur.FactuurNummer}.pdf";
            await _context.SaveChangesAsync();

            // Verstuur email
            await VerstuurFactuurEmail(factuur);

            return factuur;
        }

        public async Task<byte[]> GenereerPDF(Factuur factuur)
        {
            var factuurMetData = await _context.Facturen
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.Klant)
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.BestellingItems)
                        .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(f => f.Id == factuur.Id);

            if (factuurMetData == null)
                factuurMetData = factuur;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Pagina ");
                        text.CurrentPageNumber();
                        text.Span(" van ");
                        text.TotalPages();
                    });

                    void ComposeHeader(IContainer container)
                    {
                        container.Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("FACTUUR").FontSize(24).Bold();
                                column.Item().Text($"Factuurnummer: {factuurMetData.FactuurNummer}");
                                column.Item().Text($"Datum: {factuurMetData.FactuurDatum:dd-MM-yyyy}");
                                column.Item().Text($"Vervaldatum: {factuurMetData.VervalDatum:dd-MM-yyyy}");
                            });

                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("FlowBill B.V.").Bold().FontSize(16);
                                column.Item().Text("Hoofdstraat 123");
                                column.Item().Text("1234 AB Amsterdam");
                                column.Item().Text("info@flowbill.nl");
                                column.Item().Text("KvK: 12345678");
                                column.Item().Text("BTW: NL123456789B01");
                            });
                        });
                    }

                    void ComposeContent(IContainer container)
                    {
                        container.PaddingVertical(40).Column(column =>
                        {
                            column.Spacing(5);

                            // Klantgegevens
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Component(new KlantComponent(factuurMetData.Bestelling.Klant));
                            });

                            column.Item().PaddingTop(20);

                            // Tabel met items
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Product");
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Aantal");
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Prijs");
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("BTW %");
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Totaal");
                                });

                                foreach (var item in factuurMetData.Bestelling.BestellingItems)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Product.Naam);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Aantal.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"€ {item.PrijsPerStuk:F2}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.BTWPercentage}%");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"€ {item.Totaal:F2}");
                                }
                            });

                            // Totalen
                            column.Item().AlignRight().PaddingTop(20).Column(col =>
                            {
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("Subtotaal:");
                                    row.ConstantItem(100).Text($"€ {factuurMetData.SubTotaal:F2}").AlignRight();
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("BTW:");
                                    row.ConstantItem(100).Text($"€ {factuurMetData.BTWBedrag:F2}").AlignRight();
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("Totaal:").Bold();
                                    row.ConstantItem(100).Text($"€ {factuurMetData.Totaal:F2}").Bold().AlignRight();
                                });
                            });

                            // Betalingsinformatie
                            column.Item().PaddingTop(40).Text("Betalingsinformatie:").Bold();
                            column.Item().Text("IBAN: NL12 ABNA 0123 4567 89");
                            column.Item().Text("BIC: ABNANL2A");
                            column.Item().Text($"Onder vermelding van: {factuurMetData.FactuurNummer}");
                        });
                    }
                });
            });

            return document.GeneratePdf();
        }

        public async Task<bool> VerstuurFactuurEmail(Factuur factuur)
        {
            try
            {
                var factuurMetData = await _context.Facturen
                    .Include(f => f.Bestelling)
                        .ThenInclude(b => b.Klant)
                    .FirstOrDefaultAsync(f => f.Id == factuur.Id);

                if (factuurMetData == null)
                    return false;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("FlowBill", _configuration["Email:From"]));
                message.To.Add(new MailboxAddress(
                    factuurMetData.Bestelling.Klant.Contactpersoon ?? factuurMetData.Bestelling.Klant.Bedrijfsnaam,
                    factuurMetData.Bestelling.Klant.Email));
                message.Subject = $"Factuur {factuurMetData.FactuurNummer} - FlowBill";

                var builder = new BodyBuilder();
                builder.HtmlBody = $@"
                    <h2>Beste {factuurMetData.Bestelling.Klant.Contactpersoon ?? factuurMetData.Bestelling.Klant.Bedrijfsnaam},</h2>
                    <p>Bijgaand treft u factuur {factuurMetData.FactuurNummer} aan.</p>
                    <p><strong>Factuurbedrag:</strong> € {factuurMetData.Totaal:F2}<br/>
                    <strong>Vervaldatum:</strong> {factuurMetData.VervalDatum:dd-MM-yyyy}</p>
                    <p>Gelieve het bedrag voor de vervaldatum over te maken op:<br/>
                    IBAN: NL12 ABNA 0123 4567 89<br/>
                    Onder vermelding van: {factuurMetData.FactuurNummer}</p>
                    <p>Met vriendelijke groet,<br/>
                    FlowBill</p>
                ";

                // Voeg PDF toe als bijlage
                var pdfBytes = await GenereerPDF(factuurMetData);
                builder.Attachments.Add($"{factuurMetData.FactuurNummer}.pdf", pdfBytes, ContentType.Parse("application/pdf"));

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration["Email:Host"],
                        int.Parse(_configuration["Email:Port"]),
                        bool.Parse(_configuration["Email:UseSsl"]));

                    await client.AuthenticateAsync(
                        _configuration["Email:Username"],
                        _configuration["Email:Password"]);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                // Update factuur status
                factuurMetData.EmailVerzonden = true;
                factuurMetData.EmailVerzondenOp = DateTime.Now;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private class KlantComponent : IComponent
        {
            private Klant _klant;

            public KlantComponent(Klant klant)
            {
                _klant = klant;
            }

            public void Compose(IContainer container)
            {
                container.Column(column =>
                {
                    column.Item().Text("Factuuradres:").Bold();
                    column.Item().Text(_klant.Bedrijfsnaam);
                    if (!string.IsNullOrEmpty(_klant.Contactpersoon))
                        column.Item().Text($"T.a.v. {_klant.Contactpersoon}");
                    column.Item().Text(_klant.Adres);
                    column.Item().Text($"{_klant.Postcode} {_klant.Stad}");
                    if (!string.IsNullOrEmpty(_klant.BTWNummer))
                        column.Item().Text($"BTW: {_klant.BTWNummer}");
                });
            }
        }
    }
}