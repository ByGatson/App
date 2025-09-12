using System.Globalization;
using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{
    public class ProductDataGenerateService
    {
        public void AddUrunler(XElement urunlerNode, Dictionary<string, dynamic> gtipDict)
        {
            int counter = 1;

            foreach (var kvp in gtipDict)
            {
                string malKalemId = Guid.NewGuid().ToString("N");

                urunlerNode.Add(new XElement("MalKalem",
                    new XElement("SiraNo", counter),
                    new XElement("GumrukSiraNo", counter),
                    new XElement("MalKalemId", malKalemId),
                    new XElement("gtip", kvp.Key),
                    new XElement("Cins", kvp.Value.MalinCinsi),
                    new XElement("Mensei", kvp.Value.Ulke),
                    new XElement("GeldigiGidecegiUlke", kvp.Value.Ulke),
                    new XElement("BirinciMiktar", kvp.Value.ToplamMiktar),
                    new XElement("BirinciBirim", kvp.Value.Birim),
                    new XElement("KapTuru", kvp.Value.KapTuru),
                    new XElement("KapAdedi", kvp.Value.ToplamKap),
                    new XElement("MalBedeli", kvp.Value.ToplamTutar.ToString("F2", CultureInfo.InvariantCulture)),
                    new XElement("MalBedeliParaBirimi", kvp.Value.Para),
                    new XElement("NavlunBedeli", "0.00"),
                    new XElement("NavlunBedeliParaBirimi", "USD"),
                    new XElement("SigortaBedeli", "0.00"),
                    new XElement("SigortaBedeliParaBirimi", "USD"),
                    new XElement("BrutAgirlik", kvp.Value.ToplamBrutAgirlik.ToString("F2", CultureInfo.InvariantCulture)),
                    new XElement("MalKalemTeslimSekli", kvp.Value.Teslim)
                ));

                counter++;
            }
        }
    }
}
