using RepositoryApplication.Common.Mappings;
using RepositoryDomain;
using System;
using AutoMapper;
using System.Text;

namespace RepositoryApplication.Repositorys.Queries.GetTaskStatus
{
    public class TaskStatusVm : IMapWith<Repository>
    {
        public string FileName { get; set; }

        public string Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Repository, TaskStatusVm>()
                .ForMember(repositoryVm => repositoryVm.FileName,
                    opt => opt.MapFrom(repository => repository.FileName))
                .ForMember(repositoryVm => repositoryVm.Status,
                    opt => opt.MapFrom(repository => repository.Status));

        }

    }
}