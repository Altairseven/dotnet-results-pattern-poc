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
        });

        group.MapGet("/cmd/invalid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommand("Value1", ""), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        });

        group.MapGet("/cmd_void/valid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommandVoid("Value1", "Value2"), ct);

            return res.Match(
                onSuccess: () => Results.NoContent(),
                onFailure: error => Results.BadRequest(error)
            );
        });

        group.MapGet("/cmd_void/invalid", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommandVoid("Value1", ""), ct);

            return res.Match(
                onSuccess: () => Results.NoContent(),
                onFailure: error => Results.BadRequest(error)
            );
        });

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
        });

        group.MapGet("/cmd/ex", async ([FromServices] ISender _sender, CancellationToken ct) =>
        {
            var res = await _sender.Send(new PocCommandWithException("Value1", "Value2"), ct);

            return res.Match(
                onSuccess: value => Results.Ok(value),
                onFailure: error => Results.BadRequest(error)
            );
        });
    }
}
