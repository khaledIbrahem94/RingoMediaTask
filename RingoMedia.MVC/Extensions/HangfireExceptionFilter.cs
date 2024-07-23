using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using RingoMedia.MVC.Interface;
using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Extensions
{
    public class HangfireExceptionFilter(ILogger<HangfireExceptionFilter> _logger, ILogs _logs) : JobFilterAttribute, IServerFilter, IElectStateFilter
    {

        public void OnPerforming(PerformingContext context)
        {
        }

        public void OnPerformed(PerformedContext context)
        {
        }

        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is FailedState failedState)
            {
                var exception = failedState.Exception;
                _logger.LogError(exception, "Job {JobId} failed: {ExceptionMessage}", context.BackgroundJob.Id, exception.Message);
                _logs.AddError(new ErrorLog
                {
                    Function = "HangFire",
                    Message = exception.Message,
                });
            }
        }
    }
}
