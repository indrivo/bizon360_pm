namespace Gear.Common.Extensions.HttpClient
{
    public class DefaultHttpClientAccessor : IHttpClientAccessor
    {
        public System.Net.Http.HttpClient Client { get; }

        public DefaultHttpClientAccessor()
        {
            Client = new System.Net.Http.HttpClient();
        }
    }
}