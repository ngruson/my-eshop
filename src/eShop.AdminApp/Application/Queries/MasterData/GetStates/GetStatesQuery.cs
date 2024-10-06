using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.MasterData.GetStates;

public class GetStatesQuery : IRequest<Result<StateViewModel[]>>
{
}
