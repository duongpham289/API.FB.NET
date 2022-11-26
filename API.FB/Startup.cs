using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using API.FB.Core.Services;
using API.FB.Infrastructure.Repository;
<<<<<<< Updated upstream
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

=======
using API.FB.Core.Interfaces.Repository;
>>>>>>> Stashed changes
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


<<<<<<< Updated upstream
            //Services DI:
            services.AddScoped<IUserService, UserService>();
=======
            //services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();



            services.AddScoped<IAuthRepo, AuthRepo>();

            services.AddScoped<ICommentRepo, CommentRepo>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepo, PostRepo>();
>>>>>>> Stashed changes

            //Repository DI:
            services.AddScoped<IUserRepository, UserRepository>();

<<<<<<< Updated upstream
            //Base DI:
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
=======
            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));

            services.AddScoped<ICommentRepo, CommentRepo>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepo, PostRepo>();

>>>>>>> Stashed changes
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
