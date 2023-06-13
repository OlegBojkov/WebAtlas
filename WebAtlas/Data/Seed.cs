using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Diagnostics;
using WebAtlas.Data;
using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                //Adresses
                if (!context.Adresses.Any())
                {
                    context.Adresses.AddRange(new List<Address>()
                    {
                        new Address
                        {
                            City = "New York",
                            Street = "Broadway",
                            HouseNumber = "123"
                        },
                        new Address
                        {
                            City = "Los Angeles",
                            Street = "Hollywood Blvd",
                            HouseNumber = "456"
                        },
                        new Address
                        {
                            City = "Chicago",
                            Street = "Michigan Ave",
                            HouseNumber = "789"
                        }

                    });
                    context.SaveChanges();
                }
                //Litas
                if (!context.Events.Any())
                {
                    context.Events.AddRange(new List<Event>()
                    {
                        new Event
                        {
                            Title = "Event 1",
                            Description = "Our latest book 'The Art of Fiction' is now available in stores",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod sed arcu sit amet congue. Etiam consequat, purus sed tempor molestie, quam velit eleifend sapien, vel malesuada metus est eget nisi. Sed bibendum, felis vel feugiat posuere, nisl massa aliquam libero, sed semper velit lorem et nulla.",
                            Image = "https://s0.rbk.ru/v6_top_pics/resized/590xH/media/img/1/83/756079611261831.jpg",
                            DateCreated = DateTime.Now,
                            DateEvent = DateTime.Parse("2024-03-15"),
                            Category = Category.Книги
                        },
                        new Event
                        {
                            Title = "Event 2",
                            Description = "Our latest book 'The Art of Fiction' is now available in stores",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod sed arcu sit amet congue. Etiam consequat, purus sed tempor molestie, quam velit eleifend sapien, vel malesuada metus est eget nisi. Sed bibendum, felis vel feugiat posuere, nisl massa aliquam libero, sed semper velit lorem et nulla.",
                            Image = "https://s0.rbk.ru/v6_top_pics/resized/590xH/media/img/1/83/756079611261831.jpg",
                            DateCreated = DateTime.Now,
                            DateEvent = DateTime.Parse("2024-03-15"),
                            Category = Category.Авторы
                        },
                        new Event
                        {
                            Title = "Event 3",
                            Description = "Our latest book 'The Art of Fiction' is now available in stores",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod sed arcu sit amet congue. Etiam consequat, purus sed tempor molestie, quam velit eleifend sapien, vel malesuada metus est eget nisi. Sed bibendum, felis vel feugiat posuere, nisl massa aliquam libero, sed semper velit lorem et nulla.",
                            Image = "https://s0.rbk.ru/v6_top_pics/resized/590xH/media/img/1/83/756079611261831.jpg",
                            DateCreated = DateTime.Now,
                            DateEvent = DateTime.Parse("2024-03-15"),
                            Category = Category.Новое
                        }
                    });
                    context.SaveChanges();
                }
                //News
                if (!context.News.Any())
                {
                    context.News.AddRange(new List<News>()
                    {
                        new News()
                        {
                            Title = "New book release",
                            Description = "Our latest book 'The Art of Fiction' is now available in stores",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod sed arcu sit amet congue. Etiam consequat, purus sed tempor molestie, quam velit eleifend sapien, vel malesuada metus est eget nisi. Sed bibendum, felis vel feugiat posuere, nisl massa aliquam libero, sed semper velit lorem et nulla.",
                            Image = "https://s0.rbk.ru/v6_top_pics/resized/590xH/media/img/1/83/756079611261831.jpg",
                            DateCreated = DateTime.Now,
                            Category = Category.Книги
                        },
                        new News()
                        {
                            Title = "Poetry reading event",
                            Description = "Join us for an evening of poetry readings from local writers",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod sed arcu sit amet congue. Etiam consequat, purus sed tempor molestie, quam velit eleifend sapien, vel malesuada metus est eget nisi. Sed bibendum, felis vel feugiat posuere, nisl massa aliquam libero, sed semper velit lorem et nulla.",
                            Image = "https://s0.rbk.ru/v6_top_pics/resized/590xH/media/img/1/83/756079611261831.jpg",
                            DateCreated = DateTime.Now,
                            Category = Category.Авторы
                        },
                        new News()
                        {
                            Title = "Call for submissions",
                            Description = "We are now accepting submissions for our upcoming anthology",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod sed arcu sit amet congue. Etiam consequat, purus sed tempor molestie, quam velit eleifend sapien, vel malesuada metus est eget nisi. Sed bibendum, felis vel feugiat posuere, nisl massa aliquam libero, sed semper velit lorem et nulla.",
                            Image = "https://s0.rbk.ru/v6_top_pics/resized/590xH/media/img/1/83/756079611261831.jpg",
                            DateCreated = DateTime.Now,
                            Category = Category.Новое
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "bojkov.oleg@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "oleg",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        NameLita = "Любители книг",
                        Description = "Мы любим книги, очень",
                        Address = new Address()
                        {
                            City = "Волгоград",
                            Street = "Кузнецова",
                            HouseNumber = "5"
                        },
                        City = "Волгоград",
                        Street = "Кузнецова",
                        HouseNumber = "5",
                        ContactEmail = "lita1@example.com",
                        ContactPhone = "123-456-7890"
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "test2@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "test2",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        NameLita = "Кушать и читать",
                        Description = "Мы любим кушать, очень",
                        Address = new Address()
                        {
                            City = "Москва",
                            Street = "Бажова",
                            HouseNumber = "11"
                        },
                        City = "Москва",
                        Street = "Бажова",
                        HouseNumber = "11",
                        ContactEmail = "lita2@example.com",
                        ContactPhone = "123-456-7890"
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                string appUserEmail3 = "test3@gmail.com";

                var appUser3 = await userManager.FindByEmailAsync(appUserEmail3);
                if (appUser3 == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "test3",
                        Email = appUserEmail3,
                        EmailConfirmed = true,
                        NameLita = "Чтение ABCD",
                        Description = "Изучаем историю книг",
                        Address = new Address()
                        {
                            City = "Ростов",
                            Street = "Ремесленная",
                            HouseNumber = "8"
                        },
                        City = "Ростов",
                        Street = "Ремесленная",
                        HouseNumber = "8",
                        ContactEmail = "lita3@example.com",
                        ContactPhone = "123-456-7890"
                    };
                    await userManager.CreateAsync(appUser3, "Coding@1234?");
                    await userManager.AddToRoleAsync(appUser3, UserRoles.User);
                }
            }
        }
    }
}
