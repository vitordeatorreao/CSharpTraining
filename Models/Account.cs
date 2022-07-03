namespace BankApi.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? OwnerName { get; set; }

        public bool IsOpen { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime ClosedAt { get; set; }

        public decimal Balance { get; set; }
    }
}