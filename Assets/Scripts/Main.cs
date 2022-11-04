using System;
using System.Collections.Generic;
using Mini9C.GameData;
using Mini9C.Network;
using Mini9C.ScriptableObjects;
using Mini9C.ScriptableObjects.EventChannels;
using Mini9C.UI;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mini9C
{
    public class Main :
        MonoBehaviour,
        UIPlayerInfo.IDataProvider
    {
        [SerializeField]
        private MainConfig config;

        [SerializeField]
        private UIControlEventChannel uiControlEventChannel;

        [SerializeField]
        private BlockTipEventChannel blockTipEventChannel;

        [SerializeField]
        private AgentStateEventChannel agentStateEventChannel;

        [SerializeField]
        private SigninEventChannel signinEventChannel;

        private readonly List<ISubscribable> _subscribers = new();
        private readonly List<IDisposable> _disposables = new();

        public PlayerInfo PlayerInfo { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(config);

            GraphQLWorker.Instance.Initialize(config.graphQlConfig);

            PlayerInfo = new PlayerInfo(agentStateEventChannel);
        }

        private void Start()
        {
            StartSubscriptions();

            // Show UISignin
            uiControlEventChannel.GetController().Show<UISignin>();
        }

        private void OnDestroy()
        {
            StopSubscriptions();
        }

        private void StartSubscriptions()
        {
            new BlockTipSubscriber(config, blockTipEventChannel)
                .StartSubscription()
                .AddTo(_subscribers);
            new AgentSubscriber(
                    config,
                    agentStateEventChannel,
                    blockTipEventChannel,
                    PlayerInfo.Address)
                .StartSubscription()
                .AddTo(_subscribers);

            signinEventChannel.SetAddress.Subscribe(addr =>
            {
                agentStateEventChannel.Address.OnNext(addr);
                var controller = uiControlEventChannel.GetController();
                controller.Hide<UISignin>();
                controller.Show<UIPlayerInfo, UIPlayerInfo.IDataProvider>(this);
            }).AddTo(_disposables);
        }

        private void StopSubscriptions()
        {
            _subscribers.ForEach(e => e.StopSubscription());
            _subscribers.Clear();

            _disposables.ForEach(e => e.Dispose());
            _disposables.Clear();
        }
    }
}
