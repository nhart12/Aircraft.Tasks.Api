using System.Collections.Concurrent;
using System.Collections.Generic;
using Aircraft.Tasks.Internal.Private.Repositories.DbModels;

namespace Aircraft.Tasks.Internal.Private.Repositories
{
    internal class AirCraftUtilizationRepository: IAirCraftUtilizationRepository
    {
        private readonly ConcurrentDictionary<int, AirCraftUtilization> mockAirCraft;
        public AirCraftUtilizationRepository()
        {
            mockAirCraft = new ConcurrentDictionary<int, AirCraftUtilization>(new Dictionary<int, AirCraftUtilization>()
            {
                {1, new AirCraftUtilization()
                {
                    CurrentHours = 550,
                    DailyHours = 0.7
                } },
                {2, new AirCraftUtilization()
                {
                    CurrentHours = 200,
                    DailyHours = 1.1
                } }
            });
        }

        public AirCraftUtilization GetAirCraftUtilization(int id)
        {
            if (mockAirCraft.TryGetValue(id, out var aircraft))
            {
                return aircraft;
            }
            return null;
        }
    }
}
