using Ardalis.Result;
using eShop.MasterData.Contracts;
using MediatR;

namespace eShop.MasterData.API.Application.Queries.GetStates;

public class GetStatesQuery : IRequest<Result<List<StateDto>>>
{
}
