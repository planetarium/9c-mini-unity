using System;
using Mini9C.UI;
using UniRx;
using UnityEngine;

namespace Mini9C.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/EventChannels/Create UIControlEventChannel",
        fileName = "UIControlEventChannel",
        order = 0)]
    public class UIControlEventChannel : ScriptableObject
    {
        private UIController _controller;

        public void RegisterController(UIController controller)
        {
            Debug.Log($"[{nameof(UIControlEventChannel)}] {nameof(RegisterController)}()");
            _controller = controller;
        }

        public UIController GetController() => _controller;
    }
}
