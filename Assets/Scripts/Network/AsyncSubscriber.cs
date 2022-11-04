using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mini9C.Network
{
    public abstract class AsyncSubscriber : ISubscribable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        protected AsyncSubscriber()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public virtual ISubscribable StartSubscription()
        {
            SubscribeAsync(GetCancellationToken()).Forget();
            return this;
        }

        protected virtual UniTaskVoid SubscribeAsync(CancellationToken ct)
        {
            return new UniTaskVoid();
        }

        public virtual void StopSubscription()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        protected CancellationToken GetCancellationToken() =>
            _cancellationTokenSource.Token;
    }
}
