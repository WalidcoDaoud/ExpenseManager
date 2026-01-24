var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Adiciona suporte a Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger para documentação

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Interface visual do Swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // Mapeia os controllers

app.Run();