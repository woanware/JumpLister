using Shellify;
using System;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class DestListEntry
    {
        public Guid[] Droid { get; set; }
        public Guid[] DroidBirth { get; set; }
        public Uuid Uuid { get; set; }
        public Uuid UuidBirth { get; set; }
        public string NetBiosName { get; set; }
        public string StreamNo { get; set; }
        public DateTime FileTime { get; set; }
        public string Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DestListEntry()
        {
            Droid = new Guid[2];
            DroidBirth = new Guid[2];
            NetBiosName = string.Empty;
            StreamNo = string.Empty;
            Data = string.Empty;
        }
    }
}
