using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mini9C.ScriptableObjects;
using Mini9C.ScriptableObjects.EventChannels;
using UniRx;

namespace Mini9C.Network
{
    public class AgentSubscriber : AsyncSubscriber
    {
        private readonly MainConfig _mainConfig;
        private readonly AgentStateEventChannel _agentStateEventChannel;
        private long _lastBlockIndex;
        private string _agentAddress;
        private readonly List<IDisposable> _disposables = new();

        public AgentSubscriber(
            MainConfig mainConfig,
            AgentStateEventChannel agentStateEventChannel,
            BlockTipEventChannel blockTipEventChannel,
            ReactiveProperty<string> agentAddress)
        {
            _mainConfig = mainConfig;
            _agentStateEventChannel = agentStateEventChannel;

            blockTipEventChannel.BlockIndex
                .Subscribe(value => _lastBlockIndex = value)
                .AddTo(_disposables);
            agentAddress.Subscribe(value => _agentAddress = value)
                .AddTo(_disposables);
        }

        protected override async UniTaskVoid SubscribeAsync(CancellationToken ct)
        {
            // Initialize
            _agentStateEventChannel.Address.OnNext(string.Empty);
            UpdateProgress(0);

            // Wait for agent address
            await UniTask.WaitUntil(
                () => !string.IsNullOrEmpty(_agentAddress),
                cancellationToken: ct);

            // Update once on start
            await GraphQLWorker.Instance.GetAgentStateAsync(
                _agentAddress,
                _agentStateEventChannel);
            var lastBlockIndex = _lastBlockIndex;
            await UniTask.Yield();

            // Update periodically
            while (!ct.IsCancellationRequested)
            {
                var deltaBlockCount = _lastBlockIndex - lastBlockIndex;
                UpdateProgress(deltaBlockCount);
                if (deltaBlockCount < _mainConfig.agentStateRequestCooldown)
                {
                    await UniTask.Yield();
                    continue;
                }

                await GraphQLWorker.Instance.GetAgentStateAsync(
                    _agentAddress,
                    _agentStateEventChannel);
                lastBlockIndex = _lastBlockIndex;
                UpdateProgress(deltaBlockCount);
                await UniTask.Yield();
            }
        }

        private void UpdateProgress(long deltaBlockCount)
        {
            var progress = Math.Clamp(
                (float)deltaBlockCount / _mainConfig.agentStateRequestCooldown,
                0f,
                1f);
            _agentStateEventChannel.RequestCooldownProgress.OnNext(progress);
        }

        public void Dispose()
        {
            _disposables.ForEach(e => e.Dispose());
            _disposables.Clear();
        }
    }
}
