using AppAPI.Domain.Dto_s;

namespace AppAPI.Application.Interfaces
{
    public interface IGenerateSbifService
    {
        Task<MemoryStream> GenerateSbifAsync(List<FileUploadDto> files, GenericDataDto genericDataDto);
    }
}
