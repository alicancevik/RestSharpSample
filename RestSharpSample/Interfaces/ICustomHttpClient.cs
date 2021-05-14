using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSample.Interfaces
{
    public class ProductDto : IDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
    public interface IDto
    {

    }

    public interface ICustomHttpClient<T> where T : IDto
    {
        List<T> GetAll();
    }

    public interface IProductService
    {
        List<ProductDto> GetAll();
        ProductDto GetById(int productId);
        void Add(ProductDto productDto);
    }

    public interface IGenericHttpClient<T>
    {
        List<T> GetAll();
        T GetById(int id);
    }

    public class CustomHttpClient<T> : HttpClient, IGenericHttpClient<T>
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:56148/api/");
        }

        public List<T> GetAll()
        {
            var responseMessage = _httpClient.GetAsync("products").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<T>>(responseMessage.Content.ReadAsStringAsync().Result);
            }

            return new List<T>();
        }

        public T GetById(int id)
        {
            var responseMessage = _httpClient.GetAsync("products/1").Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseMessage.Content.ReadAsStringAsync().Result);
            }

            return default(T);
        }
    }

    public class CustomRestClient<T> : RestClient, IGenericHttpClient<T>
    {
        private readonly RestClient _httpClient;

        public CustomRestClient()
        {
            _httpClient = new RestClient();
            _httpClient.BaseUrl = new Uri("http://localhost:56148/api/");
        }

        public List<T> GetAll()
        {
            var request = new RestRequest("products", Method.GET);
            var queryResult = _httpClient.Execute<List<T>>(request).Data;

            if (queryResult != null)
                return queryResult;

            return new List<T>();
        }

        public T GetById(int id)
        {
            var request = new RestRequest("products/1", Method.GET);
            var queryResult = _httpClient.Execute<T>(request).Data;

            if (queryResult != null)
                return queryResult;

            return default(T);
        }
    }

    public class ProductManager : IProductService
    {
        private readonly IGenericHttpClient<ProductDto> _genericHttpClient;

        public ProductManager(IGenericHttpClient<ProductDto> genericHttpClient)
        {
            _genericHttpClient = genericHttpClient;
        }

        public void Add(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public List<ProductDto> GetAll()
        {
            return _genericHttpClient.GetAll();
        }

        public ProductDto GetById(int productId)
        {
            return _genericHttpClient.GetById(productId);
        }
    }

}
