// Build and run the web application

var builder = WebApplication.CreateBuilder(args);

#region Builder setup
builder.Services.AddControllers();
#endregion

var app = builder.Build();

#region Middleware
app.UseHttpsRedirection();
app.MapControllers();
#endregion

app.Run();
