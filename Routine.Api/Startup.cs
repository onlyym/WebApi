using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Routine.Api.Data;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //注册服务
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup => {
                setup.ReturnHttpNotAcceptable = true;
                //在注册Controller的时候配置默认返回格式  3.0之前这样写
                // setup.OutputFormatters.Insert(0,new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();
            //3.0之后只需要在AddControllers之后  使用AddXmlDataContractSerializerFormatters方法，输入输出都会添加xml支持

            //使用 AutoMapper，扫描当前应用域的所有 Assemblies 寻找 AutoMapper 的配置文件（视频P12）
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            //注册自己写的服务 使用Scoped每次HTTP请求都会注册
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            //注册DbContext
            services.AddDbContext<RoutesDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });
        }


        //配置请求管道 -- 添加中间件
        //每一句话都相当于添加了一个中间件
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //添加中间件的顺序非常重要，如果你把授权中间件放在了Controller的后边，
            //那么即使需要授权，那么请求也会先到达Controller并执行里面的代码，这样的话授权就没有意义了。（视频P1）
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                //500 错误信息
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error!");
                    });
                });
            }

            //使用路由
            app.UseRouting();

            //添加授权中间件
            app.UseAuthorization();

            //使用端点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
