using System.Threading;
using System.Threading.Tasks;

namespace ADM
{
    // TODO - This is not complete. It's just something to start making async/await calls.
    //        Will revisit later.

    [ServiceDefinition(typeof(IAsyncService), isSingleton: true)]
    internal class AsyncService : IAsyncService
    {
        private readonly CancellationTokenSource k_ctSrc = new();

        public async Task RunAsync(IAsyncService.AsyncOperation operation)
        {
            k_ctSrc.Token.ThrowIfCancellationRequested();
            await operation(k_ctSrc.Token);
        }
    }
}
