using Application.Companies.Commands.CreateCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Queries.GetCompany;
using Application.Employees.Commands.DeleteEmployee;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

public static class CompanyModule
{
    public static void AddCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("companies", async (CreateCompanyCommand command, ISender sender) =>
        {
            await sender.Send(command);

            return Results.Ok();
        });

        app.MapGet("companies/{id:int}", async (int id, ISender sender) =>
        {
            try
            {
                var command = await sender.Send(new GetCompanyByIdCommand(id));
                return Results.Ok(command);
            }
            catch (CompanyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });

        app.MapPut("companies/{id:int}", async (int id, [FromBody] UpdateCompanyRequest request, ISender sender) =>
        {
            try
            {
                var command = new UpdateCompanyCommand(id, request.Employees);
                await sender.Send(command);

                return Results.NoContent();
            }
            catch (CompanyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });

        app.MapDelete("companies/{id:int}", async (int id, ISender sender) =>
        {
            try
            {
                await sender.Send(new DeleteEmployeeCommand(id));

                return Results.NoContent();
            }
            catch (CompanyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });
    }
}