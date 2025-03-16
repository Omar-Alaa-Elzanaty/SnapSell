using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.ResultDtos
{
    public class Result<T>
    {


        public static Result<T> Success()
        {
            return new();
        }

        public static Result<T> Success(T data)
        {
            return new();
        }

        public static Result<T> Success(T data,string message)
        {
            return new();
        }

        public static Result<T> Failure(string message)
        {
            return new();
        }
    }
}
