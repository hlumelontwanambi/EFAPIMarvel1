
using EFAPIMarvel1.Model;

namespace EFAPIMarvel1
{
    public class Program
    {
        static DbApiContext db = new DbApiContext();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            //GET endPOINTS
            app.MapGet("/users", () =>
            {
                DbApiContext db = new DbApiContext();
                return db.TblAvengers.ToList();
                db.Dispose();
            })
            .WithName("/GetUsers")
            .WithOpenApi();

            app.MapGet("/contacts", () =>
            {
                DbApiContext db = new DbApiContext();
                return db.TblAvengers.ToList();
                db.Dispose();
            })
            .WithName("/GetContacts")
            .WithOpenApi();

            //POST endPoints
            app.MapPost("/users", (TblAvenger newUser) =>
            {
                db.TblAvengers.Add(newUser);
                db.SaveChanges();
                return Results.Created($"/users/{newUser.Username}", newUser);
            }).WithName("CreateUser").WithOpenApi();

            app.MapPost("/contacts", (TblContact newContact) =>
            {
                db.TblContacts.Add(newContact);
                db.SaveChanges();
                return Results.Created($"/users/{newContact.Username}", newContact);
            }).WithName("CreateContract").WithOpenApi();


            //PUT endpoints
            app.MapPut("/users/{username}", (string username, TblAvenger updatedUser) =>
            {
                var user = db.TblAvengers.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    user.Password = updatedUser.Password;
                    db.SaveChanges();
                    return Results.NoContent();
                }
                return Results.NotFound();

            }).WithName("UpdateUser").WithOpenApi();

            app.MapPut("/contacts/{id}", (int id, TblContact updatedContact) =>
            {
                var contact = db.TblContacts.FirstOrDefault(c => c.AvengerId == id);
                if (contact != null)
                {
                    contact.HeroName = updatedContact.HeroName;
                    contact.RealName = updatedContact.RealName;
                    db.SaveChanges();
                    return Results.NoContent();
                }
                return Results.NotFound();

            }).WithName("UpdateContact").WithOpenApi();

            //DELETE endpoints
            app.MapDelete("/users/{username}", (string username) =>
            {
                var user = db.TblAvengers.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    db.TblAvengers.Remove(user);
                    db.SaveChanges();
                    return Results.NoContent();
                }
                return Results.NotFound();

            }).WithName("DeleteUser").WithOpenApi();

            app.MapDelete("/contacts/{id}", (int id) =>
            {
                var contact = db.TblContacts.FirstOrDefault(c => c.AvengerId == id);
                if (contact != null)
                {
                    db.TblContacts.Remove(contact);
                    db.SaveChanges();
                    return Results.NoContent();
                }
                return Results.NotFound();

            }).WithName("DeleteContact").WithOpenApi();

            app.Run();
        }
    }
}