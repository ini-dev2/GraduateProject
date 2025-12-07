using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace RestBuilder
{
    public sealed class ApiService : IApiService
    {
        private readonly string _baseUrl;

        public ApiService(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        private string BuildUrl(string endpoint) => $"{_baseUrl}/{endpoint.TrimStart('/')}";

        public UniTask<TResponse> GetAsync<TResponse>(string endpoint, RequestOptions options = null)
            => SendRequest<TResponse>(UnityWebRequest.Get(BuildUrl(endpoint)), options);

        public UniTask<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, RequestOptions options = null)
            => SendRequestWithBody<TRequest, TResponse>("POST", endpoint, data, options);

        public UniTask<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, RequestOptions options = null)
            => SendRequestWithBody<TRequest, TResponse>("PUT", endpoint, data, options);

        public UniTask<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data, RequestOptions options = null)
            => SendRequestWithBody<TRequest, TResponse>("PATCH", endpoint, data, options);

        public UniTask<byte[]> GetBytesAsync(string endpoint, RequestOptions options = null)
            => SendRequestForBytes(UnityWebRequest.Get(BuildUrl(endpoint)), options);

        public UniTask<TResponse> DeleteAsync<TResponse>(string endpoint, RequestOptions options = null)
        {
            var request = UnityWebRequest.Delete(BuildUrl(endpoint));
            ApplyHeaders(request, options);
            return SendRequest<TResponse>(request, options);
        }

        public UniTask PostAsync<TRequest>(string endpoint, TRequest data, RequestOptions options = null)
            => SendRequestWithBody("POST", endpoint, data, options);

        public UniTask PutAsync<TRequest>(string endpoint, TRequest data, RequestOptions options = null)
            => SendRequestWithBody("PUT", endpoint, data, options);

        public UniTask PatchAsync<TRequest>(string endpoint, TRequest data, RequestOptions options = null)
            => SendRequestWithBody("PATCH", endpoint, data, options);

        public UniTask DeleteAsync(string endpoint, RequestOptions options = null)
        {
            var request = UnityWebRequest.Delete(BuildUrl(endpoint));
            ApplyHeaders(request, options);
            return SendRequest(request, options);
        }

        private async UniTask SendRequest(UnityWebRequest request, RequestOptions options)
        {
            try
            {
                ApplyHeaders(request, options);

                var operation = await request.SendWebRequest().WithCancellation(options?.CancellationToken ?? default);

                if (request.result != UnityWebRequest.Result.Success)
                    throw new ApiException(request.responseCode, request.downloadHandler?.text);
            }
            finally
            {
                request.Dispose();
            }
        }

        private async UniTask<byte[]> SendRequestForBytes(UnityWebRequest request, RequestOptions options)
        {
            try
            {
                ApplyHeaders(request, options);
                request.downloadHandler = request.downloadHandler ?? new DownloadHandlerBuffer();

                await request.SendWebRequest().WithCancellation(options?.CancellationToken ?? default);

                if (request.result != UnityWebRequest.Result.Success)
                    throw new UnityWebRequestException(request);

                return request.downloadHandler.data;
            }
            finally
            {
                request.Dispose();
            }
        }
        private async UniTask<TResponse> SendRequest<TResponse>(UnityWebRequest request, RequestOptions options)
        {
            try
            {
                ApplyHeaders(request, options);

                var operation = await request.SendWebRequest().WithCancellation(options?.CancellationToken ?? default);

                if (request.result != UnityWebRequest.Result.Success)
                    throw new ApiException(request.responseCode, request.downloadHandler?.text);

                var json = request.downloadHandler.text;
                return JsonConvert.DeserializeObject<TResponse>(json);
            }
            finally
            {
                request.Dispose();
            }
        }

        private async UniTask SendRequestWithBody<TRequest>(string method, string endpoint, TRequest data, RequestOptions options)
        {
            var json = JsonConvert.SerializeObject(data);
            var bodyRaw = Encoding.UTF8.GetBytes(json);

            var request = new UnityWebRequest(BuildUrl(endpoint), method)
            {
                uploadHandler = new UploadHandlerRaw(bodyRaw),
                downloadHandler = new DownloadHandlerBuffer()
            };

            await SendRequest(request, options);
        }

        private async UniTask<TResponse> SendRequestWithBody<TRequest, TResponse>(string method, string endpoint, TRequest data, RequestOptions options)
        {
            var json = JsonConvert.SerializeObject(data);
            var bodyRaw = Encoding.UTF8.GetBytes(json);

            var request = new UnityWebRequest(BuildUrl(endpoint), method)
            {
                uploadHandler = new UploadHandlerRaw(bodyRaw),
                downloadHandler = new DownloadHandlerBuffer()
            };

            return await SendRequest<TResponse>(request, options);
        }

        private void ApplyHeaders(UnityWebRequest request, RequestOptions options)
        {
            if (options == null) return;

            foreach (var header in options.Headers)
                request.SetRequestHeader(header.Key, header.Value);
        }
    }
}