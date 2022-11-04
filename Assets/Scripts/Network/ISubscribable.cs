namespace Mini9C.Network
{
    public interface ISubscribable
    {
        ISubscribable StartSubscription();

        void StopSubscription();
    }
}
