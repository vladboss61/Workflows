using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace WebAppEF.AdventureS.Ef;

public static class ApplicationDbContextExtensions
{
    public static async Task ExecuteAsync(this ApplicationDbContext context, Action action)
    {
        IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
        try
        {
            action();
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
