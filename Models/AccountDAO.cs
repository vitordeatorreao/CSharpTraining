using Microsoft.EntityFrameworkCore;

namespace BankApi.Models
{
    public interface IAccountDAL
    {
        Account Create(Account account);
        Account? ReadById(Guid id);
        Account Update(Account account);
        Account DeleteById(Guid id);
        IEnumerable<Account> List();
    }

    public class AccountDAO : DbContext, IAccountDAL
    {
        public AccountDAO(DbContextOptions<AccountDAO> options)
            : base(options)
        {
        }

        public DbSet<Account> AccountItems { get; set; } = null!;

        public Account Create(Account account)
        {
            var addition = this.AccountItems.Add(account);
            this.SaveChanges();
            return addition.Entity;
        }

        public Account DeleteById(Guid id)
        {
            var account = this.AccountItems.Find(id);

            if (account == null) throw new AccountNotFoundException(id);

            var removal = this.AccountItems.Remove(account);
            this.SaveChanges();
            return removal.Entity;
        }

        public IEnumerable<Account> List()
        {
            return this.AccountItems.ToList();
        }

        public Account? ReadById(Guid id)
        {
            return this.AccountItems.Find(id);
        }

        public Account Update(Account account)
        {
            var dbAccount = this.AccountItems.Find(account.Id);

            if (dbAccount == null) throw new AccountNotFoundException(account.Id);
            
            dbAccount.Name = account.Name;
            dbAccount.OwnerName = account.OwnerName;
            dbAccount.IsOpen = account.IsOpen;
            dbAccount.OpenedAt = account.OpenedAt;
            dbAccount.ClosedAt = account.ClosedAt;
            dbAccount.Balance = account.Balance;

            try
            {
                this.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) when (!AccountExists(account.Id))
            {
                throw new AccountNotFoundException(account.Id);
            }

            return dbAccount;
        }

        private bool AccountExists(Guid id)
        {
            return this.AccountItems.Any(acc => acc.Id == id);
        }
    }
}