using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.FB.Core.Interfaces.Services;
using API.FB.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.FB.Core.Interfaces.Services;
using API.FB.Core.Services;
using API.FB.Infrastructure.Repository;
using API.FB.Core.Interfaces.Repository;
namespace Web07.FinalTest.MF960
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API .NET", Version = "v1" });
            });

            services.AddControllers().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });


            //services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();



            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepo, AuthRepo>();

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentRepo, CommentRepo>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepo, PostRepo>();


            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentRepo, CommentRepo>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepo, PostRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web07.FinalTest.MF960 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
