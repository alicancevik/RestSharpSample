using RestSharp;
using RestSharp.Deserializers;
using RestSharpSample.Interfaces;
using System;

namespace RestSharpSample.Concrete
{
    public class PostModel
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; }
    }

    public class PostsRestClient : CustomRestClient
    {
        public PostsRestClient(ICacheService cacheService, IDeserializer serializer) : base(cacheService, serializer, "https://jsonplaceholder.typicode.com/posts")
        {
           
        }

        public PostModel GetByID(int id)
        {
            RestRequest request = new RestRequest("/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return GetFromCache<PostModel>(request, "Post" + id.ToString());
        }

    }

    public class CustomRestClient : RestSharp.RestClient
    {
        protected ICacheService _cacheService;
        public CustomRestClient(ICacheService cacheService, IDeserializer serializer, string baseUrl)
        {
            _cacheService = cacheService;

            AddHandler("application/json", serializer);
            AddHandler("text/json", serializer);
            AddHandler("text/x-json", serializer);

            BaseUrl = new Uri(baseUrl);
        }

        private void TimeoutCheck(IRestRequest request, IRestResponse response)
        {
            if (response.StatusCode == 0)
            {
                // log
            }
        }

        public override IRestResponse Execute(IRestRequest request)
        {
            var response = base.Execute(request);
            TimeoutCheck(request, response);
            return response;
        }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = base.Execute<T>(request);
            TimeoutCheck(request, response);
            return response;
        }

        public T Get<T>(IRestRequest request) where T : new()
        {
            var response = Execute<T>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                //LogError(BaseUrl, request, response);
                return default(T);
            }
        }

        public T GetFromCache<T>(IRestRequest request, string cacheKey)
      where T : class, new()
        {
            var item = _cacheService.Get<T>(cacheKey);
            if (item == null)
            {
                var response = Execute<T>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _cacheService.Set<T>(cacheKey, response.Data,30);
                    item = response.Data;
                }
                else
                {
                    //LogError(BaseUrl, request, response);
                    return default(T);
                }
            }
            return item;
        }
    }
}
