namespace RestaurantOrdering.Api.Services;

public enum ServiceErrorType
{
    None,
    BadRequest,
    NotFound
}

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public ServiceErrorType ErrorType { get; set; }

    public static ServiceResult<T> Ok(T data)
    {

        return new ServiceResult<T>
        {
            Success = true,
            Data = data,
            ErrorType = ServiceErrorType.None
        };
    }

    public static ServiceResult<T> BadRequest(string errorMessage)
    {
        return new ServiceResult<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorType = ServiceErrorType.BadRequest
        };
    }

    public static ServiceResult<T> NotFound(string errorMessage)
    {
        return new ServiceResult<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorType = ServiceErrorType.NotFound
        };
    }
}