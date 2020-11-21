Imports System.IO
Imports System.Security.Claims
Imports System.Web.Helpers
Imports System.Web.Hosting
Imports System.Web.Http
Imports System.Web.Optimization
Imports AutoMapper
Imports risersoft.shared.bot
Imports risersoft.shared.web
Imports risersoft.shared.web.mvc
Imports Serilog

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        Dim oApp = New clsMvcWebApp(New clsExtendWebAppPrj)
        BotTaskProviderBase.fncAdapter = Function(x, y, z)
                                             Return New AdapterWithErrorHandler(x, y, z)
                                         End Function

        'myFuncs2.InitializeMapper()
        GlobalWeb.myWebApp = oApp
        HostingEnvironment.RegisterVirtualPathProvider(oApp.fncVirutalPathProvider())
        ControllerBuilder.Current.SetControllerFactory(GetType(MyControllerFactory))
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier

        Dim path2 = HostingEnvironment.MapPath("~/App_Data/Logs")
        If (Not Directory.Exists(path2)) Then Directory.CreateDirectory(path2)
        Serilog.Log.Logger = New LoggerConfiguration().WriteTo.File(System.IO.Path.Combine(path2, "log-{Date}.txt"), rollingInterval:=RollingInterval.Day, rollOnFileSizeLimit:=True).CreateLogger()

        Dim razorEngine = ViewEngines.Engines.OfType(Of RazorViewEngine)().FirstOrDefault()
        razorEngine.ViewLocationFormats = razorEngine.ViewLocationFormats.Concat(New String() {"~/Views/Forms/{0}.cshtml", "~/Views/Forms/{0}.vbhtml"}).ToArray()
        razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats.Concat(New String() {"~/Views/Forms/{0}.cshtml", "~/Views/Forms/{0}.vbhtml"}).ToArray()

    End Sub
End Class
