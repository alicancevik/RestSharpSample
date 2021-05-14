namespace RestSharpSample.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key) where T : class;
        void Set<T>(string key, object data,int minutes);
    }
}
