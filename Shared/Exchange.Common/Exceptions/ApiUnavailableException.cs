using System;

namespace Exchange.Common.Exceptions;

public class ApiUnavailableException : Exception
{
    public string Code { get; set; } = default!;

    public ApiUnavailableException(string code, string message) : base(message)
    {
        Code = code;
    }
}
