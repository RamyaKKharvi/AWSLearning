namespace ASP_AWSTest.Enums;

public class ReturnCodeHelper
{
    public static string GetMessage(ReturnCode code)
    {
        return code switch
        {
            ReturnCode.SUCCESS => "Operation successful",
            ReturnCode.UNKNOWN_ERROR => "An unknown error occurred.",
            ReturnCode.INVALID_REQUEST => "The request is invalid. Please verify your request and try again.",
            ReturnCode.INTERNAL_SERVER_ERROR => "Internal Server Error. ",
            ReturnCode.NOT_FOUND => "Data not found. Please verify your request and try again.",
            ReturnCode.UNAUTHORIZED => "Unauthorized access. Please verify your credentials.",
            ReturnCode.MISSING_REQUIRED_PARAMETER => "A required parameter is missing. Please check your request.",
            ReturnCode.MISSING_REQUIRED_BODY => "The request body is required but was not provided.",
            ReturnCode.BAD_REQUEST => "Bad request. Please verify your input and try again.",
        };
    }
}
