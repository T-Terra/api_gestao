using Expenses.Config;
using Expenses.Routes;
using Expenses.Data;
using Expenses.Middlewares;
using Expenses.Services;
using Expenses.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ExpenseContext>();
builder.Services.AddScoped<Configuration>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // deixe true em produção
        options.SaveToken = true;
        options.TokenValidationParameters = TokenHelpers.GetTokenValidationParameters(builder.Configuration);
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Cookies["token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiPolicy", policy =>
    {
        policy.WithOrigins("https://gestao-gastos.vercel.app", "http://localhost:5173")
            .WithMethods(AllowedHttpMethods.Methods)
            .AllowCredentials()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ExpenseContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
    options.UseNpgsql(connectionString);
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

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpenseContext>();
    db.Database.Migrate();  // Aplica as migrations pendentes
}

app.Run();
