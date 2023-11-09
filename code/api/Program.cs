using data;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure our own services and dependencies.
builder.Services.AddMemoryCache();
// Singleton because we don't want multiple instances of our cached data.
builder.Services.AddSingleton<IDataProvider, CachedDataProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Didn't want to move the files around in this repo.
// By using a custom PhysicalFileProvider I can point the code to look into the other directory.
// Obviously not for production scenario's.
var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), $@"..{Path.DirectorySeparatorChar}", "web", "dist"));
app.UseDefaultFiles(new DefaultFilesOptions() { FileProvider = fileProvider });
app.UseStaticFiles(new StaticFileOptions() { FileProvider = fileProvider });

app.UseAuthorization();

app.MapControllers();

app.Run();
