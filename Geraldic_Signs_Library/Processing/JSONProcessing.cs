using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Geraldic_Signs_Library.Processing
{
    /// <summary>
    /// Provides methods for processing JSON data containing Geraldic Signs.
    /// </summary>
    public class JSONProcessing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONProcessing"/> class.
        /// </summary>
        public JSONProcessing() { }

        /// <summary>
        /// Writes Geraldic Signs data to a JSON stream.
        /// </summary>
        /// <param name="signs">The list of Geraldic Signs to write.</param>
        /// <returns>A stream containing the JSON data.</returns>
        public Stream Write(List<Geraldic_Signs> signs)
        {
            try
            {
                // Serialize the list of Geraldic_Signs objects to a JSON string
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(signs, options);

                // Create a MemoryStream and write the serialized JSON bytes to it
                var stream = new MemoryStream();
                stream.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

                // Reset the stream position to the beginning, so it can be read from the start
                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while forming the write stream", ex);
            }
        }

        /// <summary>
        /// Reads Geraldic Signs data from a JSON stream.
        /// </summary>
        /// <param name="stream">The stream containing the JSON data.</param>
        /// <returns>A list of Geraldic Signs read from the JSON stream.</returns>
        public List<Geraldic_Signs> Read(Stream stream)
        {
            try
            {
                // Ensure the stream is positioned at the beginning
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                // Deserialize the JSON stream to a List<Geraldic_Signs>
                List<Geraldic_Signs> geraldicSignsList = JsonSerializer.DeserializeAsync<List<Geraldic_Signs>>(stream).Result;
                return geraldicSignsList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deserializing data from the stream", ex);
            }
        }
    }
}
