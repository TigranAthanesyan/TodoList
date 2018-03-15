using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoListApp.Controllers;
using TodoListApp.Models;

namespace TodoListApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(connection));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey(HomeController.CookyUserName) &&
                    !HomeController.IsAllowableFunction(context.Request.Path.Value))
                {
                    string url = "http://" + context.Request.Host.Value + "/Home/" + nameof(HomeController.SignIn);
                    context.Response.Redirect(url);
                }
                else
                {
                    await next.Invoke();
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=List}/{id?}");
            });
        }
    }
}
