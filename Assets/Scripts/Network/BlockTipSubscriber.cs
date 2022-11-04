using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mini9C.ScriptableObjects;
using Mini9C.ScriptableObjects.EventChannels;
using Mini9C.UI;
using UnityEngine;

namespace Mini9C.Network
{
    public class BlockTipSubscriber : AsyncSubscriber
    {
        private readonly MainConfig _mainConfig;
        private readonly BlockTipEventChannel _blockTipEventChannel;

        public BlockTipSubscriber(
            MainConfig mainConfig,
            BlockTipEventChannel blockTipEventChannel)
        {
            _mainConfig = mainConfig;
            _blockTipEventChannel = blockTipEventChannel;
        }

        protected override async UniTaskVoid SubscribeAsync(CancellationToken ct)
        {
            // Initialize
            _blockTipEventChannel.BlockIndex.OnNext(0L);
            var deltaTime = 0f;
            UpdateProgress(deltaTime);

            // Update once on start
            await GraphQLWorker.Instance
                .GetBlockTipAsync(_blockTipEventChannel.BlockIndex);
            await UniTask.Yield();

            // Update periodically
            while (!ct.IsCancellationRequested)
            {
                deltaTime += Time.deltaTime;
                UpdateProgress(deltaTime);
                if (deltaTime < _mainConfig.blockTipRequestCooldown)
                {
                    await UniTask.Yield();
                    continue;
                }

                await GraphQLWorker.Instance
                    .GetBlockTipAsync(_blockTipEventChannel.BlockIndex);
                deltaTime = 0f;
                UpdateProgress(deltaTime);
                await UniTask.Yield();
            }
        }

        private void UpdateProgress(float deltaTime)
        {
            var progress = Math.Clamp(
                deltaTime / _mainConfig.blockTipRequestCooldown,
                0f,
                1f);
            _blockTipEventChannel.RequestCooldownProgress.OnNext(progress);
        }
    }
}
