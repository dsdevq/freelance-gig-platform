using AutoMapper;
using JobService.Application.Events;
using JobService.Application.Models;
using JobService.Domain.Entities;

namespace JobService.Application.Mappings;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<Job, JobModel>();

        CreateMap<CreateJobModel, Job>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => JobStatus.Draft))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateJobModel, Job>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Job, JobCreatedEvent>();
    }
}
