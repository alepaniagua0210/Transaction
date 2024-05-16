
using Ejemplo01.Data;
using Ejemplo01.Models;
using Microsoft.EntityFrameworkCore;


const string X_ORIGIN_ACCOUNT = "10";
const string X_DESTINATION_ACCOUNT = "20";

using (var dbContext = new BankContext())
{
    using (var transaction = dbContext.Database.BeginTransaction()) 
    {
        try
        {
            decimal quantityToTransfer = 200M;

            var originAccount = dbContext.Transactions
                .Where(a => a.AccountNumber == X_ORIGIN_ACCOUNT)
                .GroupBy(a => a.AccountNumber)
                .Select(a => new { Balance = a.Sum(a => a.Credit) - a.Sum(a => a.Debit) })
                .FirstOrDefault();

            if (originAccount == null || originAccount.Balance < quantityToTransfer)
            {
                throw new Exception($"Hey this error is very Bad {X_ORIGIN_ACCOUNT}");
            }

            var CreditTransaction = new AccountTransaction
            {
                AccountNumber = X_ORIGIN_ACCOUNT,
                Credit = quantityToTransfer,
                Debit = 0M
            };

            dbContext.SaveChanges();

            transaction.Commit();
            Console.WriteLine("Los fondos fueron transferidos exitosamente");
        }
        catch (Exception)
        {

            Console.WriteLine("Error al transferir doooooooog"); 
        }
    }
}
