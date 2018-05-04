using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Models;

namespace User.Api.Data
{
    public class UserContextSeed
    {
        public UserContextSeed() { }

        public static async Task SeedAsync(IApplicationBuilder app, ILoggerFactory loggerFactory, int? retry = 0)
        {
            var retryForAvaiability = retry.Value;

            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var context = (UserContext)scope.ServiceProvider.GetService(typeof(UserContext));

                    var logger = (ILogger<UserContextSeed>)scope.ServiceProvider.GetService(typeof(ILogger<UserContextSeed>));

                    logger.LogDebug("begin usercontext seed");

                    context.Database.Migrate();

                    if (!context.AppUser.Any())
                    {
                        context.AppUser.Add(new AppUser() { Name = "yijia" });
                        context.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    var logger = loggerFactory.CreateLogger(typeof(UserContextSeed));
                    logger.LogError(ex.Message);

                    await SeedAsync(app, loggerFactory, retryForAvaiability);
                }
            }

        }
    }
}
