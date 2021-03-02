using Database.Entities;
using System;

namespace Database.Models
{
    public class ContractModel
    {
        public int ContractId { get; set; }
        public string UserId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public string Sign { get; set; }
        public bool IsRefused { get; set; }
    }
}
