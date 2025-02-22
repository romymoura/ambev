using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using OneOf.Types;
using System;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class InterceptExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public InterceptExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch(InvalidOperationException iEx)
            {
                await HandleInvalidOperationExceptionAsync(context, iEx);
            }
            catch(KeyNotFoundException knfEx)
            {
                await HandleKeyNotFoundExceptionAsync(context, knfEx);
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

        private static Task HandleExceptionAsync(HttpContext context, bool success = false, string message = "Validation Failed", 
            ValidationException? validationException = null,
            InvalidOperationException? invalidOperationException = null,
            KeyNotFoundException? keyNotFoundException = null)
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

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
