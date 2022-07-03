using Microsoft.EntityFrameworkCore;

namespace BankApi.Models
{
    public interface IAccountDALAsync
    {
        Task<Account> Create(Account account);
        Task<Account?> ReadById(Guid id);
        Task<Account> Update(Account account);
        Task<Account> DeleteById(Guid id);
        Task<IEnumerable<Account>> List();
    }

    public class AccountDAOAsync : DbContext, IAccountDALAsync
    {
        public AccountDAOAsync(DbContextOptions<AccountDAOAsync> options)
            : base(options)
        {
        }

        public DbSet<Account> AccountItems { get; set; } = null!;

        public async Task<Account> Create(Account account)
        {
            var addition = await this.AccountItems.AddAsync(account);
            await this.SaveChangesAsync();
            return addition.Entity;
        }

        public async Task<Account> DeleteById(Guid id)
        {
            var account = await this.AccountItems.FindAsync(id);

            if (account == null) throw new AccountNotFoundException(id);

            var removal = this.AccountItems.Remove(account);
            await this.SaveChangesAsync();
            return removal.Entity;
        }

        public async Task<IEnumerable<Account>> List()
        {
            return await this.AccountItems.ToListAsync();
        }

        public async Task<Account?> ReadById(Guid id)
        {
            return await this.AccountItems.FindAsync(id);
        }

        public async Task<Account> Update(Account account)
        {
            var dbAccount = await this.AccountItems.FindAsync(account.Id);

            if (dbAccount == null) throw new AccountNotFoundException(account.Id);
            
            dbAccount.Name = account.Name;
            dbAccount.OwnerName = account.OwnerName;
            dbAccount.OpenedAt = account.OpenedAt;
            dbAccount.CanGoNegative = account.CanGoNegative;
            dbAccount.Balance = account.Balance;

            try
            {
                await this.SaveChangesAsync();
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