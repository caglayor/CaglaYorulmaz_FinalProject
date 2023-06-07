namespace CETHotelProject_CY.Models.ViewModels
{
    public class ReserveRoomViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double? TotalPrice { get; set; }
        public double TotalDays { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
