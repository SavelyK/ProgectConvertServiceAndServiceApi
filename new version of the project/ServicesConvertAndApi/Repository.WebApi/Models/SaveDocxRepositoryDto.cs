using Repository_Application.Repositorys.Commands.SaveDocxFile;
using Repository_Application.Common.Mappings;

using AutoMapper;
using System;

namespace RepositoryWebApi.Models
{
    public class SaveDocxRepositoryDto : IMapWith<SaveDocxRepositoryCommand>
    {
        

        public string FileName { get; set; }
        public string Path { get; set; }
        public int Priority { get; set; }
        public long FileLength { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDocxRepositoryDto, SaveDocxRepositoryCommand>()
                .ForMember(repositoryCommand => repositoryCommand.FileName,
                opt => opt.MapFrom(repositoryDto => repositoryDto.FileName))
                 .ForMember(repositoryCommand => repositoryCommand.Path,
                opt => opt.MapFrom(repositoryDto => repositoryDto.Path))
                  .ForMember(repositoryCommand => repositoryCommand.Priority,
                opt => opt.MapFrom(repositoryDto => repositoryDto.Priority))
                   .ForMember(repositoryCommand => repositoryCommand.FileLength,
                opt => opt.MapFrom(repositoryDto => repositoryDto.FileLength));

        }
    }
}
