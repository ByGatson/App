using AppAPI.Application.Interfaces;
using AppAPI.Domain.Dto_s;
using System.Xml.Linq;

namespace AppAPI.Infrastructure.Services
{
    public class GenerateSbifService : IGenerateSbifService
    {

        public Task<MemoryStream> GenerateSbifAsync(List<FileUploadDto> files, GenericDataDto genericDataDto)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("En az bir dosya yüklemelisiniz.");

            XDocument sbifDoc = SbifDocumentFactory.CreateDocument();
            XElement formNode = sbifDoc.Root.Element(SbifDocumentFactory.Elements.Form);
            XElement urunlerNode = sbifDoc.Root.Element(SbifDocumentFactory.Elements.Urunler);
            XElement toplamlarNode = sbifDoc.Root.Element(SbifDocumentFactory.Elements.Toplamlar);
            XElement faturaBilgileriNode = sbifDoc.Root.Element(SbifDocumentFactory.Elements.Fatura);

            int toplamMiktar = 0;
            decimal toplamTutar = 0;
            int toplamKap = 0;
            decimal toplamBrutAgirlik = 0;

            var gtipDict = new Dictionary<string, dynamic>();
            bool ilkDosya = true;

            foreach (var file in files)
            {
                using var stream = new MemoryStream(file.Content);
                XDocument doc = XDocument.Load(stream);

                if (ilkDosya)
                {
                    new GenericDataGenerateService().GenelBilgiler(formNode, genericDataDto);
                    ilkDosya = false;
                }

                new InvoiceDataGenerateService().AddFaturaBilgisi(faturaBilgileriNode, doc);
                new ProcessInvoiceLinesService().ProcessInvoiceLines(doc, gtipDict, ref toplamMiktar, ref toplamTutar);
            }

            new ProductDataGenerateService().AddUrunler(urunlerNode, gtipDict);
            new TotalDataGenerateService().AddToplamlar(toplamlarNode, toplamMiktar, toplamKap, toplamBrutAgirlik, toplamTutar);

            var memoryStream = new MemoryStream();
            sbifDoc.Save(memoryStream);
            memoryStream.Position = 0;

            return Task.FromResult(memoryStream);
        }
    }
}
