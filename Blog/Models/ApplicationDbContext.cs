using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Blog.Models
{

        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public DbSet<Post> Posts { get; set; }
            public DbSet<Comment> Comments { get; set; }

            public DbSet<Resource> Resources { set; get; }

            public ApplicationDbContext()
                : base("DefaultConnection")
            {
            }


            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                // Change default identity table names
                modelBuilder.Entity<IdentityUser>().ToTable("Blog_Users");


                modelBuilder.Entity<ApplicationUser>()
                    .HasMany<IdentityUserRole>(u => u.Roles);

                modelBuilder.Entity<IdentityUserRole>()
                    .HasKey(iur => new { iur.UserId, iur.RoleId })
                    .ToTable("Blog_UserRoles");

                modelBuilder.Entity<IdentityUserLogin>()
                    .HasKey(iul => new { iul.UserId, iul.LoginProvider, iul.ProviderKey })
                    .ToTable("Blog_UserLogins")
                    .HasRequired(iul => iul.User);

                modelBuilder.Entity<IdentityUserClaim>()
                    .ToTable("Blog_UserClaims")
                    .HasRequired(iuc => iuc.User);

                modelBuilder.Entity<IdentityRole>()
                    .ToTable("Blog_Roles")
                    .Property(ir => ir.Name).IsRequired();
            }

            static ApplicationDbContext()
            {
#if DEBUG
                Database.SetInitializer<ApplicationDbContext>(new DebugStrategy());
#else
            //Database.SetInitializer<ApplicationDbContext>(new ReleaseStrategy());
#endif
            }





        }

        //public class ReleaseStrategy : CreateDatabaseIfNotExists<ApplicationDbContext> { }
        public class DebugStrategy : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                base.Seed(context);

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }
                var authorUser = new ApplicationUser() { UserName = "Andrea", Email = "andrea@domain.com" };
                var adminResult = userManager.Create(authorUser, "password");

                if (adminResult.Succeeded)
                {
                    userManager.AddToRole(authorUser.Id, "Admin");
                }


                var user = new ApplicationUser() { UserName = "User1", Email = "user@mail.com" };
                userManager.Create(user, "123456");

                Post post = new Post()
                {
                    Author = authorUser,
                    Titel = "This is a post",
                    Content = "Donec mattis, purus nec placerat bibendum, dui pede condimentum odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra" +
                    "condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. In tristique orci porttitor ipsum. Lorem ipsum dolor sit amet," +
                    " consectetuer adipiscing elit. Donec libero. Suspendisse bibendum." +
                    "Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu posuere nunc justo tempus leo." +
                    "Donec mattis, purus nec placerat bibendum, dui pede condimentum odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra" +
                    "condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. In tristique orci porttitor ipsum. Lorem ipsum dolor sit amet," +
                    " consectetuer adipiscing elit. Donec libero. Suspendisse bibendum." +
                    "Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu posuere nunc justo tempus leo.",
                    CreateDate = DateTime.Now,
                    Status = PostStatus.draft,

                };
                context.Posts.Add(post);

                context.SaveChanges();

                post = new Post()
                {
                    Author = authorUser,
                    Titel = "Lorem tellus eleifend magna",
                    Content = "Donec mattis, purus nec placerat bibendum, dui pede condimentum odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra" +
                    "condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. In tristique orci porttitor ipsum. Lorem ipsum dolor sit amet," +
                    " consectetuer adipiscing elit. Donec libero. Suspendisse bibendum." +
                    "Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu posuere nunc justo tempus leo." +
                    "Donec mattis, purus nec placerat bibendum, dui pede condimentum odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra" +
                    "condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. In tristique orci porttitor ipsum. Lorem ipsum dolor sit amet," +
                    " consectetuer adipiscing elit. Donec libero. Suspendisse bibendum." +
                    "Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu posuere nunc justo tempus leo.",
                    CreateDate = DateTime.Now.AddDays(-2),
                    Status = PostStatus.published,
                    PublishDate = DateTime.Now.AddDays(-2)
                };
                context.Posts.Add(post);

                context.SaveChanges();

                post = new Post()
                {
                    Author = authorUser,
                    Titel = "Purus nec placerat bibendum",
                    Content = "Donec mattis, purus nec placerat bibendum, dui pede condimentum odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra" +
                    "condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. In tristique orci porttitor ipsum. Lorem ipsum dolor sit amet," +
                    " consectetuer adipiscing elit. Donec libero. Suspendisse bibendum." +
                    "Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu posuere nunc justo tempus leo." +
                    "Donec mattis, purus nec placerat bibendum, dui pede condimentum odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra" +
                    "condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque. Curabitur vel urna. In tristique orci porttitor ipsum. Lorem ipsum dolor sit amet," +
                    " consectetuer adipiscing elit. Donec libero. Suspendisse bibendum." +
                    "Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu posuere nunc justo tempus leo.",
                    CreateDate = DateTime.Now.AddMonths(-3),
                    Status = PostStatus.published,
                    PublishDate = DateTime.Now.AddMonths(-3)
                };
                context.Posts.Add(post);

                context.SaveChanges();

                Comment comment = new Comment()
                {
                    Content = "This is a comment",
                    ApplicationUserId = user.Id,
                    CreateDate = DateTime.Now.AddMinutes(-30),
                    Email = user.Email,
                    Approved = true,
                    PostId = post.Id,
                };

                context.Comments.Add(comment);
                context.SaveChanges();

                var comment2 = new Comment()
                {
                    Content = "This is a reply to a comment",
                    ApplicationUserId = user.Id,
                    CreateDate = DateTime.Now.AddHours(-2),
                    Email = user.Email,
                    Approved = true,
                    ParentCommentId = comment.Id,
                    PostId = post.Id,
                };

                context.Comments.Add(comment2);
                context.SaveChanges();

                var comment3 = new Comment()
                {
                    Content = "This is a reply to a reply",
                    ApplicationUserId = user.Id,
                    CreateDate = DateTime.Now.AddHours(-2),
                    Email = user.Email,
                    Approved = true,
                    ParentCommentId = comment2.Id,
                    PostId = post.Id,
                };

                context.Comments.Add(comment3);
                context.SaveChanges();

                var comment4 = new Comment()
                {
                    Content = "Yet another comment",
                    ApplicationUserId = user.Id,
                    CreateDate = DateTime.Now.AddHours(-2),
                    Email = user.Email,
                    Approved = true,
                    PostId = post.Id,
                };

                context.Comments.Add(comment4);
                context.SaveChanges();


            }
        }
    }
