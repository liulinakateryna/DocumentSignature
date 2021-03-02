namespace Database.KeyCenter.Model
{
    public class SignatureModel
    {
        public string UserId { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Sign { get; set; }
    }
}
