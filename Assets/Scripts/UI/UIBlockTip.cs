using System;
using System.Collections.Generic;
using Mini9C.ScriptableObjects.EventChannels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Mini9C.UI
{
    public class UIBlockTip : UIBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private BlockTipEventChannel blockTipEventChannel;

        private readonly List<IDisposable> _disposables = new();

        private void OnEnable()
        {
            blockTipEventChannel.BlockIndex
                .Subscribe(blockIndex => text.text = blockIndex.ToString("N0"))
                .AddTo(_disposables);
            blockTipEventChannel.RequestCooldownProgress
                .Subscribe(progress =>
                {
                    var scale = image.rectTransform.localScale;
                    scale.x = progress;
                    image.rectTransform.localScale = scale;
                })
                .AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.ForEach(d => d.Dispose());
            _disposables.Clear();
        }
    }
}
