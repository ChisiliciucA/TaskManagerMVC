using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.Data;
using TaskManagerMVC.Services; 

var builder = WebApplication.CreateBuilder(args);

//  MVC + API
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

//  Swagger simplu
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Configurare DbContext (MySQL cu Pomelo)
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")!)
    )
);

// Adăugăm Business Layer prin Dependency Injection
builder.Services.AddScoped<ITasksService, TasksService>();

var app = builder.Build();

//  Middleware pentru erori și Swagger
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//  Mapăm controllerele API
app.MapControllers();

//  Rutele MVC clasice
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
