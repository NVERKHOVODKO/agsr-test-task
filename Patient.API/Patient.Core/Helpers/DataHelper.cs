using System.Globalization;
using Patient.Core.Constants;

namespace Patient.Core.Helpers;

public class DataHelper
{
    private const int ApproximateRangeMonths = 1;
    
    public (string Prefix, DateTime DateTime) ParseFhirDateParameter(string fhirDateParam)
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

    public (DateTime? Start, DateTime? End) CalculateDateRange(string prefix, DateTime date)
    {
        var (periodStart, periodEnd) = GetPeriodBounds(date);

        return prefix switch
        {
            DatePrefix.Equal => (periodStart, periodEnd),
            DatePrefix.NotEqual => (periodStart, periodStart),
            DatePrefix.LessThan => (null, periodStart),
            DatePrefix.LessOrEqual => (null, periodEnd),
            DatePrefix.GreaterThan => (periodEnd, null),
            DatePrefix.GreaterOrEqual => (periodStart, null),
            DatePrefix.StartsAfter => (periodEnd, null),
            DatePrefix.EndsBefore => (null, periodStart),
            DatePrefix.Approximately => (
                periodStart.AddMonths(-ApproximateRangeMonths),
                periodEnd.AddMonths(ApproximateRangeMonths)),
            _ => (periodStart, periodEnd)
        };
    }

    private (DateTime PeriodStart, DateTime PeriodEnd) GetPeriodBounds(DateTime date)
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