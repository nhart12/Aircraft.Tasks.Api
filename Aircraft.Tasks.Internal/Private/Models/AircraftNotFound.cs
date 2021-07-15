namespace Aircraft.Tasks.Internal.Private.Models
{
    public class AircraftNotFound
    {
        public AircraftNotFound(int aircraftId)
        {
            AircraftId = aircraftId;
        }
        public int AircraftId { get; set; }
    }
}
