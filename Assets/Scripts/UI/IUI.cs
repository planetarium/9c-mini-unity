namespace Mini9C.UI
{
    public interface IUI
    {
        void Show();
        void Hide();
    }

    public interface IUI<in T> : IUI
    {
        void Set<TDataProvider>(TDataProvider dataProvider)
            where TDataProvider : T;

        void Show<TDataProvider>(TDataProvider dataProvider)
            where TDataProvider : T;
    }
}
