using AutoMapper;
using Patient.Core.DAL.Models;
using Patient.Core.DTOs;

namespace Patient.Core.Mappings;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<CreatePatientDto, DAL.Models.Patient>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => 
            {
                opt.PreCondition(src => src.Name is not null);
                opt.MapFrom(src => new PatientName
                {
                    Id = Guid.NewGuid(),
                    Use = src.Name!.Use,
                    Family = src.Name!.Family ?? string.Empty,
                    Given = src.Name!.Given
                        .Select(g => new PatientGivenName
                        {
                            Id = Guid.NewGuid(),
                            Value = g
                        })
                        .ToList()
                });
            });

        CreateMap<UpdatePatientDto, DAL.Models.Patient>()
            .ForMember(dest => dest.Name, opt => 
            {
                opt.PreCondition(src => src.Name != null);
                opt.MapFrom(src => new PatientName
                {
                    Use = src.Name!.Use,
                    Family = src.Name!.Family ?? string.Empty,
                    Given = src.Name!.Given
                        .Select(g => new PatientGivenName
                        {
                            Value = g
                        })
                        .ToList()
                });
            })
            .ForAllMembers(opts => opts.Condition((srcMember) => srcMember is not null));
        
        CreateMap<DAL.Models.Patient, GetPatientDto>();
        
        CreateMap<PatientName, PatientNameDto>()
            .ForMember(dest => dest.Given, opt => 
            {
                opt.MapFrom(src => src.Given.Select(g => g.Value));
            });
    }
}