﻿namespace TutorHub.BusinessLogic.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base()
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
