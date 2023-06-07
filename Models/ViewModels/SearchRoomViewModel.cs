using System.ComponentModel.DataAnnotations;

namespace CETHotelProject_CY.Models.ViewModels
{
    public class SearchRoomViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }
        public RoomType roomType { get; set; }
        public int MaximumGuests { get; set; }
        public IList<Room> Room { get; set;} = new List<Room>();
    }
}
