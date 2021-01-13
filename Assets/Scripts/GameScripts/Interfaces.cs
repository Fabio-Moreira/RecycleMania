namespace GameScripts
{
    public interface IObservable<T, K>
    {
        void Subscribe(IObserver<T, K> o);
        void Notify(T param, K value);
    }

    public interface IObserver<T, K>
    {
        void UpdateObserver(T param, K value);
    }

    public interface IObservable<T>
    {
        void Subscribe(IObserver<T> o);
        void Notify(T value);
    }

    public interface IObserver<T>
    {
        void UpdateObserver(T value);
    }
}