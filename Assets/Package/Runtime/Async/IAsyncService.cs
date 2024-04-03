using System.Threading;
using System.Threading.Tasks;

namespace ADM
{
    public interface IAsyncService
    {
        public delegate Task AsyncOperation(CancellationToken token);

        Task RunAsync(AsyncOperation operation);
    }
}
