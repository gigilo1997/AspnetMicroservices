﻿using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

internal class ValidationException : ApplicationException
{
    public ValidationException() : base("One or more validation failures have occured.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public Dictionary<string, string[]> Errors { get; private set; }
}
