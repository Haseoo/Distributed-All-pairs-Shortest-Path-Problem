using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using com.Github.Haseoo.DASPP.Main.Exceptions.Workers;
using RestSharp;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Main.Helper
{
    public class ClientHelper
    {
        private readonly RestClient _restClient;
        private readonly WorkerHostInfo _workerHost;

        public ClientHelper(WorkerHostInfo worker,
            GraphDto graph)
        {
            _restClient = new RestClient(worker.Uri + "/api")
            {
                CookieContainer = new CookieContainer()
            };
            _workerHost = worker;

            var registerRequest = new RestRequest("/task", Method.POST);
            var graphJson = JsonSerializer.Serialize(graph);
            registerRequest.AddJsonBody(graphJson);
            var response = _restClient.Execute(registerRequest);
            if (!response.IsSuccessful)
            {
                ThrowExceptionOnNotSuccessfulResponse(response);
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
                ThrowExceptionOnNotSuccessfulResponse(response);
            }

            return JsonSerializer.Deserialize<ResultDto>(response.Content);
        }

        public void FinalizeSession()
        {
            var request = new RestRequest("/task", Method.DELETE);
            var response = _restClient.Execute(request);
            if (!response.IsSuccessful)
            {
                ThrowExceptionOnNotSuccessfulResponse(response);
            }
        }

        private void ThrowExceptionOnNotSuccessfulResponse(IRestResponse response)
        {
            throw (response.ResponseStatus == ResponseStatus.Completed)
                ? (InternalServerErrorException)new WorkerBadResponseException((int)response.StatusCode,
                    response.ErrorMessage)
                : new WorkerNotRespondingException(_workerHost.Uri);
        }
    }
}