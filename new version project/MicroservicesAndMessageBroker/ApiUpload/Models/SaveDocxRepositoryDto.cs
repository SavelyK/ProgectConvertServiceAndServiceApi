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
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDocxRepositoryDto, SaveDocxRepositoryCommand>()
                 .ForMember(repositoryCommand => repositoryCommand.Path,
                opt => opt.MapFrom(repositoryDto => repositoryDto.Path));
        }
    }
}
