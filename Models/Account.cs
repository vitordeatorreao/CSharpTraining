namespace BankApi.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? OwnerName { get; set; }

        public DateTime OpenedAt { get; set; }

        public bool CanGoNegative { get; set; }

        public decimal Balance { get; set; }
    }
}