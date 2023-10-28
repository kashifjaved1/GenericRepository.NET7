using GenericRepository.Demo;
using MarkdownDocumenting.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.ProjectSettings(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableDeepLinking();
        options.ShowExtensions();
        options.DisplayRequestDuration();
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // this'll make the swagger docs appear with all sections collapsed.
        options.RoutePrefix = "api-docs";
        options.SwaggerEndpoint("/swagger/GenericRepository/swagger.json", "GenericProfilerSwagger");
    });

    //this'll get and apply documentation options setted in appsettings DocumentationOptions section.
    app.UseDocumentation(opts =>
        builder.Configuration.Bind("DocumentationOptions", opts)
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
