using eShop.MasterData.Contracts;

namespace eShop.AdminApp.Application.Queries.MasterData.GetStates;

internal static class MapperExtensions
{
    public static StateViewModel[] MapToStateViewModels(this StateDto[] states)
    {
        return states
            .Select(c => new StateViewModel(c.Code, c.Name))
            .ToArray();
    }
}
