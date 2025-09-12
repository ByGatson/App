using System.Globalization;
using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{
    class TotalDataGenerateService
    {
        public void AddToplamlar(XElement toplamlarNode, int toplamMiktar, int toplamKap, decimal toplamBrutAgirlik, decimal toplamTutar)
        {
            toplamlarNode.Add(
                new XElement("ToplamMiktar", toplamMiktar),
                new XElement("ToplamKap", toplamKap),
                new XElement("ToplamBrutAgirlik", toplamBrutAgirlik.ToString("F2", CultureInfo.InvariantCulture)),
                new XElement("ToplamTutar", toplamTutar.ToString("F2", CultureInfo.InvariantCulture)),
                new XElement("ParaBirimi", "EUR")
            );
        }
    }
}
