using Coravel;
using Microsoft.Extensions.DependencyInjection;
using ServiceManager.BookManagement;
using ServiceManager.BorrowingManagement;
using ServiceManager.EmailLogManagement;
using ServiceManager.Login;
using ServiceManager.MemberManagement;
using ServiceManager.ReportManagement;
using ServiceManager.ReturnManagement;

namespace ConfigureManager.ServiceManager
{
    public static partial class ServiceManager
    {
        public static IServiceCollection RepoConfigure(this IServiceCollection services)
        {
            //reginster Services Here
            services.AddScoped<IBookManagementServices, BookManagementServices>();
            services.AddScoped<IMemberManagementServices, MemberManagementServices>();
            services.AddScoped<IBorrowDetailsServices, BorrowDetailsServices>();
            services.AddScoped<IBorrowBookListServices, BorrowBookListServices>();
            services.AddScoped<IReturnManagementServices, ReturnManagementServices>();
            services.AddScoped<IReportManagementServices, ReportManagementServices>();
            services.AddScoped<IEmailLogMailManagementServices, EmailLogMailManagementServices>();

            services.AddScoped<ILoginService, LoginService>();

            services.AddScoped<IRepoManger, RepoManger>();

            // Register Scheduled Jobs
            services.AddScheduler();
            services.AddScoped<DailyDueEmailScheduleJob>();

            return services;
        }
    }
}
