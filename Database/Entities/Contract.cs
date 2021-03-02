using System;

namespace Database.Entities
{
    public class Contract
    {
        public int ContractId { get; set; }
        public string Sign { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public bool IsRefused { get; set; }

        public string Id { get; set; }
        public User User { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
