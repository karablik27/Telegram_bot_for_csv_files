using Geraldic_Signs_Library;
using System;
using System.Collections.Generic;
using System.IO;

namespace Var8.TG_Bot_Main
{
    /// <summary>
    /// Represents a class to manage user states and associated data.
    /// </summary>
    public class User_Main : IDisposable
    {
        private readonly Dictionary<long, string> _states = new Dictionary<long, string>();
        private readonly Dictionary<long, Stream> _fFiles = new Dictionary<long, Stream>();
        private readonly Dictionary<long, string> _fields = new Dictionary<long, string>();
        private readonly Dictionary<long, List<Geraldic_Signs>> _libraries = new Dictionary<long, List<Geraldic_Signs>>();

        /// <summary>
        /// Gets or sets a value indicating whether a file has been uploaded by the user.
        /// </summary>
        public bool FileUploaded { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User_Main"/> class.
        /// </summary>
        public User_Main() { }

        /// <summary>
        /// Gets the state associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The state associated with the user ID.</returns>
        public string GetState(long id) => _states.TryGetValue(id, out var state) ? state : "Normal";

        /// <summary>
        /// Sets the state associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="state">The state to associate with the user ID.</param>
        public void SetState(long id, string state) => _states[id] = state;

        /// <summary>
        /// Sets the file associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="file">The file stream to associate with the user ID.</param>
        public void SetFile(long id, Stream file)
        {
            Remove(id);
            _fFiles[id] = file;
        }

        /// <summary>
        /// Gets the file stream associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The file stream associated with the user ID, or null if not found.</returns>
        public Stream GetFile(long id) => _fFiles.TryGetValue(id, out var fule) ? fule : null;

        /// <summary>
        /// Sets the field associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="field">The field value to associate with the user ID.</param>
        public void SetField(long id, string field) => _fields[id] = field;

        /// <summary>
        /// Gets the field value associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The field value associated with the user ID, or null if not found.</returns>
        public string GetField(long id) => _fields.TryGetValue(id, out var field) ? field : null;

        /// <summary>
        /// Sets the list of Geraldic Signs associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="lib">The list of Geraldic Signs to associate with the user ID.</param>
        public void SetGeraldicSigns(long id, List<Geraldic_Signs> lib) => _libraries[id] = lib;

        /// <summary>
        /// Gets the list of Geraldic Signs associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The list of Geraldic Signs associated with the user ID, or null if not found.</returns>
        public List<Geraldic_Signs> GetLibraries(long id) => _libraries.TryGetValue(id, out var list) ? list : null;

        /// <summary>
        /// Removes the file stream associated with the specified user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        public void Remove(long id)
        {
            if (_fFiles.TryGetValue(id, out var fileStream))
            {
                fileStream?.Dispose();
                _fFiles.Remove(id);
            }
        }

        /// <summary>
        /// Disposes resources used by the User_Main instance.
        /// </summary>
        public void Dispose()
        {
            foreach (var file in _fFiles.Values)
            {
                file?.Dispose();
            }
            _fFiles.Clear();
        }
    }
}
