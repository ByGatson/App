using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{
    public class InvoiceDataGenerateService
    {
        public void AddFaturaBilgisi(XElement faturaBilgileriNode, XDocument doc)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";

            var root = doc.Root;
            var supplier = root.Element(cac + "AccountingSupplierParty");
            var customer = root.Element(cac + "AccountingCustomerParty");

            var fatura = new XElement("Fatura",
                new XElement("FaturaTuru", "Fatura"),
                new XElement("FaturaTarih", root.Element(cbc + "IssueDate")?.Value ?? ""),
                new XElement("FaturaNo", root.Element(cbc + "ID")?.Value ?? ""),
                new XElement("TeslimAdresi",
                    new XElement("AdiTicaretUnvani", supplier?.Descendants(cbc + "RegistrationName").FirstOrDefault()?.Value ?? ""),
                    new XElement("Adresi", supplier?.Descendants(cac + "AddressLine").Descendants(cbc + "Line").FirstOrDefault()?.Value ?? ""),
                    new XElement("PostaKodu", supplier?.Descendants(cbc + "PostalZone").FirstOrDefault()?.Value ?? ""),
                    new XElement("Ulke", supplier?.Descendants(cac + "Country").Descendants(cbc + "Name").FirstOrDefault()?.Value ?? ""),
                    new XElement("Il", supplier?.Descendants(cbc + "CityName").FirstOrDefault()?.Value ?? ""),
                    new XElement("Ilce", supplier?.Descendants(cbc + "Region").FirstOrDefault()?.Value ?? ""),
                    new XElement("Telefon1", supplier?.Descendants(cbc + "Telephone").FirstOrDefault()?.Value ?? ""),
                    new XElement("Faks1", supplier?.Descendants(cbc + "Telefax").FirstOrDefault()?.Value ?? ""),
                    new XElement("EPosta", supplier?.Descendants(cbc + "ElectronicMail").FirstOrDefault()?.Value ?? "")
                ),
                new XElement("KarsiFirmaBilgisi",
                    new XElement("VergiKimlikNo", customer?.Descendants(cbc + "CompanyID").FirstOrDefault()?.Value ?? ""),
                    new XElement("VergiDairesi", customer?.Descendants(cbc + "TaxScheme").Descendants(cbc + "Name").FirstOrDefault()?.Value ?? ""),
                    new XElement("AdiUnvani", customer?.Descendants(cbc + "RegistrationName").FirstOrDefault()?.Value ?? ""),
                    new XElement("Adresi", customer?.Descendants(cac + "AddressLine").Descendants(cbc + "Line").FirstOrDefault()?.Value ?? ""),
                    new XElement("PostaKodu", customer?.Descendants(cbc + "PostalZone").FirstOrDefault()?.Value ?? ""),
                    new XElement("Ulke", customer?.Descendants(cac + "Country").Descendants(cbc + "Name").FirstOrDefault()?.Value ?? ""),
                    new XElement("Il", customer?.Descendants(cbc + "CityName").FirstOrDefault()?.Value ?? ""),
                    new XElement("Ilce", customer?.Descendants(cbc + "Region").FirstOrDefault()?.Value ?? ""),
                    new XElement("Telefon1", customer?.Descendants(cbc + "Telephone").FirstOrDefault()?.Value ?? ""),
                    new XElement("Faks1", customer?.Descendants(cbc + "Telefax").FirstOrDefault()?.Value ?? ""),
                    new XElement("EPosta", customer?.Descendants(cbc + "ElectronicMail").FirstOrDefault()?.Value ?? "")
                ),
                new XElement("OdemeSekli", root.Descendants(cbc + "PaymentMeans").Descendants(cbc + "PaymentMeansCode").FirstOrDefault()?.Value ?? ""),
                new XElement("FaturaTeslimSekli", root.Descendants(cbc + "ID").Where(e => e.Attribute("schemeID")?.Value == "INCOTERMS").FirstOrDefault()?.Value ?? ""),
                new XElement("FaturaTutari", root.Descendants(cbc + "LegalMonetaryTotal").Descendants(cbc + "PayableAmount").FirstOrDefault()?.Value ?? ""),
                new XElement("FaturaParaBirimi", root.Descendants(cbc + "LegalMonetaryTotal").Descendants(cbc + "PayableAmount").FirstOrDefault()?.Attribute("currencyID")?.Value ?? "")
            );

            faturaBilgileriNode.Add(fatura);
        }
    }
}
