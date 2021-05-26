using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.ValueObjects
{
    public class Attachment
    {
        /// <summary>
        /// Unique identifier for the uploaded file 
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Name of the file along with extension
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Fully qualified path name (path and name)
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Size of the file
        /// </summary>
        public string FileSize { get; set; }
    }
}
