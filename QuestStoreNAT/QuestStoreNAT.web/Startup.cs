using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.DatabaseLayer.ConcreteDAO;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;

namespace QuestStoreNAT.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectDB.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILoginValidatorService, LoginValidatorService>();
            services.AddScoped<IUserFinderService, UserFinderService>();
            services.AddSingleton<ICurrentSession, CurrentSession>();
          
            services.AddScoped<MentorDAO>();
            services.AddScoped<StudentDAO>();
            services.AddScoped<CredentialsDAO>();
            services.AddScoped<ClassEnrolmentDAO>();
            services.AddScoped<ClassroomDAO>();
            services.AddScoped<GroupDAO>();
            services.AddScoped<OwnedQuestStudentDAO>();
            services.AddScoped<QuestDAO>();
            services.AddScoped<IDB_GenericInterface<Credentials>, CredentialsDAO>();
            services.AddScoped<IDB_GenericInterface<Quest>, QuestDAO>();
            services.AddScoped<IDB_GenericInterface<Group>, GroupDAO>();
            services.AddScoped<IDB_GenericInterface<OwnedQuestStudent>, OwnedQuestStudentDAO>();
            services.AddScoped<IDB_GenericInterface<OwnedQuestGroup>, OwnedQuestGroupDAO>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
