using System;
using System.Threading.Tasks;
using Polly;

namespace Quiz.Voting.Results
{
    public static class RetryExtensions
    {
        public static async Task DefaultRetryAsync(Func<Task> action) =>
            await RetryAsync(action);

        public static async Task RetryAsync(Func<Task> action, int retries = 5) =>
            await Policy.Handle<Exception>()
            .WaitAndRetryAsync(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(action);
    }
}
