using Yarışma.Models;

namespace Yarışma.Middleware
{
    public class ContestantAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CompetitionDbContext _dbContext;

        // Doğru constructor
        public ContestantAccessMiddleware(RequestDelegate next, CompetitionDbContext dbContext)
        {
            _next = next;
            _dbContext = dbContext;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentPeriod = _dbContext.Periods.FirstOrDefault();

            if (currentPeriod != null && currentPeriod.ContestantEndDate.HasValue)
            {
                if (DateTime.Now > currentPeriod.ContestantEndDate.Value)
                {
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }
            }

            await _next(context);
        }
    }

}
