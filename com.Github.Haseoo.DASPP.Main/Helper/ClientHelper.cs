using com.Github.Haseoo.DASPP.CoreData.Dtos;
using RestSharp;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Main.Helper
{
    public class ClientHelper
    {
        private readonly RestClient _restClient;

        public ClientHelper(WorkerHostInfo worker,
            GraphDto graph)
        {
            _restClient = new RestClient(worker.Uri + "/api");
            _restClient.CookieContainer = new CookieContainer();
            var registerRequest = new RestRequest("/task", Method.POST);
            var graphJson = JsonSerializer.Serialize(graph);
            registerRequest.AddJsonBody(graphJson);
            var response = _restClient.Execute(registerRequest);
            if (!response.IsSuccessful)
            {
                throw new Exception("TODO"); //TODO
            }
        }

        public async Task<ResultDto> CalculateFor(int begin, int end)
        {
            var request = new RestRequest("/task", Method.PUT);
            request.AddJsonBody(new FindBestVertexRequestDto()
            {
                BeginVertexIndex = begin,
                EndVertexIndex = end
            });
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("TODO"); //TODO
            }

            return JsonSerializer.Deserialize<ResultDto>(response.Content);
        }

        public void Finalize()
        {
            var request = new RestRequest("/task", Method.DELETE);
            var response = _restClient.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("TODO"); //TODO
            }
        }
    }
}