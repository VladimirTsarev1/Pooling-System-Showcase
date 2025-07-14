namespace Showcase.Providers
{
    public static class ConfigProvider
    {
        public static PoolConfigProvider Pools { get; private set; }

        public static void Initialize()
        {
            Pools = new PoolConfigProvider();
        }
    }
}