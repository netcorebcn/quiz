using System;
using System.Threading.Tasks;
using Polly;

namespace Quiz.Api
{
    public static class RetryExtensions
    {
        public static void DefaultRetry(Action action) => Retry(action);
        
        public static void Retry(Action action, int retries = 5) =>
            Policy.Handle<Exception>()
            .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .Execute(action);
    }
}
