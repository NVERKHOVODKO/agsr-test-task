using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Patient.Core.Constants;
using Patient.Core.DAL.Repositories.Interfaces;
using Patient.Core.DTOs;
using Patient.Core.Enums;
using Patient.Core.Helpers;
using Patient.Core.Services.Interfaces;

namespace Patient.Core.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<DAL.Models.Patient> _patientRepository;
    private readonly IMapper _mapper;
    private readonly DataHelper _dataHelper;

    public PatientService(
        IRepository<DAL.Models.Patient> patientRepository, 
        IMapper mapper,
        DataHelper dataHelper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
        _dataHelper = dataHelper;
    }

    public async Task<IEnumerable<GetPatientDto>> GetAllAsync()
    {
        var models = await _patientRepository.GetAllAsync();
        return _mapper.Map<List<GetPatientDto>>(models);
    }

    public async Task<GetPatientDto?> GetByIdAsync(Guid id)
    {
        var model = await _patientRepository.GetByIdAsync(id);
        var patient = _mapper.Map<GetPatientDto>(model);
        return patient;
    }

    public async Task<GetPatientDto> CreateAsync(CreatePatientDto patient)
    {
        var model = _mapper.Map<DAL.Models.Patient>(patient);
        await _patientRepository.AddAsync(model);

        return _mapper.Map<GetPatientDto>(model);
    }

    public async Task<Status> UpdateAsync(Guid id, UpdatePatientDto patient)
    {
       if (!await IsExistsAsync(id))
            return Status.NotFound;

       var model = _mapper.Map<DAL.Models.Patient>(patient);

       await _patientRepository.UpdateAsync(model);

       return Status.Success;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _patientRepository.DeleteAsync(id);
    }

    private async Task<bool> IsExistsAsync(Guid id) => await _patientRepository.IsExists(id);
    
    public async Task<IEnumerable<GetPatientDto>> SearchByBirthDateAsync(string dateParam)
    {
        var parsedParam = _dataHelper.ParseFhirDateParameter(dateParam);
        var date = _dataHelper.CalculateDateRange(parsedParam.Prefix, parsedParam.DateTime);

        var query = _patientRepository.GetQueryable();
            
        if (date.Start == date.End && date.Start.HasValue && parsedParam.Prefix == DatePrefix.NotEqual)
            query = query.Where(p => p.BirthDate != date.Start.Value.Date);
        else
        {
            if (date.Start.HasValue)
                query = query.Where(p => p.BirthDate >= date.Start.Value);
        
            if (date.End.HasValue)
                query = query.Where(p => p.BirthDate <= date.End.Value);
        }
        
        var patients = await query.ToListAsync();
        
        return _mapper.Map<IEnumerable<GetPatientDto>>(patients);
    }
}