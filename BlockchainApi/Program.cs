using BlockchainSharp;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(new[] { $"http://localhost:{args[0]}/" });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Node>();
builder.Services.AddHostedService<Node>(provider => provider.GetService<Node>());

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
