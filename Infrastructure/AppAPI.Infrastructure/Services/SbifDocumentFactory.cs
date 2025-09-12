using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{

    public static class SbifDocumentFactory
    {
        public static class Elements
        {
            public const string Root = "SBIFBilgileri";
            public const string Form = "GenelBilgiler";
            public const string Fatura = "FaturaBilgileri";
            public const string Urunler = "Urunler";
            public const string Toplamlar = "Toplamlar";
            public const string Ek = "EkBilgiler";
        }

        public static XDocument CreateDocument()
        {
            return new XDocument(
                new XElement(Elements.Root,
                    new XElement(Elements.Form),
                    new XElement(Elements.Fatura),
                    new XElement(Elements.Urunler),
                    new XElement(Elements.Toplamlar),
                    new XElement(Elements.Ek)
                )
            );
        }
    }
}
