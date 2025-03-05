using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OneOf.Types;
using System;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class InterceptExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InterceptExceptionMiddleware> _logger;

        public InterceptExceptionMiddleware(RequestDelegate next, ILogger<InterceptExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Ocorreu um erro no ValidationException.");
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (InvalidOperationException iEx)
            {
                _logger.LogError(iEx, "Ocorreu um erro no InvalidOperationException.");
                await HandleInvalidOperationExceptionAsync(context, iEx);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogError(knfEx, "Ocorreu um erro no KeyNotFoundException.");
                await HandleKeyNotFoundExceptionAsync(context, knfEx);
            }
            catch (Exception exCore)
            {
                _logger.LogError(exCore, "Ocorreu um erro no Exception exCore.");
                await HandleKeyNotFoundExceptionAsync(context, exCore);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            return HandleExceptionAsync(context, validationException: exception);
        }
        private static Task HandleInvalidOperationExceptionAsync(HttpContext context, InvalidOperationException exception)
        {
            return HandleExceptionAsync(context, message: "Operation Failed", invalidOperationException: exception);
        }
        private static Task HandleKeyNotFoundExceptionAsync(HttpContext context, KeyNotFoundException exception)
        {
            return HandleExceptionAsync(context, message: "Key Found Failed", keyNotFoundException: exception);
        }
        private static Task HandleKeyNotFoundExceptionAsync(HttpContext context, Exception exception)
        {
            return HandleExceptionAsync(context, message: "Key Found Failed", exception: exception);
        }

        private static Task HandleExceptionAsync(HttpContext context, bool success = false, string message = "Validation Failed",
            ValidationException? validationException = null,
            InvalidOperationException? invalidOperationException = null,
            KeyNotFoundException? keyNotFoundException = null,
            Exception exception = null
            )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            IEnumerable<ValidationErrorDetail> listError = new List<ValidationErrorDetail>();
            if (validationException != null)
                listError = validationException.Errors.Select(error => (ValidationErrorDetail)error);

            if (invalidOperationException != null)
                listError = new List<ValidationErrorDetail> { new ValidationErrorDetail() { Error = "OperationValidator", Detail = invalidOperationException.Message } };

            if (keyNotFoundException != null)
                listError = new List<ValidationErrorDetail> { new ValidationErrorDetail() { Error = "KeyFoundValidator", Detail = keyNotFoundException.Message } };

            if (exception != null)
                listError = new List<ValidationErrorDetail> { new ValidationErrorDetail() { Error = "Error unexpected", Detail = exception.Message } };

            var response = new ApiResponse
            {
                Success = success,
                Message = message,
                Errors = listError
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));

            Console.WriteLine($"Result erro: {result}");

            return result;
        }
    }
}
