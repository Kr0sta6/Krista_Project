using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krista_Project.Models
{
    public class Booking
    {
        [Key]
        public int Booking_Id { get; set; }

        public string? Facility_Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime Booking_Date_From { get; set; }

        [Column(TypeName = "date")]
        public DateTime Booking_Date_To { get; set; }

        public string? Booked_By { get; set; }

        public string? Booking_Status { get; set; }
    }

    public class FacilityDTO
    { 
        public string Facility_Description { get; set; }

        public string Booking_Date_From { get; set; }

        public string Booking_Date_To { get; set; }

        public string Booked_By { get; set; }

        public string Booking_Status { get; set; }
    }

    public class BookingDTO
    {
        public int BookingID { get; set; }

        public string Facility_Description { get; set; }

        public string Booking_Date_From { get; set; }

        public string Booking_Date_To { get; set; }

        public string Booked_By { get; set; }

        public string Booked_Status { get; set; }
    }
}
