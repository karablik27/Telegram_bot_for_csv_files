using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Var8.TG_Bot_Main
{
    /// <summary>
    /// Represents a class to manage file types associated with user IDs.
    /// </summary>
    public class File_Main
    {
        /// <summary>
        /// Enumerates the possible file types.
        /// </summary>
        public enum FileType { CSV, JSON }

        private readonly Dictionary<long, FileType> _types = new Dictionary<long, FileType>();

        /// <summary>
        /// Gets the file type associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The file type associated with the user ID.</returns>
        public FileType GetType(long id) => _types.TryGetValue(id, out var type) ? type : FileType.CSV;

        /// <summary>
        /// Sets the file type associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="type">The file type to associate with the user ID.</param>
        public void SetType(long id, FileType type) => _types[id] = type;
    }
}
