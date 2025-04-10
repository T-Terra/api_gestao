using Expenses.Config;
using Expenses.Routes;
using Expenses.Data;
using Expenses.Middlewares;
using Expenses.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ExpenseContext>();
builder.Services.AddScoped<Configuration>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173").WithMethods(AllowedHttpMethods.Methods).AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ApiPolicy");
app.UseMethodsMiddleware();

app.ApiRoutes();

// app.UseHttpsRedirection();
app.Run();
