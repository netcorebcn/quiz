using System;
using System.Threading.Tasks;
using Polly;

namespace Quiz.EventSourcing
{
    public static class TaskExtensions
    {
        public static async Task DefaultRetry (this Task @this) =>
            await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .ExecuteAsync(async () => await @this);
    }
}