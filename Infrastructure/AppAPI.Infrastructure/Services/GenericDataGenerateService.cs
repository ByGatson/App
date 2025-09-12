using AppAPI.Domain.Dto_s;
using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{
    public class GenericDataGenerateService
    {
        public void GenelBilgiler(XElement genelBilgilerNode, GenericDataDto data)
        {
            genelBilgilerNode.Add(
                new XElement("BelgeNo", data.BelgeNo ?? ""),
                new XElement("SerbestBolgeAdi", data.SerbestBolgeAdi ?? ""),
                new XElement("KayitTarihi", data.KayitTarihi?.ToString("yyyy-MM-dd") ?? ""),
                new XElement("DisReferansNo", data.DisReferansNo ?? ""),
                new XElement("GumrukId", data.GumrukId ?? ""),
                new XElement("FormDurumu", data.FormDurumu ?? ""),
                new XElement("FirmaTicaretUnvani", data.FirmaTicaretUnvani ?? ""),
                new XElement("FirmaFaaliyetRuhsatiNo", data.FirmaFaaliyetRuhsatiNo ?? ""),
                new XElement("FirmaFaaliyetRuhsatiKonusu", data.FirmaFaaliyetRuhsatiKonusu ?? ""),
                new XElement("DepoKullanimBelgeliFirma", data.DepoKullanimBelgeliFirma.HasValue ?
                                                        (data.DepoKullanimBelgeliFirma.Value ? "Evet" : "Hayir") : ""),
                new XElement("DepoKullanimBelgeNo", data.DepoKullanimBelgeNo ?? ""),
                new XElement("FormuDolduraninAdi", data.FormuDolduraninAdi ?? ""),
                new XElement("FirmaTelefonu", data.FirmaTelefonu ?? ""),
                new XElement("IslemYonuBilgileri",
                    new XElement("IslemYonu", data.IslemYonu ?? "")
                ),
                new XElement("IslemTuruBilgileri",
                    new XElement("IslemTuru", data.IslemTuru ?? "")
                ),
                new XElement("IslemKonusuBilgileri",
                    new XElement("IslemKonusu", data.IslemKonusu ?? ""),
                    new XElement("IslemKonusuTicari", data.IslemKonusuTicari ?? ""),
                    new XElement("IslemKonusuTicariOlmayan", data.IslemKonusuTicariOlmayan ?? "")
                ),
                new XElement("SevkiyatSekli",
                    new XElement("SevkiyatSekliAdi", data.SevkiyatSekliAdi ?? "")
                ),
                new XElement("ETicaretMi", data.ETicaretMi.HasValue ?
                                           (data.ETicaretMi.Value ? "Evet" : "Hayir") : "")
            );
        }

    }
}
