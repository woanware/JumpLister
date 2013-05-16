using System.IO;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class Errors
    {
        #region Member Variables
        private CsvHelper.CsvWriter _csvWriter { get; set; }
        private MemoryStream _memoryStream { get; set; }
        private StreamWriter _streamWriter { get; set; }
        public bool HasErrors { get; private set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Errors()
        {
            _memoryStream = new MemoryStream();
            _streamWriter = new StreamWriter(_memoryStream);
            _csvWriter = new CsvHelper.CsvWriter(_streamWriter);

            _csvWriter.WriteField("File");
            _csvWriter.WriteField("Stream No.");
            _csvWriter.WriteField("NetBIOS Name");
            _csvWriter.WriteField("Date/Time");
            _csvWriter.WriteField("Data");
            _csvWriter.WriteField("Error");
            _csvWriter.NextRecord();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="destListEntry"></param>
        public void AddError(string file,
                             DestListEntry destListEntry)
        {
            HasErrors = true;

            _csvWriter.WriteField(file);
            _csvWriter.WriteField(destListEntry.StreamNo);
            _csvWriter.WriteField(destListEntry.NetBiosName);
            _csvWriter.WriteField(destListEntry.FileTime);
            _csvWriter.WriteField(destListEntry.Data);
            _csvWriter.WriteField(string.Empty);
            _csvWriter.NextRecord();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="error"></param>
        public void AddError(string file,
                             string error)
        {
            HasErrors = true;

            _csvWriter.WriteField(file);
            _csvWriter.WriteField(string.Empty);
            _csvWriter.WriteField(string.Empty);
            _csvWriter.WriteField(string.Empty);
            _csvWriter.WriteField(string.Empty);
            _csvWriter.WriteField(error);
            _csvWriter.NextRecord();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Csv
        {
            get
            {
                string csv = string.Empty;
                if (_memoryStream != null)
                {
                    _memoryStream.Position = 0;
                    using (StreamReader streamReader = new StreamReader(_memoryStream))
                    {
                        csv = streamReader.ReadToEnd();
                    }

                    _memoryStream.Dispose();
                    _memoryStream = null;
                    _streamWriter.Dispose();
                    _streamWriter = null;
                    _csvWriter.Dispose();
                    _csvWriter = null;
                }

                return csv;
            }
        }
    }
}
