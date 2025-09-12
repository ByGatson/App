using System.Globalization;
using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{
    public class ProcessInvoiceLinesService
    {
        public void ProcessInvoiceLines(
    XDocument doc,
    Dictionary<string, dynamic> gtipDict,
    ref int toplamMiktar,
    ref decimal toplamTutar)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";

            var invoiceLines = doc.Descendants(cac + "InvoiceLine");

            foreach (var line in invoiceLines)
            {
                string gtip = line.Descendants(cbc + "RequiredCustomsID").FirstOrDefault()?.Value;
                if (string.IsNullOrWhiteSpace(gtip))
                    continue;

                string miktarStr = line.Element(cbc + "InvoicedQuantity")?.Value ?? "0";
                string fiyatStr = line.Element(cbc + "LineExtensionAmount")?.Value ?? "0";
                string birim = line.Element(cbc + "InvoicedQuantity")?.Attribute("unitCode")?.Value ?? "NIU";

                int.TryParse(miktarStr, NumberStyles.Any, CultureInfo.InvariantCulture, out int miktarInt);
                decimal.TryParse(fiyatStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal fiyatDec);

                toplamMiktar += miktarInt;
                toplamTutar += fiyatDec;

                string teslimSekli = line.Descendants(cbc + "ID")
                                         .Where(e => e.Attribute("schemeID")?.Value == "INCOTERMS")
                                         .FirstOrDefault()?.Value ?? "EXW";

                string malinCinsi = line.Descendants(cac + "SellersItemIdentification")
                                        .Descendants(cbc + "ID").FirstOrDefault()?.Value ?? "";

                string para = line.Element(cbc + "LineExtensionAmount")?.Attribute("currencyID")?.Value ?? "EUR";

                string kapAdediStr = line.Descendants(cac + "ActualPackage")
                                         .Descendants(cbc + "Quantity").FirstOrDefault()?.Value ?? "0";
                int.TryParse(kapAdediStr, NumberStyles.Any, CultureInfo.InvariantCulture, out int kapAdedi);

                string kapTuru = line.Descendants(cac + "ActualPackage")
                                     .Descendants(cbc + "PackagingTypeCode").FirstOrDefault()?.Value ?? "";

                string ulke = line.Descendants(cac + "DeliveryAddress")
                                  .Descendants(cac + "Country")
                                  .Descendants(cbc + "Name").FirstOrDefault()?.Value ?? "Türkiye";

                string brutAgirlikStr = line.Descendants(cbc + "GrossWeightMeasure").FirstOrDefault()?.Value ?? "0.00";
                decimal.TryParse(brutAgirlikStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal brutAgirlik);

                if (!gtipDict.ContainsKey(gtip))
                {
                    gtipDict[gtip] = new
                    {
                        MalinCinsi = malinCinsi,
                        Birim = birim,
                        Para = para,
                        Teslim = teslimSekli,
                        ToplamMiktar = 0,
                        ToplamTutar = 0m,
                        ToplamKap = 0,
                        KapTuru = kapTuru,
                        Ulke = ulke,
                        ToplamBrutAgirlik = 0m
                    };
                }

                dynamic urun = gtipDict[gtip];
                gtipDict[gtip] = new
                {
                    urun.MalinCinsi,
                    urun.Birim,
                    urun.Para,
                    urun.Teslim,
                    ToplamMiktar = urun.ToplamMiktar + miktarInt,
                    ToplamTutar = urun.ToplamTutar + fiyatDec,
                    ToplamKap = urun.ToplamKap + kapAdedi,
                    urun.KapTuru,
                    urun.Ulke,
                    ToplamBrutAgirlik = urun.ToplamBrutAgirlik + brutAgirlik
                };
            }
        }
    }
}
