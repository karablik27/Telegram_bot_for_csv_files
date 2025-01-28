using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Geraldic_Signs_Library.Processing
{
    /// <summary>
    /// Provides methods for processing CSV files containing Geraldic Signs data.
    /// </summary>
    public class CSVProcessing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSVProcessing"/> class.
        /// </summary>
        public CSVProcessing() { }

        // Constants for the first and second lines of the CSV file
        const string first = "\"Name\";\"Type\";\"Picture\";\"Description\";\"Semantics\";\"CertificateHolderName\";\"RegistrationDate\";\"RegistrationNumber\";\"global_id\";";
        const string second = "\"Название символа (знака)\";\"Вид символа (знака)\";\"Изображение\";\"Описание\";\"Семантика\";\"Наименование обладателя свидетельства\";\"Дата внесения официального символа (знака) в реестр\";\"Регистрационный номер\";\"global_id\";";

        /// <summary>
        /// Writes Geraldic Signs data to a CSV stream.
        /// </summary>
        /// <param name="signs">The list of Geraldic Signs to write.</param>
        /// <returns>A stream containing the CSV data.</returns>
        public Stream Write(List<Geraldic_Signs> signs)
        {
            Stream stream;

            try
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine(first);
                csvContent.AppendLine(second);

                foreach (var sign in signs)
                {
                    csvContent.AppendLine(sign.ToString());
                }

                stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(true));
                writer.Write(csvContent.ToString());
                writer.Flush();
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while forming the write stream.", ex);
            }

            return stream;
        }

        /// <summary>
        /// Reads Geraldic Signs data from a CSV stream.
        /// </summary>
        /// <param name="stream">The stream containing the CSV data.</param>
        /// <returns>A list of Geraldic Signs read from the CSV stream.</returns>
        public List<Geraldic_Signs> Read(Stream stream)
        {
            var geraldicSignsList = new List<Geraldic_Signs>();
            StringBuilder fileContent = new StringBuilder();

            try
            {
                if (stream == null || !stream.CanRead)
                {
                    throw new ArgumentException("The stream cannot be read.");
                }

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    // Read the entire file into a StringBuilder
                    fileContent.Append(reader.ReadToEnd());
                }

                // Regular expression for finding records, considering quotes around values
                var entriesPattern = new Regex("\"([^\"]*?)\";\"([^\"]*?)\";\"([^\"]*?)\";\"([^\"]*?)\";\"([^\"]*?)\";\"([^\"]*?)\";\"([^\"]*?)\";\"([^\"]*?)\";\"(\\d+)\";");
                var matches = entriesPattern.Matches(fileContent.ToString());

                foreach (Match match in matches)
                {
                    // Skip headers
                    if (match.Groups[1].Value == "Name" && match.Groups[9].Value == "global_id")
                        continue;

                    // Create a Geraldic_Signs object based on data from the regular expression
                    geraldicSignsList.Add(new Geraldic_Signs(
                        match.Groups[1].Value,
                        match.Groups[2].Value,
                        match.Groups[3].Value,
                        match.Groups[4].Value,
                        match.Groups[5].Value,
                        match.Groups[6].Value,
                        match.Groups[7].Value,
                        match.Groups[8].Value,
                        decimal.Parse(match.Groups[9].Value, CultureInfo.InvariantCulture)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                // Add any necessary error handling here
            }

            return geraldicSignsList;
        }
    }
}
