using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.KeyCenter.Entity
{
    public class Keys
    {
        public int KeysId { get; set; }
        public byte[] D { get; set; }
        public byte[] DP { get; set; }
        public byte[] DQ { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] InverseQ { get; set; }
        public byte[] Modulus { get; set; }
        public byte[] P { get; set; }
        public byte[] Q { get; set; }

        public PrivateData PrivateData { get; set; }
    }
}
