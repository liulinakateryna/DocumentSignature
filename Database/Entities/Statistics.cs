using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Statistics
    {
        public List<int> Points { get; set; }
        public List<int> Values { get; set; }

        public Statistics()
        {
            Points = new List<int>();
            Values = new List<int>();
        }
    }
}
