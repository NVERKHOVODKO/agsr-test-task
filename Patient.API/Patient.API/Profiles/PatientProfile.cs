using AutoMapper;
using Patient.API.DTOs;
using Patient.API.Models;

namespace Patient.API.Profiles;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<CreatePatientDto, Models.Patient>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new PatientName
            {
                Id = Guid.NewGuid(),
                Use = src.Name.Use,
                Family = src.Name.Family,
                Given = src.Name.Given.Select(g => new PatientGivenName
                {
                    Id = Guid.NewGuid(),
                    Value = g
                }).ToList()
            }));

        CreateMap<UpdatePatientDto, Models.Patient>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new PatientName
            {
                Use = src.Name.Use,
                Family = src.Name.Family,
                Given = src.Name.Given.Select(g => new PatientGivenName
                {
                    Value = g
                }).ToList()
            }))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<Models.Patient, GetPatientDto>();
        CreateMap<PatientName, PatientNameDto>()
            .ForMember(dest => dest.Given, opt => opt.MapFrom(src => src.Given.Select(g => g.Value)));
    }
}