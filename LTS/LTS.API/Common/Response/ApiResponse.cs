using LTS.API.Domain.Enums;

namespace LTS.API.Common.Response
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public ResponseType Status { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success") =>
       new() { Data = data, IsSuccess = true, Status = ResponseType.Ok, Message = message };

        public static ApiResponse<T> Created(T data, string message = "Created successfully") =>
            new() { Data = data, IsSuccess = true, Status = ResponseType.Created, Message = message };

        public static ApiResponse<T> Fail(string message, ResponseType status = ResponseType.BadRequest) =>
            new() { Data = default, IsSuccess = false, Status = status, Message = message };
    }
}
