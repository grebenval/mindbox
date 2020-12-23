using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mindbox.Bl;
using Mindbox.Bl.Bl;
using Mindbox.Database.Sqlite;
using Mindbox.Database.Sqlite.Data;

namespace MindboxApi
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Конфигурация
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавляем интерфейс соединения с БД
            AddDatabaseConnect(services);

            // Добавляем интерфейс взаимодействия с БД хранящих фигуры
            services.AddTransient<IFigureManipulation, DataSqlite>();
            
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                // XML основной сборки
                var xmlFileMain = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPathMain = Path.Combine(AppContext.BaseDirectory, xmlFileMain);
                c.IncludeXmlComments(xmlPathMain);
                // XML сборки Mindbox.Bl
                var xmlFileBl = $"{typeof(IDatabaseConnect).Assembly.GetName().Name}.xml";
                var xmlPathBl = Path.Combine(AppContext.BaseDirectory, xmlFileBl);
                c.IncludeXmlComments(xmlPathBl);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mindbox Test API V1");
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Добавляем интерфейс соединения с БД
        /// </summary>
        /// <param name="services"></param>
        public void AddDatabaseConnect(IServiceCollection services)
        {
            //string connectionString = this.Configuration.GetConnectionString("DefaultConnection");
            string sqliteFile = "mindbox.sqlite";
            string sqliteFileFull = Path.Combine(AppContext.BaseDirectory, sqliteFile);
            string connectionString = $"Data Source={sqliteFileFull}";
            services.AddSingleton<IDatabaseConnect>(new DatabaseConnect(connectionString));

            services.AddDbContext<MindboxContext>();
        }
        
        
    }
}
