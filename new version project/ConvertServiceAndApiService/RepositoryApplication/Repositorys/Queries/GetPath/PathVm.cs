using AutoMapper;
using RepositoryApplication.Common.Mappings;
using RepositoryDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryApplication.Repositorys.Queries.GetPath
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