using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Document
    {
        public int DocumentId { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public List<Contract> Contracts { get; set; }

        public Document()
        {
            this.Contracts = new List<Contract>();
        }
    }
}
