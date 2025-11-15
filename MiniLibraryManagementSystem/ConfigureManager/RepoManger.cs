using ServiceManager.BookManagement;
using ServiceManager.BorrowingManagement;
using ServiceManager.EmailLogManagement;
using ServiceManager.Login;
using ServiceManager.MemberManagement;
using ServiceManager.ReportManagement;
using ServiceManager.ReturnManagement;

namespace ConfigureManager
{
    public interface IRepoManger
    {
        public IBookManagementServices BookManagementServices { get; }
        public IMemberManagementServices MemberManagementServices { get; }
        public IBorrowDetailsServices BorrowDetailsServices { get; }
        public IReturnManagementServices ReturnManagementServices { get; }
        public IReportManagementServices ReportManagementServices { get; }
        public IEmailLogMailManagementServices EmailLogMailManagementServices { get; }
        public ILoginService LoginService { get; }
    }
    public class RepoManger : IRepoManger
    {
        private readonly IBookManagementServices _bookManagementServices;
        private readonly IMemberManagementServices _memberManagementServices;
        private readonly IBorrowDetailsServices _borrowDetailsServices;
        private readonly IReturnManagementServices _returnManagementServices;
        private readonly IReportManagementServices _reportManagementServices;
        private readonly IEmailLogMailManagementServices _emailLogMailManagementServices;
        private readonly ILoginService _loginService;
        public RepoManger(IBookManagementServices bookManagementServices,            
                          IMemberManagementServices memberManagementServices,
                          IBorrowDetailsServices borrowDetailsServices,
                          IReturnManagementServices returnManagementServices,
                          IReportManagementServices reportManagementServices,
                          IEmailLogMailManagementServices emailLogMailManagementServices,
                          ILoginService loginService)
        {
            _bookManagementServices = bookManagementServices;
            _loginService = loginService;
            _memberManagementServices = memberManagementServices;
            _borrowDetailsServices = borrowDetailsServices;
            _returnManagementServices = returnManagementServices;
            _reportManagementServices = reportManagementServices;
            _emailLogMailManagementServices = emailLogMailManagementServices;
        }
        public IBookManagementServices BookManagementServices => _bookManagementServices;
        public IMemberManagementServices MemberManagementServices => _memberManagementServices;
        public IBorrowDetailsServices BorrowDetailsServices => _borrowDetailsServices;
        public IReturnManagementServices ReturnManagementServices => _returnManagementServices;
        public IReportManagementServices ReportManagementServices => _reportManagementServices;
        public IEmailLogMailManagementServices EmailLogMailManagementServices => _emailLogMailManagementServices;
        public ILoginService LoginService => _loginService;
    }
}
