using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TeamWebApplication.Data;
using TeamWebApplication.Data.Database;

UserContainer userContainer = new UserContainer();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Established connection with PostgreSQL database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(connectionString));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddSingleton<IUserContainer, UserContainer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //Course Environment
    endpoints.MapControllerRoute(
        name: "CourseEnvironmentIndex",
        pattern: "CourseEnvironment/Index/{courseId}",
        defaults: new { controller = "CourseEnvironment", action = "Index" }
    );

    endpoints.MapControllerRoute(
        name: "CourseEnvironmentTeacherIndex",
        pattern: "CourseEnvironment/TeacherIndex/{courseId}",
        defaults: new { controller = "CourseEnvironment", action = "TeacherIndex" }
    );

    endpoints.MapControllerRoute(
        name: "CourseEnvironmentCreateTextPost",
        pattern: "CourseEnvironment/CreateTextPost/{courseId}",
        defaults: new { controller = "CourseEnvironment", action = "CreateTextPost" }
    );

    endpoints.MapControllerRoute(
        name: "CourseEnvironmentCreateLinkPost",
        pattern: "CourseEnvironment/CreateLinkPost/{courseId}",
        defaults: new { controller = "CourseEnvironment", action = "CreateLinkPost" }
    );

    endpoints.MapControllerRoute(
        name: "CourseEnvironmentEditTextPost",
        pattern: "CourseEnvironment/EditTextPost/{postId}",
        defaults: new { controller = "CourseEnvironment", action = "EditTextPost" }
    );

    endpoints.MapControllerRoute(
        name: "CourseEnvironmentEditLinkPost",
        pattern: "CourseEnvironment/EditLinkPost/{postId}",
        defaults: new { controller = "CourseEnvironment", action = "EditLinkPost" }
    );

	endpoints.MapControllerRoute(
    	name: "CourseEnvironmentDeleteTextPost",
	    pattern: "CourseEnvironment/DeleteTextPost/{postId}",
	    defaults: new { controller = "CourseEnvironment", action = "DeleteTextPost" }
    );

	endpoints.MapControllerRoute(
		name: "CourseEnvironmentDeleteLinkPost",
		pattern: "CourseEnvironment/DeleteLinkPost/{postId}",
		defaults: new { controller = "CourseEnvironment", action = "DeleteLinkPost" }
	);

	endpoints.MapControllerRoute(
        name: "CourseEnvironmentAddComment",
        pattern: "CourseEnvironment/AddComment/{courseId}",
        defaults: new { controller = "CourseEnvironment", action = "AddComment" }
    );

    endpoints.MapControllerRoute(
        name: "CourseEnvironment",
        pattern: "CourseEnvironment/DeleteComment/{courseId}",
        defaults: new { controller = "CourseEnvironment", action = "Delete" }
    );

    endpoints.MapControllerRoute(
       name: "CourseEnvironment",
       pattern: "CourseEnvironment/EditComment/{courseId}",
       defaults: new { controller = "CourseEnvironment", action = "EditComment" }
    );

    //Course
    endpoints.MapControllerRoute(
        name: "CourseEdit",
        pattern: "CourseEdit/{courseId}",
        defaults: new { controller = "Course", action = "Edit" }
    );

    endpoints.MapControllerRoute(
        name: "CourseDelete",
        pattern: "CourseDelete/{courseId}",
        defaults: new { controller = "Course", action = "Delete" }
    );

    endpoints.MapControllerRoute(
        name: "AddUser",
        pattern: "AddUser/{courseId}",
        defaults: new { controller = "Course", action = "AddUser" }
    );

    endpoints.MapControllerRoute(
        name: "RemoveUser",
        pattern: "RemoveUser/{courseId}",
        defaults: new { controller = "Course", action = "RemoveUser" }
    );

    endpoints.MapControllerRoute(
       name: "CreatePost",
       pattern: "CourseEnvironment/TeacherIndex/CreatePost/{courseId}",
       defaults: new { controller = "TeacherIndex", action = "CreatePost" }
   );

    endpoints.MapControllerRoute(
      name: "EditPost",
      pattern: "CourseEnvironment/TeacherIndex/EditPost/{courseId}",
      defaults: new { controller = "TeacherIndex", action = "EditPost" }
  );

    endpoints.MapControllerRoute(
     name: "DeletePost",
     pattern: "CourseEnvironment/TeacherIndex/DeletePost/{courseId}",
     defaults: new { controller = "TeacherIndex", action = "DeletePost" }
 );
    //Default
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});
app.Run();