using UnityEngine;

namespace Mini9C.UI
{
    public abstract class UIBehaviour : MonoBehaviour, IUI
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public abstract class UIBehaviour<T> : UIBehaviour, IUI<T>
    {
        public virtual void Set<TDataProvider>(TDataProvider dataProvider)
            where TDataProvider : T
        {
        }

        public virtual void Show<TDataProvider>(TDataProvider dataProvider)
            where TDataProvider : T
        {
            Set(dataProvider);
            Show();
        }
    }
}
