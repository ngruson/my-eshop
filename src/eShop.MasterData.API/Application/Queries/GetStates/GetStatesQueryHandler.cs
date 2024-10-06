using Ardalis.Result;
using eShop.MasterData.Contracts;
using MediatR;

namespace eShop.MasterData.API.Application.Queries.GetStates;

public class GetStatesQueryHandler : IRequestHandler<GetStatesQuery, Result<List<StateDto>>>
{
    public Task<Result<List<StateDto>>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
    {
        List<StateDto> states = StateEnum.List
            .Select(c => new StateDto(c.Value, c.Name))
            .ToList();

        return Task.FromResult(Result<List<StateDto>>.Success(states));
    }
}
