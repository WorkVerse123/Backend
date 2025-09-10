namespace WorkVerseAPI.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } // Thông điệp (ví dụ: "Lấy dữ liệu thành công")
        public T Data { get; set; } = default!;       // Dữ liệu trả về (nếu có)

        public ApiResponse(string message, T data, int statusCode = 200)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public ApiResponse(string message, int statusCode = 400)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
