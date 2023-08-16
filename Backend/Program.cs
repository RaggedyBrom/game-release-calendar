// Build and run the web application

var builder = WebApplication.CreateBuilder(args);

#region Builder setup
builder.Services.AddControllers();

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("*");
        });
    });
#endregion

var app = builder.Build();

#region Middleware
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
#endregion

app.Run();
