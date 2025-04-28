namespace Patient.Core.Enums;

/// <summary>
/// Represents a patient's gender identity.
/// </summary>
public enum Gender
{
    /// <summary>
    /// Male gender identity.
    /// </summary>
    Male,

    /// <summary>
    /// Female gender identity.
    /// </summary>
    Female,

    /// <summary>
    /// Other gender identity not represented by Male or Female.
    /// </summary>
    Other,

    /// <summary>
    /// Gender identity is not known or not specified.
    /// </summary>
    Unknown
}