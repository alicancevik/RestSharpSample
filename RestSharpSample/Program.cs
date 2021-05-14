using RestSharp;
using RestSharpSample.Concrete;
using RestSharpSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSample
{
    class Program
    {


        static void Main(string[] args)
        {
            IGenericHttpClient<ProductDto> httpClientSample = new CustomHttpClient<ProductDto>();

            IProductService productService = new ProductManager(httpClientSample);

            var products = productService.GetAll();

            var product = productService.GetById(1);

            Console.WriteLine("Http Client Worked...");


            IGenericHttpClient<ProductDto> restClientSample = new CustomRestClient<ProductDto>();

            IProductService productService2 = new ProductManager(restClientSample);

            var products2 = productService2.GetAll();

            var product2 = productService2.GetById(1);

            Console.WriteLine("Rest Client Worked...");


            Console.Read();
        }

        private void RestSampleCode()
        {

            var serializer = new JsonSerializer();

            var _builder = new RestBuilder("https://jsonplaceholder.typicode.com/users/1");


            PostsRestClient client = new PostsRestClient(new CacheService(), serializer);

            var response = client.GetByID(1);


            CustomRestClient customRestClient = new CustomRestClient(new CacheService(), serializer, "https://jsonplaceholder.typicode.com/users/1");

            var request = _builder
               .SetFormat(DataFormat.Json)
               .SetMethod(Method.GET)
               .Create();

            var userResponse = customRestClient.Get<UserModel>(request);

            //serializer.Deserialize<>
        }
    }
}
