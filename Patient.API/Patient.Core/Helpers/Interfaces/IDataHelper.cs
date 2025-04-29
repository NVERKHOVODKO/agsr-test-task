namespace Patient.Core.Helpers.Interfaces;

public interface IDataHelper
{
    (string prefix, DateTime dateTime) ParseFhirDateParameter(string fhirDateParam);
    
    (DateTime? start, DateTime? end) CalculateDateRange(string prefix, DateTime date);
}