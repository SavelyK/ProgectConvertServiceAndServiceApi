using Repository_Application.Repositorys.Commands.SaveDocxFile;
using Repository_Application.Common.Mappings;

using AutoMapper;
using System;

namespace ConvertService.Models
{
    public class SaveDocxModelDto : IMapWith<SaveDocxRepositoryCommand>
    {
        

        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDocxModelDto, SaveDocxRepositoryCommand>()
                .ForMember(repositoryCommand => repositoryCommand.Id,
                opt => opt.MapFrom(repositoryDto => repositoryDto.Id))
                 .ForMember(repositoryCommand => repositoryCommand.Path,
                opt => opt.MapFrom(repositoryDto => repositoryDto.Path))
                  .ForMember(repositoryCommand => repositoryCommand.Status,
                opt => opt.MapFrom(repositoryDto => repositoryDto.Status));
        }
    }
}
