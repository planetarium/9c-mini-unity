using System;

namespace Mini9C.UI
{
    public interface IUIGetter
    {
        TUI Get<TUI>() where TUI : IUI;
        IUI Get(Type type);
    }
}
