using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamDatabaseImplement.Implements;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IceCreamShopBusinessLogic.HelperModels;

namespace IceCreamRestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddTransient<IClientStorage, ClientStorage>();
            services.AddTransient<IMessageInfoStorage, MessageInfoStorage>();
            services.AddTransient<IOrderStorage, OrderStorage>();
            services.AddTransient<IIceCreamStorage, IceCreamStorage>();
            services.AddTransient<IWareHouseStorage, WareHouseStorage>();
            services.AddTransient<IIngredientStorage, IngredientStorage>();
            services.AddTransient<MailLogic>();
            services.AddTransient<OrderLogic>();
            services.AddTransient<ClientLogic>();
            services.AddTransient<IceCreamLogic>();
            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = "smtp.gmail.com",
                SmtpClientPort = 587,
                MailLogin = "yudenichevaforlab@gmail.com",
                MailPassword = "passwd2001",
            });
            services.AddTransient<WareHouseLogic>();
            services.AddTransient<IngredientLogic>();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
