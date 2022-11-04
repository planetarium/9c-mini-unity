using System;
using System.Collections.Generic;
using Mini9C.GameData;
using Mini9C.ScriptableObjects.EventChannels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Mini9C.UI
{
    public class UIPlayerInfo : UIBehaviour<UIPlayerInfo.IDataProvider>
    {
        public interface IDataProvider
        {
            PlayerInfo PlayerInfo { get; }
        }

        [SerializeField]
        private Image image;

        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private AgentStateEventChannel agentStateEventChannel;

        private readonly List<IDisposable> _disposables = new();

        public override void Set<TDataProvider>(TDataProvider dataProvider)
        {
            Dispose();
            dataProvider.PlayerInfo.OnValueChange
                .Subscribe(OnValueChange)
                .AddTo(_disposables);
            agentStateEventChannel.RequestCooldownProgress
                .Subscribe(progress =>
                {
                    var scale = image.rectTransform.localScale;
                    scale.x = progress;
                    image.rectTransform.localScale = scale;
                })
                .AddTo(_disposables);
        }

        private void OnValueChange(PlayerInfo playerInfo)
        {
            text.text = $"{playerInfo.Address.Value}\n" +
                        $"{playerInfo.NCG.Value} NCG";
        }

        private void Dispose()
        {
            _disposables.ForEach(x => x.Dispose());
            _disposables.Clear();
        }
    }
}
