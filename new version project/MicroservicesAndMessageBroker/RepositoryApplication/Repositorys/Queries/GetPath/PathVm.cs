using AutoMapper;
using Repository_Application.Common.Mappings;
using RepositoryDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Application.Repositorys.Queries.GetPath
{
    public class PathVm : IMapWith<Repository>
    {
        public string Path { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Repository, PathVm>()
                .ForMember(repositoryVm => repositoryVm.Path,
                    opt => opt.MapFrom(repository => repository.Path));


        }
    }
}
