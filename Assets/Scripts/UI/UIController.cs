using System;
using Mini9C.ScriptableObjects.EventChannels;
using UnityEngine;

namespace Mini9C.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIBehaviour[] uiList;

        [SerializeField]
        private UIControlEventChannel uiControlEventChannel;

        private void Awake()
        {
            uiControlEventChannel.RegisterController(this);
        }

        public void Show(Type type) => Get(type)?.Show();

        public void Show<TUI>() where TUI : UIBehaviour => Show(typeof(TUI));

        public void Show<TUI, TDataProvider>(TDataProvider dataProvider)
            where TUI : UIBehaviour<TDataProvider>
            => Get<TUI>().Show(dataProvider);

        public void Hide(Type type) => Get(type)?.Hide();

        public void Hide<TUI>() where TUI : UIBehaviour => Hide(typeof(TUI));

        public IUI Get(Type type)
        {
            for (var i = 0; i < uiList.Length; i++)
            {
                var ui = uiList[i];
                if (ui.GetType() == type)
                {
                    return ui;
                }
            }

            return null;
        }

        public TUI Get<TUI>() where TUI : IUI => (TUI)Get(typeof(TUI));
    }
}
