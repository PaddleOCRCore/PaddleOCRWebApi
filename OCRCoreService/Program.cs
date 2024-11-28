using OCRCoreService.Authorization;
using OCRCoreService;
using OCRCoreService.Services;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using NLog.Web;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseNLog();
    // Add services to the container.
    //builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    //检测模型依赖注入
    builder.Services.AddSingleton<IOCREngine, OCREngine>();

    // 网页显示中文
    builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    //使用本地缓存必须添加
    builder.Services.AddMemoryCache();
    //添加Api全局过滤
    builder.Services.AddControllersWithViews(options =>
    {
        //options.Filters.Add<WebApiActionAttribute>();//改为在接口中单独引用，上传文件接口无法使用此方法
        options.Filters.Add<ApiErrorHandleAttribute>();
    });
    builder.Services.AddSwagger();


    var app = builder.Build();

    var fordwardedHeaderOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    };
    fordwardedHeaderOptions.KnownNetworks.Clear();
    fordwardedHeaderOptions.KnownProxies.Clear();
    app.UseForwardedHeaders(fordwardedHeaderOptions);

    if (builder.Configuration.GetValue("UseHttps", true)) app.UseHttpsRedirection();
    var pathBase = builder.Configuration["SwaggerPathBase"]?.TrimEnd('/') ?? "";
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseSwaggerApp(pathBase);
    }
    else
    {
        app.UseDeveloperExceptionPage();
        app.UseSwaggerApp(pathBase);
    }
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultControllerRoute();
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
