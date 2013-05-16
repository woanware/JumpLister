using System.Collections.Generic;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class JumpList
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<NameValue> Data { get; set; }
        public DestListEntry DestListEntry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JumpList()
        {
            Data = new List<NameValue>();
            DestListEntry = new DestListEntry();
        }
    }
}
