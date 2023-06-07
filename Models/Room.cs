namespace CETHotelProject_CY.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public RoomType Type { get; set; }

        public double DayPrice { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
    }

    public enum RoomType
    {
        Any = -1,
        Standart,
        Family,
        Deluxe,
        Suite
    }
}
