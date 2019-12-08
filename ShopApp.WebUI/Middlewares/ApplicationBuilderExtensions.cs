using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace ShopApp.WebUI.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        //miidlewares olması için ilk paramtre this sonrada IApplicationBuilder genişletcez
        //startup.cs öyle çünkü
        public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app)
        {
            //dışarıya açacağımız modul = node_modules
            var path = Path.Combine(Directory.GetCurrentDirectory(), "node_modules");

            var options = new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/modules" // takma ismi node_modules bununlar erişcez

            };
            app.UseStaticFiles(options);
            return app;
        }
    }
}
