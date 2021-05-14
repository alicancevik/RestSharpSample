using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSample.Interfaces
{
    public interface IRestBuilder
    {
        int HeaderCount { get; }

        IRestBuilder AddCookie(string name, string value);

        IRestBuilder AddHeader(string name, string value);

        IRestBuilder AddHeaders(Dictionary<string, string> headers);

        IRestBuilder SetTimeout(int timeout);

        IRestBuilder SetMethod(Method method);

        IRestBuilder AddParameter(Parameter parameter);

        IRestBuilder AddParameters(Parameter[] parameters);

        IRestBuilder RemoveHeaders();

        IRestBuilder RemoveHeader(string name);

        IRestBuilder RemoveCookies();

        IRestBuilder RemoveParameters();

        IRestBuilder RemoveParameter(Parameter parameter);

        IRestBuilder SetFormat(DataFormat dataFormat);

        IRestBuilder AddBody(object body);

        IRestBuilder AddFile(string name, string path, string contentType = null);

        IRestRequest Create();
    }
}
