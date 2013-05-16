using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class JumpListFile
    {
        public string FilePath { get; set; }
        public string FileName { get;set;}
        public string AppName { get; set; }
        public List<JumpList> JumpLists { get; set; }
        public List<DestListEntry> DestListEntries { get; set; }
        public long DestListSize { get; set; }
        public bool IsCustom { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public JumpListFile(bool isCustom)
        {
            IsCustom = isCustom;
            FilePath = string.Empty;
            FileName = string.Empty;
            JumpLists = new List<JumpList>();
            DestListEntries = new List<DestListEntry>();
        }
    }
}
