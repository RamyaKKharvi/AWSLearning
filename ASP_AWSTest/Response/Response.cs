using System;
using ASP_AWSTest.Enums;
namespace ASP_AWSTest.Response;

public class Response<T>
{
    public bool Status { get; set; }
    public string ResponseMessage { get; set; }
    public string ResponseCode { get; set; }
    public T Data { get; set; }
    protected Response(bool status, string message, string returnCode, T data = default)
    {
        Status = status;
        ResponseMessage = message;
        ResponseCode = returnCode;
        Data = data;
    }
    public static Response<T> SuccessResult()
    {
        var returnCode = ReturnCode.SUCCESS;
        var message = ReturnCodeHelper.GetMessage(returnCode);
        return new Response<T>(true, message, ReturnCode.SUCCESS.ToString());
    }
    public static Response<T> SuccessResult(string message)
    {
        return new Response<T>(true, message, "SUCCESS");
    }
    public static Response<T> SuccessResult(T data)
    {
        var returnCode = ReturnCode.SUCCESS;
        var message = ReturnCodeHelper.GetMessage(returnCode);
        return new Response<T>(true, message, ReturnCode.SUCCESS.ToString(), data);
    }
    public static Response<T> SuccessResult(T data, string message)
    {
        return new Response<T>(true, message, ReturnCode.SUCCESS.ToString(), data);
    }

    public static Response<T> SuccessResult(T data, string message = null, string code = null)
    {
        return new Response<T>(true, message, code, data);
    }
    public static Response<T> FailureResult(string message)
    {
        return new Response<T>(false, message, ReturnCode.BAD_REQUEST.ToString(), default);
    }
    public static Response<T> FailureResult(string message, T data)
    {
        return new Response<T>(false, message, ReturnCode.BAD_REQUEST.ToString(), data);
    }
    public static Response<T> FailureResult(ReturnCode returnCode)
    {
        var message = ReturnCodeHelper.GetMessage(returnCode);
        return new Response<T>(false, message, returnCode.ToString(), default);
    }
    public static Response<T> FailureResult(string message, ReturnCode returnCode)
    {
        return new Response<T>(false, message, returnCode.ToString(), default);
    }
    public static Response<T> FailureResult(string message, ReturnCode returnCode, T data)
    {
        return new Response<T>(false, message, returnCode.ToString(), data);
    }

    public static Response<T> FailureResult(string message = null, string code = null)
    {
        return new Response<T>(false, message, code);
    }
    public static Response<T> FailureResult(T data, string message = null, string code = null)
    {
        return new Response<T>(false, message, code, data);
    }

    public static Response<T> ExceptionFailureResult(string message, ReturnCode returnCode)
    {
        return new Response<T>(false, message, returnCode.ToString(), default);
    }
    public static Response<T> ExceptionFailureResult(ReturnCode returnCode)
    {
        var message = ReturnCodeHelper.GetMessage(returnCode);
        return new Response<T>(false, message, returnCode.ToString(), default);
    }
    public static Response<T> ExceptionFailureResult(string message)
    {
        return new Response<T>(false, message, ReturnCode.UNKNOWN_ERROR.ToString(), default);
    }
}
