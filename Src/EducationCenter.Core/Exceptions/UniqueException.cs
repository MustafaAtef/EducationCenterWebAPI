using System;

namespace EducationCenter.Core.Exceptions;

public class UniqueException : Exception
{
    public UniqueException(string message) : base(message)
    {
    }

    public UniqueException(string message, Exception innerException) : base(message, innerException)
    {
    }
}