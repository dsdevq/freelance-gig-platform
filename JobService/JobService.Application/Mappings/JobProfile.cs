using AutoMapper;
using JobService.Application.Models;
using JobService.Domain.Entities;

namespace JobService.Application.Mappings;

public class JobProfile : Profile
{
    public JobProfile()
    {
        // Domain → DTO
        CreateMap<Job, JobModel>();

        // Create model → Domain
        CreateMap<CreateJobModel, Job>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => JobStatus.Draft))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Update model → Domain (partial updates handled manually)
        CreateMap<UpdateJobModel, Job>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
