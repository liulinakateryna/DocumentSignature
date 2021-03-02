using System.Security.Cryptography;

namespace Database.KeyCenter.Entity
{
    public class PrivateData
    {
        public int PrivateDataId { get; set; }
        public string UserId { get; set; }
        public byte[] Fingerprint { get; set; }

        public int KeysId { get; set; }
        public Keys RsaParameters { get; set; }

    }
}
