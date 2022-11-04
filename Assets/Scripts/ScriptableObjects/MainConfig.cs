using System;
using System.Collections.Generic;
using Mini9C.ScriptableObjects.EventChannels;
using SimpleGraphQL;
using UniRx;
using UnityEngine;

namespace Mini9C.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/Create MainConfig",
        fileName = "MainConfig",
        order = 0)]
    public class MainConfig : ScriptableObject
    {
        public GraphQLConfig graphQlConfig;

        [Tooltip("Block tip request cooldown in seconds")]
        public float blockTipRequestCooldown = 10f;

        [Tooltip("Agent state request cooldown in block count")]
        public int agentStateRequestCooldown = 3;

        [SerializeField]
        private MainConfigEventChannel mainConfigEventChannel;

        private readonly List<IDisposable> _disposables = new();

        private void OnEnable()
        {
            if (mainConfigEventChannel is null)
            {
                return;
            }

            this.ObserveEveryValueChanged(e => e.blockTipRequestCooldown)
                .Subscribe(value => mainConfigEventChannel.BlockTipRequestCooldown.OnNext(value))
                .AddTo(_disposables);

            mainConfigEventChannel.SetBlockTipRequestCooldown
                .Subscribe(value => blockTipRequestCooldown = value)
                .AddTo(_disposables);
            mainConfigEventChannel.SetAgentStateRequestCooldown
                .Subscribe(value => agentStateRequestCooldown = value)
                .AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.ForEach(e => e.Dispose());
            _disposables.Clear();
        }

        public override string ToString()
        {
            return nameof(MainConfig) +
                   $"\n- graph ql config: {graphQlConfig}" +
                   $"\n- block tip interval seconds: {blockTipRequestCooldown}";
        }
    }
}
