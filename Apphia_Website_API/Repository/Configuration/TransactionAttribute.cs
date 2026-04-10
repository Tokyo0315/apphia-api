using Microsoft.AspNetCore.Mvc.Filters;

namespace Apphia_Website_API.Repository.Configuration
{
    public class TransactionAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<DatabaseContext>();

            if (dbContext == null)
            {
                await next();
                return;
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
                throw new Exception("Transaction rolled back due to an error during the process.");
            }
        }
    }
}
