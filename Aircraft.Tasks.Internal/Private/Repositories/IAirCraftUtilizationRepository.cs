using Aircraft.Tasks.Internal.Private.Repositories.DbModels;

namespace Aircraft.Tasks.Internal.Private.Repositories
{
    internal interface IAirCraftUtilizationRepository
    {
        public AirCraftUtilization GetAirCraftUtilization(int id);
    }
}
