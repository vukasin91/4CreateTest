using Application.Employees.Commands.CreateEmployee;
using Application.Employees.Commands.DeleteEmployee;
using Application.Employees.Commands.UpdateEmployee;
using Application.Employees.Queries.GetEmployee;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

public static class EmployeeModule
{
    public static void AddEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("employees", async (CreateEmployeeCommand command, ISender sender) =>
        {
            await sender.Send(command);

            return Results.Ok();
        });

        app.MapGet("employees/{id:int}", async (int id, ISender sender) =>
        {
            try
            {
                return Results.Ok(await sender.Send(new GetEmployeeByIdCommand(id)));
            }
            catch (EmployeeNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });

        app.MapPut("employees/{id:int}", async (int id, [FromBody]UpdateEmployeeCompanyRequest request, ISender sender) =>
        {
            try
            {
                var command = new UpdateEmployeeCompanyCommand(id, request.CompanyIds);
                await sender.Send(command);

                return Results.NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });

        app.MapDelete("employees/{id:int}", async (int id, ISender sender) =>
        {
            try
            {
                await sender.Send(new DeleteEmployeeCommand(id));

                return Results.NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });
    }
}