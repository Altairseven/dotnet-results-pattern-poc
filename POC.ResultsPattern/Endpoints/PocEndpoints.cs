using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.ResultsPattern.Extensions;
using POC.ResultsPattern.Features;
using POC.ResultsPattern.Features.PocCommandWithResult;

namespace POC.ResultsPattern.Endpoints;

public static class PocEndpoints
{
    public static void MapPocEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/cmd/valid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommand("Value1", "Value2"), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("Simulates a valid command that produces a response");

        group.MapGet("/cmd/invalid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommand("Value1", ""), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("Simulates a invalid command with missing parameters that would otherwise produce a Response");

        group.MapGet("/cmd/business_reason", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommand("Value3", "ValueX"), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("Simulates a valid command with proper parameters that fails for other business reason");

        group.MapGet("/cmd_void/valid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommandVoid("Value1", "Value2"), ct);

            return res.Match(
                onSuccess: () => Results.NoContent(),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("Simulates a valid command that does not produce a response");

        group.MapGet("/cmd_void/invalid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommandVoid("Value1", ""), ct);

            return res.Match(
                onSuccess: () => Results.NoContent(),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("Simulates a invalid command with missing parameters that would not produce a Response");

        group.MapGet("/query", async (
            [FromServices] ISender _sender, CancellationToken ct,
            [FromQuery] string propertyOne = default!, 
            [FromQuery] string propertyTwo = default!) =>
        {
            var res = await _sender.Send(new PocQuery(propertyOne, propertyTwo), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("simulates a query (pass a value to both parameters for success)");

        group.MapGet("/cmd/ex", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommandWithException("Value1", "Value2"), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        }).WithDescription("Simulates a valid command that encounter an unexpected exception (eg: database down, or remote api down"); ;
    }
}
