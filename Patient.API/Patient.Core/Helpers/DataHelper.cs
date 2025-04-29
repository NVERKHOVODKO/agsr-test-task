using System.Globalization;
using Patient.Core.Constants;
using Patient.Core.Helpers.Interfaces;

namespace Patient.Core.Helpers;

public class DataHelper : IDataHelper
{
    private const int ApproximateRangeMonths = 1;
    
    public (string prefix, DateTime dateTime) ParseFhirDateParameter(string fhirDateParam)
    {
        if (string.IsNullOrEmpty(fhirDateParam))
            throw new ArgumentException("Date parameter is required");

        var prefix = new string(fhirDateParam.Take(2).Where(char.IsLetter).ToArray()).ToLower();
        var datePart = fhirDateParam[prefix.Length..];

        DateTime parsedDate;
        switch (datePart.Length)
        {
            case 4: // Year only
                parsedDate = new DateTime(int.Parse(datePart), 1, 1);
                break;
            case 7: // Year-Month
            {
                var parts = datePart.Split('-');
                parsedDate = new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), 1);
                break;
            }
            default: // Full date
            {
                if (!DateTime.TryParse(datePart, CultureInfo.InvariantCulture, 
                        DateTimeStyles.AssumeLocal, out parsedDate))
                {
                    throw new ArgumentException("Invalid date format");
                }
                break;
            }
        }

        return (prefix, parsedDate);
    }

    public (DateTime? start, DateTime? end) CalculateDateRange(string prefix, DateTime date)
    {
        var (periodStart, periodEnd) = GetPeriodBounds(date);

        return prefix switch
        {
            DateConstants.EqualPrefix => (periodStart, periodEnd),
            DateConstants.NotEqualPrefix => (periodStart, periodStart),
            DateConstants.LessThanPrefix => (null, periodStart),
            DateConstants.LessOrEqualPrefix => (null, periodEnd),
            DateConstants.GreaterThanPrefix => (periodEnd, null),
            DateConstants.GreaterOrEqualPrefix => (periodStart, null),
            DateConstants.StartsAfterPrefix => (periodEnd, null),
            DateConstants.EndsBeforePrefix => (null, periodStart),
            DateConstants.ApproximatelyPrefix => (
                periodStart.AddMonths(-ApproximateRangeMonths),
                periodEnd.AddMonths(ApproximateRangeMonths)),
            _ => (periodStart, periodEnd)
        };
    }

    private (DateTime periodStart, DateTime periodEnd) GetPeriodBounds(DateTime date)
    {
        if (date.TimeOfDay != TimeSpan.Zero)
            return (date, date);

        return date switch
        {
            { Day: 1, Month: 1, Hour: 0, Minute: 0, Second: 0 } => (date, new DateTime(date.Year, 12, 31, 23, 59, 59)),
            { Day: 1, Hour: 0, Minute: 0, Second: 0 } => (date,
                new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59)),
            _ => (date.Date, date.Date.AddDays(1).AddTicks(-1))
        };
    } 
}