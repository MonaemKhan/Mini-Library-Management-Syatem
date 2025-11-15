
using ClassRecord;
using ClassRecord.EmailLogManagement;
using DataAccessManager;
using DataBaseModels.DBModels;
using EnumClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceManager.ReturnManagement;
using System.Net;
using System.Net.Mail;

namespace ServiceManager.EmailLogManagement
{
    public interface IEmailLogMailManagementServices
    {
        public Task LogEmail();
        public Task<ReturnRecord> GetAllEmailLog();
    }
    public class EmailLogMailManagementServices : IEmailLogMailManagementServices
    {
        private readonly ILogger<EmailLogMailManagementServices> _logger;
        private readonly IEFCoreDataAccessManager<DueAttaterEmailLog> _dataAccess;
        private readonly IConfiguration _configuration;
        public EmailLogMailManagementServices(IEFCoreDataAccessManager<DueAttaterEmailLog> dataAccess,
            IConfiguration configuration, ILogger<EmailLogMailManagementServices> logger)
        {
            _dataAccess = dataAccess;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ReturnRecord> GetAllEmailLog()
        {
            var emailLogs = await _dataAccess.GetAllAsync();
            return new ReturnRecord(new
            {
                TotalLogs = emailLogs.Count,
                Logs = emailLogs.Select(x=> new
                {
                    email = x.Email,
                    message = x.Message,
                    date = x.SendDate
                })
            },"Get All Result",ResultStatus.Success);
        }

        public async Task LogEmail()
        {
            var query = "SELECT MT.EMAIL, " +
                               "DATEDIFF(day, t.DUEDATE, GETDATE()) as dayDif " +
                        "FROM BorrowDetailsTable T " +
                        "LEFT JOIN MemberManagementTable MT " +
                        "ON MT.MEMBERID = T.MEMBERID " +
                        "WHERE T.ISDELETE = 0 " +
                        "and t.RETURNDATE is null;";

            var emailLogList = await DapperDataAccessManager.QueryList<EmailLogManagementParam>(query);
            await _dataAccess.Transition();
            foreach (var item in emailLogList.Where(x=>x.dayDif > 0)) // only overdue items
            {
                try
                {
                    int charge = Convert.ToInt32(_configuration["DueAmmountSetting:PerDayChargeAmmount"]);

                    DueAttaterEmailLog dueAttaterEmailLog = new DueAttaterEmailLog
                    {
                        Email = item.EMAIL,
                        Message = $"Your book is overdue by {item.dayDif} days. Penalty amount around {item.dayDif * charge}",
                        SendDate = DateTime.Now,
                    };

                    ///
                    //sending email logic here
                    //if you want to send email please configure the EmailSettings in appsettings.json
                    if (_configuration["EmailSettings:IsActive"] == "1")
                    {
                        var server = _configuration["EmailSettings:Server"];
                        var port = _configuration["EmailSettings:Port"];
                        var fromMail = _configuration["EmailSettings:FromMail"];
                        var password = _configuration["EmailSettings:Password"];

                        var smtpClient = new SmtpClient(server)
                        {
                            Port = Convert.ToInt32(port),
                            Credentials = new NetworkCredential(fromMail, password),
                            EnableSsl = true,
                        };

                        var subject = "";
                        var body = "<h3 style='color:red'>Due Alart !!!!</h3>" +
                                   "<p><strong>Message :</strong> " + dueAttaterEmailLog.Message + "</p>";

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(fromMail),
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true, // if you want HTML body
                        };

                        mailMessage.To.Add(dueAttaterEmailLog.Email);

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                    ///


                    await _dataAccess.InsertAsync(dueAttaterEmailLog);
                    await _dataAccess.SaveChangesAsync();
                }
                catch (Exception ex) 
                {
                    //in feature we will add another log table for failed sent email log
                    _logger.LogError($"Failed to send email to {item.EMAIL}. Error: {ex.Message}");
                }
            }
            await _dataAccess.CommitAsync();
        }
    }
}
