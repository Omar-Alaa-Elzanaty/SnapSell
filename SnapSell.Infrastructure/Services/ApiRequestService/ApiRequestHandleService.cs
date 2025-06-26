using MongoDB.Bson.IO;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SnapSell.Application.Abstractions.Interfaces;

namespace SnapSell.Infrastructure.Services.ApiRequestService
{
    public class ApiRequestHandleService : IApiRequestHandleService
    {
        public HttpClient HttpClient { get; set; }

        public ApiRequestHandleService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient();
        }

        public async Task<Result<T>> GetAsync<T>(string url)
        {
            var response = await HttpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return Result<T>.Failure(content);
            }

            var result = JsonSerializer.Deserialize<T>(content);

            return Result<T>.Success(result!);
        }

        public async Task<Result<T>> SendAsync<T>(string url,HttpContent? httpContent)
        {
            var response = await HttpClient.PostAsync(url, httpContent);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                //TODO: log error

                return Result<T>.Failure(content);
            }

            var result = JsonSerializer.Deserialize<T>(content);

            return Result<T>.Success(result!);
        }
    }
}
