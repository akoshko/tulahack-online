using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tulahack.API.Context;
using Tulahack.API.Extensions;
using Tulahack.API.Static;
using Tulahack.API.Utils;
using Tulahack.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ITulahackContext, TulahackContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", "database"));
    var datasource = Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", "database", "tulahack.db");
    options.UseSqlite($"Data Source={datasource}");
});

builder.Services
    .AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddCustomSwagger();

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services
    .AddAuthorization(o =>
        {
            o.AddPolicy("Public", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.Public); });
            o.AddPolicy("Public+", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.PublicPlus); });

            o.AddPolicy("Contestant", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.Contestant); });
            o.AddPolicy("Contestant+", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.ContestantPlus); });

            o.AddPolicy("Expert", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.Expert); });
            o.AddPolicy("Expert+", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.ExpertPlus); });

            o.AddPolicy("Moderator", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.Moderator); });
            o.AddPolicy("Moderator+", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.ModeratorPlus); });

            o.AddPolicy("Superuser", b => { b.RequireClaim(TulahackClaimTypes.KeycloakGroup, Groups.Superuser); });
        }
    ).AddKeycloakAuthorization(builder.Configuration);

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TulahackProfile>());

builder.Services.AddServices();

var app = builder.Build();

app.UseSwagger(c => { c.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Tulahack API V2");
    c.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage")),
    RequestPath = "/api/storage",
    // Uncomment to enable static files authorization
    // OnPrepareResponse = ctx =>
    // {
    //     if (ctx.Context.User.Identity is { IsAuthenticated: true }) return;
    //
    //     // respond HTTP 401 Unauthorized.
    //     ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //     // flush response
    //     ctx.Context.Response.ContentLength = 0;
    //     ctx.Context.Response.Body = Stream.Null;
    //     // disable cache
    //     ctx.Context.Response.Headers.Append("Cache-Control", "no-store");
    // }
});

// Initial migration and DB seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ITulahackContext>() as TulahackContext;
    if (context != null)
    {
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();

        var skillsCount = context.Skills.Count();
        if (skillsCount == 0)
        {
            JsonSerializer serializer = new JsonSerializer();
            using var stream = new StreamReader("Static/skills.json");
            await using JsonReader reader = new JsonTextReader(stream);
            var skills = serializer.Deserialize<List<string>>(reader);
            foreach (var skill in skills)
            {
                context.Skills.Add(new Skill()
                {
                    Id = Guid.NewGuid(),
                    SkillName = skill
                });
            }

            await context.SaveChangesAsync();
        }
    }
}

app.Run();