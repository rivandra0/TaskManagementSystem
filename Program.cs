using TaskManagementSystem.Repositories;
using TaskManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

#region Registering Services
//registering service with real repository
builder.Services.AddSingleton<TaskService>(_ =>
{
    //the repo object is interchangable
    //return new TaskService(new LocalTaskRepository());
    return new TaskService(new TaskRepository(Path.Combine(AppContext.BaseDirectory, "Database", "db.json")));
});

//...add more services here later

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
