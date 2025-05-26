using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddContentHashValidation();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();

app.UseContentHashValidation();
app.MapControllers();

app.Run();