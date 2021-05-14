using RestSharp;
using RestSharpSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RestSharpSample.Concrete
{

    public class RestBuilder : IRestBuilder
    {
        #region Private Properties

        private readonly string _resource;
        private readonly Dictionary<string, string> _headers;
        private readonly Dictionary<string, string> _cookies;
        private readonly List<Parameter> _parameters;
        private DataFormat _dataFormat;
        private Method _method;
        private object _body;
        private int _timeOut;

        private string _fileName;
        private string _filePath;
        private string _fileType;

        #endregion Private Properties

        #region Public Properties

        public int HeaderCount => _headers.Count;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Public Constructor with string as argument.
        /// </summary>
        /// <param name="resource"></param>
        public RestBuilder(string resource)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            _resource = resource;
            _headers = new Dictionary<string, string>();
            _parameters = new List<Parameter>();
            _method = Method.GET;
            _dataFormat = DataFormat.Json;
            _cookies = new Dictionary<string, string>();
            _timeOut = 0;
        }

        /// <summary>
        /// Public Constructor with a FormattableString argument. 
        /// The string is stored as a string compiled with arguments.
        /// </summary>
        /// <param name="resource"></param>
        public RestBuilder(FormattableString resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            _resource = resource.ToString();
            _headers = new Dictionary<string, string>();
            _parameters = new List<Parameter>();
            _method = Method.GET;
            _dataFormat = DataFormat.Json;
            _cookies = new Dictionary<string, string>();
            _timeOut = 0;
        }

        /// <summary>
        /// Public Constructor with a Uri argument.
        /// Uri argument is stored as string.
        /// </summary>
        /// <param name="resource"></param>
        public RestBuilder(Uri resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            _resource = resource.ToString();
            _headers = new Dictionary<string, string>();
            _parameters = new List<Parameter>();
            _method = Method.GET;
            _dataFormat = DataFormat.Json;
            _cookies = new Dictionary<string, string>();
            _timeOut = 0;
        }

        /// <summary>
        /// RequestBuilder Constructor with <see cref="Method"/> argument.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        public RestBuilder(string resource, Method method)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            _resource = resource;
            _headers = new Dictionary<string, string>();
            _parameters = new List<Parameter>();
            _method = method;
            _dataFormat = DataFormat.Json;
            _cookies = new Dictionary<string, string>();
            _timeOut = 0;
        }

        /// <summary>
        /// RequestBuilder Constructor with <see cref="Method"/> and <see cref="DataFormat"/> arguments.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <param name="format"></param>
        public RestBuilder(string resource, Method method, DataFormat format)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            _resource = resource;
            _headers = new Dictionary<string, string>();
            _parameters = new List<Parameter>();
            _method = method;
            _dataFormat = format;
            _cookies = new Dictionary<string, string>();
            _timeOut = 0;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Add a serialized object to the IRestRequest.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public IRestBuilder AddBody(object body)
        {
            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            _body = body;
            return this;
        }

        /// <summary>
        /// Adds a file to the RestRequest.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public IRestBuilder AddFile(string name, string path, string contentType = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _fileName = name;
            _filePath = path;
            _fileType = contentType;

            return this;
        }

        /// <summary>
        /// Set the DataFormat of the IRestRequest.
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public IRestBuilder SetFormat(DataFormat dataFormat)
        {
            _dataFormat = dataFormat;
            return this;
        }

        /// <summary>
        /// Add a Header to the IRestRequest.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IRestBuilder AddHeader(string name, string value)
        {
            string headerValue = string.Empty;

            if (_headers.TryGetValue(name, out headerValue))
            {
                if (value != headerValue)
                {
                    _headers[name] = value;
                }

                return this;
            }

            _headers.Add(name, value);
            return this;
        }

        /// <summary>
        /// Add Cookie to Request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IRestBuilder AddCookie(string name, string value)
        {
            string cookieValue = string.Empty;

            if (_cookies.TryGetValue(name, out cookieValue))
            {
                if (value != cookieValue)
                {
                    _cookies[name] = value;
                }

                return this;
            }

            _cookies.Add(name, value);
            return this;
        }

        /// <summary>
        /// Add Headers to the IRestRequest.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public IRestBuilder AddHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                string value = string.Empty;

                if (_headers.TryGetValue(header.Key, out value))
                {
                    if (value != header.Value)
                    {
                        _headers[header.Key] = header.Value;
                    }

                    continue;
                }

                _headers.Add(header.Key, header.Value);
            }

            return this;
        }

        /// <summary>
        /// Set the Method of the IRestRequest.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IRestBuilder SetMethod(Method method)
        {
            _method = method;
            return this;
        }

        /// <summary>
        /// Set the IRestRequest Timeout value.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public IRestBuilder SetTimeout(int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout));
            }

            _timeOut = timeout;
            return this;
        }

        /// <summary>
        /// Add a Parameter to the IRestRequest.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IRestBuilder AddParameter(Parameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (!_parameters.Contains(parameter))
            {
                _parameters.Add(parameter);
            }

            return this;
        }

        /// <summary>
        /// Add Parameters to the <see cref="IRestRequest"/> IRestRequest.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IRestBuilder AddParameters(Parameter[] parameters)
        {
            // TODO: Revisit, this doesn't seem like the best approach.

            var duplicates = _parameters.Select(x => x).Intersect(parameters);

            // Check for duplicates.
            if (!duplicates.Any())
            {
                _parameters.AddRange(parameters);
                return this;
            }

            // Iterate over duplicate items.
            foreach (var dup in duplicates)
            {
                var param = Array.Find(parameters, x => x.Name == dup.Name);

                if (param is null) continue;

                _parameters.Remove(dup);
                _parameters.Add(param);
            }

            return this;
        }

        /// <summary>
        /// Removes a Header by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IRestBuilder RemoveHeader(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_headers.ContainsKey(name))
            {
                _headers.Remove(name);
            }

            return this;
        }

        /// <summary>
        /// Removes All Headers.
        /// </summary>
        /// <returns></returns>
        public IRestBuilder RemoveHeaders()
        {
            _headers.Clear();
            return this;
        }

        /// <summary>
        /// Removes Cookies.
        /// </summary>
        /// <returns></returns>
        public IRestBuilder RemoveCookies()
        {
            _cookies.Clear();
            return this;
        }

        /// <summary>
        /// Removes Parameters.
        /// </summary>
        /// <returns></returns>
        public IRestBuilder RemoveParameters()
        {
            _parameters.Clear();
            return this;
        }

        /// <summary>
        /// Remove a Parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IRestBuilder RemoveParameter(Parameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var param = _parameters.Find(p => p.Name == parameter.Name);

            if (param is null)
            {
                return this;
            }

            _parameters.Remove(param);

            return this;
        }

        /// <summary>
        /// Creates the IRestRequest object.
        /// </summary>
        /// <returns>IRestRequest</returns>
        public IRestRequest Create()
        {
            var request = new RestRequest(_resource, _method, _dataFormat);

            foreach (var param in _parameters)
            {
                request.AddParameter(param);
            }

            if (_body != null)
            {
                request.AddBody(_body);
            }

            foreach (var header in _headers)
            {
                request.AddHeader(header.Key, header.Value);
            }

            foreach (var cookie in _cookies)
            {
                request.AddCookie(cookie.Key, cookie.Value);
            }

            if (!string.IsNullOrEmpty(_fileName) && !string.IsNullOrEmpty(_filePath))
            {
                request.AddFile(_fileName, _filePath, _fileType);
            }

            request.Timeout = _timeOut;

            return request;
        }

        #endregion Public Methods
    }
}
