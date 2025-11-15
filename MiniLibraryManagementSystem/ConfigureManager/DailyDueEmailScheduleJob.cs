
using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace ConfigureManager
{
    public class DailyDueEmailScheduleJob : IInvocable
    {
        private readonly IRepoManger _repoManger;
        private readonly ILogger<DailyDueEmailScheduleJob> _logger;
        public DailyDueEmailScheduleJob(IRepoManger repoManger,ILogger<DailyDueEmailScheduleJob> logger)
        {
            _repoManger = repoManger;
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("Due Reminder Email Send started at: {time}", DateTimeOffset.Now);
            await _repoManger.EmailLogMailManagementServices.LogEmail();
        }
    }
}
