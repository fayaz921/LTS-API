using LTS.API.Domain.Enums;
using System.Net;

namespace LTS.API.Common.Response
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode Status { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success", HttpStatusCode status = HttpStatusCode.OK) =>
       new() { Data = data, IsSuccess = true, Status = HttpStatusCode.OK, Message = message };
        public static ApiResponse<T> Ok(string message , HttpStatusCode status = HttpStatusCode.OK) => new() { Data = default!, IsSuccess = true, Message = message, Status = status };

        public static ApiResponse<T> Created(T data, string message = "Created successfully") =>
            new() { Data = data, IsSuccess = true, Status = HttpStatusCode.Created, Message = message };

        public static ApiResponse<T> Fail(string message, HttpStatusCode status = HttpStatusCode.BadRequest) =>
            new() { Data = default, IsSuccess = false, Status = status, Message = message };

        public static ApiResponse<T> NotFound(string message = "Not Found") =>
            new() { Data = default,IsSuccess = false,Status = HttpStatusCode.NotFound,Message = message};
    }
}
