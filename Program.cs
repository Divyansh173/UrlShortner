using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using URLShortner;
using URLShortner.Extentions;
using URLShortner.Services;

var builder = WebApplication.CreateBuilder(args);   

builder.Services.AddDbContext<ApplicationDBContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();  
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUrlShortnerService, UrlShortnerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapPost("api/shorten/{fullUrl}", async (
    string fullUrl, 
    [FromServices]IUrlShortnerService service,
    HttpContext context
    ) =>
{
    var decodedUrl = Uri.UnescapeDataString(fullUrl).Trim('"');

    if (!Uri.TryCreate(decodedUrl, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL");
    }
    var shortendUrl = await service.GetShortendUrl(fullUrl);

    return Results.Ok(shortendUrl);

});

app.MapGet("api/getfullurl/{shortendUrl}", async (
    string shortendUrl,
    [FromServices]IUrlShortnerService service,
    HttpContext context
    )   
     =>
{
    var url = await service.GetFullUrl(shortendUrl);
    var decodedUrl = Uri.UnescapeDataString(url).Trim('"');
    return Results.Redirect(decodedUrl);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
