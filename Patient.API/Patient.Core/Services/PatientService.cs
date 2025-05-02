using AutoMapper;
using Patient.Core.DTOs;
using Patient.Core.Exceptions;
using Patient.Core.Helpers.Interfaces;
using Patient.Core.Repositories.Interfaces;
using Patient.Core.Services.Interfaces;

namespace Patient.Core.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<Models.Patient> _patientRepository;
    private readonly IMapper _mapper;
    private readonly IDataHelper _dataHelper;

    public PatientService(
        IRepository<Models.Patient> patientRepository, 
        IMapper mapper,
        IDataHelper dataHelper)
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
        if (string.IsNullOrEmpty(patient.Name?.Family))
            throw new BadRequestException("Family name is required.");

        if (patient.BirthDate == default)
            throw new BadRequestException("Birth date is required.");

        var model = _mapper.Map<Core.Models.Patient>(patient);
        model.Id = Guid.NewGuid();
        model.Name.Id = Guid.NewGuid();

        await _patientRepository.AddAsync(model);

        return _mapper.Map<GetPatientDto>(model);
    }

    public async Task UpdateAsync(Guid id, UpdatePatientDto patient)
    {
        if (id != patient.Id)
            throw new BadRequestException("ID in URL does not match patient ID.");

        if (!await IsExistsAsync(id))
            throw new NotFoundException($"Patient with ID {id} not found.");

        var model = _mapper.Map<Core.Models.Patient>(patient);

        await _patientRepository.UpdateAsync(model);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await IsExistsAsync(id))
            throw new NotFoundException($"Patient with ID {id} not found.");

        await _patientRepository.DeleteAsync(id);
    }

    private async Task<bool> IsExistsAsync(Guid id) => await _patientRepository.IsExists(id);
    
    public async Task<IEnumerable<GetPatientDto>> SearchByBirthDateAsync(string dateParam)
    {
        var (prefix, dateTime) = _dataHelper.ParseFhirDateParameter(dateParam);
        var (startDate, endDate) = _dataHelper.CalculateDateRange(prefix, dateTime);
        
        var patients = await _patientRepository.SearchByBirthDateAsync(startDate, endDate, prefix);
        
        return _mapper.Map<IEnumerable<GetPatientDto>>(patients);
    }
}