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

        //ע�����
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup => {
                setup.ReturnHttpNotAcceptable = true;
                //��ע��Controller��ʱ������Ĭ�Ϸ��ظ�ʽ  3.0֮ǰ����д
                // setup.OutputFormatters.Insert(0,new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();
            //3.0֮��ֻ��Ҫ��AddControllers֮��  ʹ��AddXmlDataContractSerializerFormatters��������������������xml֧��

            //ʹ�� AutoMapper��ɨ�赱ǰӦ��������� Assemblies Ѱ�� AutoMapper �������ļ�����ƵP12��
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            //ע���Լ�д�ķ��� ʹ��Scopedÿ��HTTP���󶼻�ע��
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            //ע��DbContext
            services.AddDbContext<RoutesDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });
        }


        //��������ܵ� -- ����м��
        //ÿһ�仰���൱�������һ���м��
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����м����˳��ǳ���Ҫ����������Ȩ�м��������Controller�ĺ�ߣ�
            //��ô��ʹ��Ҫ��Ȩ����ô����Ҳ���ȵ���Controller��ִ������Ĵ��룬�����Ļ���Ȩ��û�������ˡ�����ƵP1��
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                //500 ������Ϣ
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error!");
                    });
                });
            }

            //ʹ��·��
            app.UseRouting();

            //�����Ȩ�м��
            app.UseAuthorization();

            //ʹ�ö˵�
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
