using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Blog.Models;
using Blog.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Blog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<Blog.Models.ApplicationDbContext, Blog.Migrations.Configuration>());

            //Autofac Configuration
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            //builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            var dbContextParameter = new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext),
                                                        (pi, ctx) => ctx.Resolve<ApplicationDbContext>());

            builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>().WithParameter(dbContextParameter).InstancePerLifetimeScope();

            //builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>();
            builder.RegisterType<UserManager<ApplicationUser>>();

            //builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            //builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();


            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerRequest();

            builder.RegisterType<CommentRepository>()
                .As<ICommentRepository>()
                .InstancePerRequest();

            builder.RegisterType<PostRepository>()
                .As<IPostRepository>()
                .InstancePerRequest();

            builder.RegisterType<ResourceRepository>()
                .As<IResourceRepository>()
                .InstancePerRequest();


            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
