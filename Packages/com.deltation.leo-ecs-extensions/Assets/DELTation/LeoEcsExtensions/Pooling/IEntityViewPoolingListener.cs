namespace DELTation.LeoEcsExtensions.Pooling
{
    public interface IEntityViewPoolingListener
    {
        void OnPreCreated();
        void OnPreDisposed();
    }
}