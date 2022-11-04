using System;
using System.Collections.Generic;
using Mini9C.ScriptableObjects.EventChannels;
using UniRx;

namespace Mini9C.GameData
{
    public class PlayerInfo : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        public readonly ReactiveProperty<string> Address = new();

        public readonly ReactiveProperty<decimal> NCG = new();

        public readonly Subject<PlayerInfo> OnValueChange = new();

        public PlayerInfo(AgentStateEventChannel agentStateEventChannel)
        {
            // Internal event subscription
            RelayToOnValueChange(Address);
            RelayToOnValueChange(NCG);

            // External event subscription
            agentStateEventChannel.Address
                .Subscribe(Address.SetValueAndForceNotify)
                .AddTo(_disposables);

            agentStateEventChannel.NCG
                .Subscribe(NCG.SetValueAndForceNotify)
                .AddTo(_disposables);
        }

        private void RelayToOnValueChange<T>(IObservable<T> observable) =>
            observable.Select(_ => this).Subscribe(OnValueChange).AddTo(_disposables);

        public void Dispose()
        {
            _disposables.ForEach(e => e.Dispose());
            _disposables.Clear();

            Address.Dispose();
        }
    }
}
