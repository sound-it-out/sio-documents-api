using SIO.Documents.API.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureDocumentsApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
    await app.RunProjectionMigrationsAsync();

await app.RunAsync();
