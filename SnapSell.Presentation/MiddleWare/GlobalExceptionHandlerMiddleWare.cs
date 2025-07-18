﻿using FluentValidation.Results;
using Mapster;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Common.Exceptions;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Extnesions;
using System.Net;
using System.Text.Json;

namespace SnapSell.Presentation.MiddleWare;

public sealed class GlobalExceptionHandlerMiddleWare(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseException ex)
        {
            await HandleBaseExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }

    }

    private static async Task HandleBaseExceptionAsync(HttpContext context, BaseException ex)
    {

        var response = new Result<object>
        {
            Message = ex.Message,
            StatusCode = (HttpStatusCode)context.Response.StatusCode,
            Errors = ValidationExtension.GetErrorsDictionary(ex.Errors.Adapt<List<ValidationFailure>>())
        };

        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var exceptionResult = JsonSerializer.Serialize(response, jsonOptions);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(exceptionResult);
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {

        var response = new Result<object>
        {
            Message = ex.Message,
            StatusCode = HttpStatusCode.InternalServerError,
        };

        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var exceptionResult = JsonSerializer.Serialize(response, jsonOptions);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(exceptionResult);
    }
}