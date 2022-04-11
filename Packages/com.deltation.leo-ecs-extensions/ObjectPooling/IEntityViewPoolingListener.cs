namespace DELTation.LeoEcsExtensions.ObjectPooling
{
    public interface IEntityViewPoolingListener
    {
        void OnPreCreated();
        void OnPreDisposed();
    }
}