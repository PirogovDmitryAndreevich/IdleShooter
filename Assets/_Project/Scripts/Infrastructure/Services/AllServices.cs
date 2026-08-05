public static class AllServices
{
    public static void RegisterSingle<TService>(TService implementation) where TService : IService =>
        Implementation<TService>.ServiceInstance = implementation;

    public static TService Single<TService>() where TService : IService =>
        Implementation<TService>.ServiceInstance;

    private class Implementation<TService> where TService : IService
    {
        public static TService ServiceInstance;
    }
}
