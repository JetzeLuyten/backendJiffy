using JiffyBackend.DAL;
using JiffyBackend.DAL.Entity;
using System;
using System.Linq;

namespace JiffyBackend.DAL.Initializer
{
    public class DBInitializer
    {
        public static void Initialize(JiffyDbContext context)
        {
            context.Database.EnsureCreated();

            // Check if there are any existing records in the Service table
            if (context.Services.Any())
            {
                return; // DB has been seeded
            }

            // Seed the ServiceTypes table with some dummy data
            if (!context.ServiceTypes.Any())
            {
                var serviceTypes = new ServiceType[]
                {
                    new ServiceType { Name = "Consulting" },
                    new ServiceType { Name = "Lawnmowing" },
                    new ServiceType { Name = "Babysitting" },
                    new ServiceType { Name = "Carwashing" },
                    new ServiceType { Name = "Assistance" }
                };

                context.ServiceTypes.AddRange(serviceTypes);
                context.SaveChanges();
            }

            // Add Services
            if (!context.Users.Any())
            {
                var users = new User[]
                {
                    new User { Auth0UserId = "auth0|66a5163b7686a649a4dbc971", Email = "Emailski", FullName = "Jetze L" },
                    new User { Auth0UserId = "google-oauth2|108034703800733846612", Email = "luytenjetze@gmail.com", FullName = "Jetze luyten" }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
            
            if (!context.Services.Any())
            {
                var services = new List<Service>
                {
                    new Service
                    {
                        Title = "IT-consultant wanted!",
                        ServiceTypeId = context.ServiceTypes.FirstOrDefault(st => st.Name == "Consulting")?.Id ?? 1,
                        Description = "Need an IT-consultant to help me realize some ideas!",
                        UserId = context.Users.FirstOrDefault(u => u.FullName == "Jetze L")?.Id ?? 3,
                        PublishDate = DateTime.UtcNow,
                        Price = 10
                    },
                    new Service
                    {
                        Title = "Car Washing and Detailing",
                        ServiceTypeId = context.ServiceTypes.FirstOrDefault(ot => ot.Name == "Carwashing")?.Id ?? 4,
                        Description = "Our thorough car washing and detailing service will make your vehicle shine inside and out. Includes exterior wash, interior vacuuming, and a full wax treatment.",
                        UserId = context.Users.FirstOrDefault(u => u.FullName == "Jetze luyten")?.Id ?? 2,
                        PublishDate = DateTime.UtcNow,
                        Price = 20
                    },
                    new Service
                    {
                        Title = "Pet Sitting and Dog Walking",
                        ServiceTypeId = context.ServiceTypes.FirstOrDefault(ot => ot.Name == "Babysitting")?.Id ?? 3,
                        Description = "Need someone to look after your pets while you're away? Our pet sitting service includes daily walks, feeding, and lots of playtime. Great for busy pet owners!",
                        UserId = context.Users.FirstOrDefault(u => u.FullName == "Jetze luyten")?.Id ?? 2,
                        PublishDate = DateTime.UtcNow,
                        Price = 30
                    },
                    new Service
                    {
                        Title = "Grocery Shopping Assistance",
                        ServiceTypeId = context.ServiceTypes.FirstOrDefault(ot => ot.Name == "Assistance")?.Id ?? 5,
                        Description = "If you're unable to go grocery shopping yourself, our service can help. We’ll take your shopping list and deliver the groceries to your door.",
                        UserId = context.Users.FirstOrDefault(u => u.FullName == "Jetze L")?.Id ?? 2,
                        PublishDate = DateTime.UtcNow,
                        Price = 25
                    }
                };

                context.Services.AddRange(services);
                context.SaveChanges();

                if (!context.Bookings.Any())
                {
                    var adminUserId = context.Users.FirstOrDefault(u => u.FullName == "The Admin")?.Id;
                    var serviceId = context.Services.FirstOrDefault(o => o.Title == "Service 2")?.Id;

                    if (adminUserId.HasValue && serviceId.HasValue)
                    {
                        var bookings = new List<Booking>
                    {
                        new Booking
                        {
                            BookerId = adminUserId.Value,
                            ServiceId = serviceId.Value,
                            BookingTime = DateTime.UtcNow,
                            Complete = false
                        }
                    };

                        context.Bookings.AddRange(bookings);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}