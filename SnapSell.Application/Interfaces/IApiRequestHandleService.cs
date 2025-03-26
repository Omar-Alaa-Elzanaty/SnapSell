using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Interfaces
{
    public interface IApiRequestHandleService
    {
        public HttpClient HttpClient { get; set; }
        Task<Result<T>> GetAsync<T>(string url);
        Task<Result<T>> SendAsync<T>(string url, HttpContent? httpContent);
    }
}
