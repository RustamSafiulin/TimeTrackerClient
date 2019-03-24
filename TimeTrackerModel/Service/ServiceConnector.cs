
using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

using Newtonsoft.Json;

using TimeTracker.Helpers;
using TimeTracker.Models;

using RestSharp;

namespace TimeTracker.Service
{
    public class ServiceConnector
    {
        public ServiceConnector(Options options)
        {
            _options = options;
            _routes = new Dictionary<OperationType, Route>();

            _restClient = new RestClient(_options.BaseAddress);
            _jsonSerializer = new Helpers.JsonSerializer();
        }

        #region Props and Fields

        private Options _options;
        private Dictionary<OperationType, Route> _routes;
        private RestClient _restClient;
        private Helpers.JsonSerializer _jsonSerializer;

        #endregion

        private Route GetRegisteredRoute(OperationType name)
        {
            if (!_routes.ContainsKey(name))
                throw new ApplicationException("Operation doesn't registered");

            return _routes[name];
        }

        private RestSharp.RestRequest ConfigureRestRequest<TRequestBody>(Request<TRequestBody> request, RestSharp.Method httpMethod)
        {
            var route = GetRegisteredRoute(request.OpName);
            var restRequest = new RestRequest(route.Path, httpMethod, DataFormat.Json);
            restRequest.JsonSerializer = _jsonSerializer;

            if (request.AuthToken.Key != null)
                restRequest.AddCookie(request.AuthToken.Key, request.AuthToken.Value);

            if (request.UrlSegments != null)
            {
                foreach (var urlSegment in request.UrlSegments)
                {
                    restRequest.AddUrlSegment(urlSegment.Key, urlSegment.Value);
                }
            }

            return restRequest;
        }

        private Response<TSuccessBody, TErrorBody> Execute<TSuccessBody, TErrorBody>(RestSharp.RestRequest restRequest)
            where TSuccessBody : new()
            where TErrorBody : new()
        {
            var response = _restClient.Execute(restRequest);
            if (response.ErrorException != null)
            {
                throw new ApplicationException($"Error retrieving response. Details: {response.ErrorException}");
            }

            var responseResult = new Response<TSuccessBody, TErrorBody>();
            responseResult.StatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                responseResult.SuccessBody = JsonConvert.DeserializeObject<TSuccessBody>(response.Content);
            }
            else
            {
                responseResult.ErrorBody = JsonConvert.DeserializeObject<TErrorBody>(response.Content);
            }
            
            foreach (var cookie in response.Cookies)
            {
                responseResult.CookieStore.Add(cookie.Name, cookie.Value);
            }

            return responseResult;
        }

        public void RegisterRoute(OperationType name, Route route)
        {
            if (_routes.ContainsKey(name))
                throw new ApplicationException("Route already registered");

            _routes[name] = route;
        }

        public Response<TSuccessBody, TErrorBody> UploadFile<TRequestBody, TSuccessBody, TErrorBody>(Request<TRequestBody> request, KeyValuePair<string,string> formFile)
            where TSuccessBody : new()
            where TErrorBody : new()
            where TRequestBody : IDto
        {
            var restRequest = ConfigureRestRequest(request, RestSharp.Method.POST);
            restRequest.AddHeader("Content-Type", "multipart/form-data");

            if (!File.Exists(formFile.Value))
                throw new FileNotFoundException($"Uploaded file {formFile.Value} doesn't exist");

            restRequest.AddFile(formFile.Key, formFile.Value);

            return Execute<TSuccessBody, TErrorBody>(restRequest);
        }

        public DownloadResponse<TErrorBody> DownloadFile<TRequestBody, TErrorBody>(Request<TRequestBody> request)
            where TErrorBody : new()
            where TRequestBody : IDto
        {
            var restRequest = ConfigureRestRequest(request, RestSharp.Method.GET);

            var response = _restClient.Execute(restRequest);
            if (response.ErrorException != null)
            {
                throw new ApplicationException($"Error retrieving response. Details: {response.ErrorException}");
            }

            var responseResult = new DownloadResponse<TErrorBody>();
            responseResult.StatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                responseResult.SuccessBody = response.RawBytes;
            }
            else
            {
                responseResult.ErrorBody = JsonConvert.DeserializeObject<TErrorBody>(response.Content);
            }

            foreach (var cookie in response.Cookies)
            {
                responseResult.CookieStore.Add(cookie.Name, cookie.Value);
            }

            return responseResult;
        }

        public Response<TSuccessBody, TErrorBody> Delete<TRequestBody, TSuccessBody, TErrorBody>(Request<TRequestBody> request)
            where TSuccessBody : new()
            where TErrorBody : new()
            where TRequestBody : IDto
        {
            var restRequest = ConfigureRestRequest(request, RestSharp.Method.DELETE);

            if (request.Body != null)
            {
                foreach (var param in request.Body.ToParamsQuery())
                {
                    restRequest.AddParameter(param.Key, param.Value);
                }
            }

            return Execute<TSuccessBody, TErrorBody>(restRequest);
        }

        public Response<TSuccessBody, TErrorBody> Post<TRequestBody, TSuccessBody, TErrorBody>(Request<TRequestBody> request)
            where TSuccessBody : new()
            where TErrorBody : new()
        {
            var restRequest = ConfigureRestRequest(request, RestSharp.Method.POST);
            restRequest.AddJsonBody(request.Body);

            return Execute<TSuccessBody, TErrorBody>(restRequest);
        }

        public Response<TSuccessBody, TErrorBody> Get<TRequestBody, TSuccessBody, TErrorBody>(Request<TRequestBody> request)
            where TSuccessBody : new()
            where TErrorBody : new()
            where TRequestBody : IDto
        {
            var restRequest = ConfigureRestRequest(request, RestSharp.Method.GET);

            if (request.Body != null)
            {
                foreach (var param in request.Body.ToParamsQuery())
                {
                    restRequest.AddParameter(param.Key, param.Value);
                }
            }

            return Execute<TSuccessBody, TErrorBody>(restRequest);
        }
    }
}
