using System.Globalization;
using System.Text.Json.Serialization;

namespace Geraldic_Signs_Library
{
    /// <summary>
    /// Represents a Geraldic Sign.
    /// </summary>
    public class Geraldic_Signs
    {
        [JsonPropertyName("Name")]
        string _name;
        [JsonPropertyName("Type")]
        string _type;
        [JsonPropertyName("Picture")]
        string _picture;
        [JsonPropertyName("Description")]
        string _description;
        [JsonPropertyName("Semantics")]
        string _semantics;
        [JsonPropertyName("CertificateHolderName")]
        string _certificateholdername;
        [JsonPropertyName("RegistrationDate")]
        string _registrationdate;
        [JsonPropertyName("RegistrationNumber")]
        string _registrationnumber;
        [JsonPropertyName("global_id")]
        decimal _globalid;

        /// <summary>
        /// Gets the name of the Geraldic Sign.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the type of the Geraldic Sign.
        /// </summary>
        public string Type => _type;

        /// <summary>
        /// Gets the picture of the Geraldic Sign.
        /// </summary>
        public string Picture => _picture;

        /// <summary>
        /// Gets the description of the Geraldic Sign.
        /// </summary>
        public string Description => _description;

        /// <summary>
        /// Gets the semantics of the Geraldic Sign.
        /// </summary>
        public string Semantics => _semantics;

        /// <summary>
        /// Gets the name of the certificate holder of the Geraldic Sign.
        /// </summary>
        public string CertificateHolderName => _certificateholdername;

        /// <summary>
        /// Gets the registration date of the Geraldic Sign.
        /// </summary>
        public string RegistrationDate => _registrationdate;

        /// <summary>
        /// Gets the registration number of the Geraldic Sign.
        /// </summary>
        public string RegistrationNumber => _registrationnumber;

        /// <summary>
        /// Gets the global identifier of the Geraldic Sign.
        /// </summary>
        public decimal GlobalId => _globalid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Geraldic_Signs"/> class.
        /// </summary>
        /// <param name="name">The name of the Geraldic Sign.</param>
        /// <param name="type">The type of the Geraldic Sign.</param>
        /// <param name="picture">The picture of the Geraldic Sign.</param>
        /// <param name="description">The description of the Geraldic Sign.</param>
        /// <param name="semantics">The semantics of the Geraldic Sign.</param>
        /// <param name="certificateholdername">The name of the certificate holder of the Geraldic Sign.</param>
        /// <param name="registrationdate">The registration date of the Geraldic Sign.</param>
        /// <param name="registrationnumber">The registration number of the Geraldic Sign.</param>
        /// <param name="globalid">The global identifier of the Geraldic Sign.</param>
        [JsonConstructor]
        public Geraldic_Signs(string name, string type, string picture, string description, string semantics, string certificateholdername, string registrationdate, string registrationnumber, decimal globalid)
        {
            if (name is not null)
            {
                _name = name;
            }
            if (type is not null)
            {
                _type = type;
            }
            if (picture is not null)
            {
                _picture = picture;
            }
            if (description is not null)
            {
                _description = description;
            }
            if (semantics is not null)
            {
                _semantics = semantics;
            }
            if (certificateholdername is not null)
            {
                _certificateholdername = certificateholdername;
            }
            if (registrationdate is not null)
            {
                _registrationdate = registrationdate;
            }
            if (registrationnumber is not null)
            {
                _registrationnumber = registrationnumber;
            }
            if (globalid > 0)
            {
                _globalid = globalid;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geraldic_Signs"/> class with default values.
        /// </summary>
        public Geraldic_Signs() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0) { }

        /// <summary>
        /// Returns a string representation of the Geraldic Sign.
        /// </summary>
        /// <returns>A string representing the Geraldic Sign in CSV format.</returns>
        public override string ToString()
        {
            return $"\"{_name}\";\"{_type}\";\"{_picture}\";\"{_description}\";\"{_semantics}\";\"{_certificateholdername}\";\"{_registrationdate}\";\"{_registrationnumber}\";\"{_globalid.ToString(CultureInfo.InvariantCulture)}\"";
        }

        /// <summary>
        /// Gets the registration date of the Geraldic Sign only.
        /// </summary>
        /// <returns>The registration date of the Geraldic Sign.</returns>
        public string GetRegistrationDateOnly()
        {
            return $"{_registrationdate}";
        }

    }
}
