﻿using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class Global : HttpApplication
    {
        private TracerProvider _tracerProvider;
        void Application_Start(object sender, EventArgs e)
        {
            _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetInstrumentation()
            // Other configuration, like adding an exporter and setting resources
            .AddConsoleExporter()
            // config
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("fluke-dotnet-1"))
            .AddOtlpExporter(options =>
            {
                options.Endpoint =
                new Uri("http://10.10.70.112:4330/v1/traces");
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
            })
            .Build();
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_End()
        {
            _tracerProvider?.Dispose();
        }
    }

    // public class WebApiApplication : HttpApplication
    // {
    //     private TracerProvider tracerProvider;

    //     void Application_Start(object sender, EventArgs e)
    //     {
    //         this.tracerProvider = Sdk.CreateTracerProviderBuilder()
    //             .AddAspNetInstrumentation()
    //             .AddConsoleExporter()
    //             .SetResourceBuilder(
    //                 ResourceBuilder.CreateDefault()
    //                     .AddService("my-service-name"))
    //             .AddOtlpExporter(options =>
    //             {
    //                 options.Endpoint =
    //                 new Uri("http://10.10.70.112:4330/v1/traces");
    //             })
    //             .Build();
    //     }

    //     void Application_End()
    //     {
    //         this.tracerProvider?.Dispose();
    //     }
    // }
}
