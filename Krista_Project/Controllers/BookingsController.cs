using Krista_Project.Data;
using Krista_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace Krista_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        { 
            var bookings = _context.Bookings.FirstOrDefault(e => e.Booking_Id == id);
            if (bookings == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            return Ok(bookings);
        }

        [HttpGet("Dateby")]
        public IActionResult GetByData(string Dateby)
        {
            string decodeDate = WebUtility.UrlDecode(Dateby);
            if (!DateTime.TryParseExact(

                decodeDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest("Invalid data format. Use the formate d/M/yyyy(e.g 14/06/2020");
            }
            var Booking = _context.Bookings.Where(e => e.Booking_Date_From.Date == parsedDate)
                .Select(f => new
                {
                    f.Booking_Id,
                    f.Facility_Description,
                    BookingDateFrom = f.Booking_Date_From.ToString("d/M/yyyy"),
                    BookingDateTo = f.Booking_Date_To.ToString("d/M/yyyy"),
                    f.Booked_By,
                    f.Booking_Status,
                }).ToList();

            if (Booking.Count == 0)
            {
                return Problem(detail: "Booking Date: " + parsedDate.ToString("d/M/yyyy") + " is not found.", statusCode: 404);
            }
            return Ok(Booking);
        }

        [HttpPost]
        public IActionResult Post(FacilityDTO facilityDTO)
        {
            if (!DateTime.TryParseExact(
                facilityDTO.Booking_Date_From, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
            {
                return BadRequest("Invalid date format. Use the format d/M/yyyy (e.g.30/11/2005).");
            }
            if (!DateTime.TryParseExact(
                facilityDTO.Booking_Date_To, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
            {
                return BadRequest("Invalid date format. Use the format d/M/yyyy (e.g.30/11/2005).");
            }

            var facility = new Booking
            {
                Facility_Description = facilityDTO.Facility_Description,
                Booking_Date_From = fromDate,
                Booking_Date_To = toDate,
                Booked_By = facilityDTO.Booked_By,
                Booking_Status = facilityDTO.Booking_Status,
            };
            _context.Bookings.Add(facility);
            _context.SaveChanges();

            var bookingDTO = new BookingDTO
            {
                BookingID = facility.Booking_Id,
                Facility_Description = facility.Facility_Description,
                Booking_Date_From = fromDate.ToString("d/M/yyyy"),
                Booking_Date_To = toDate.ToString("d/M/yyyy"),
                Booked_By = facility.Booked_By,
                Booked_Status = facility.Booking_Status,
            };
            return CreatedAtAction("Post", new { id = facility.Booking_Id }, bookingDTO);
        }

        [HttpPut]

        public IActionResult Put(int? id, FacilityDTO facilityDTO)
        {
            var entity = _context.Bookings.FirstOrDefault(e => e.Booking_Id == id);
            if (entity == null)
                return Problem(detail: "Booking ID: " + id + " is not found.", statusCode: 404);

            entity.Facility_Description = facilityDTO.Facility_Description;

            if (DateTime.TryParseExact(facilityDTO.Booking_Date_From, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
            {
                entity.Booking_Date_From = fromDate;
            }
            else
            {
                return BadRequest("Invalid date format for Booking_Date_From. Use the format d/M/yyyy (e.g., 21/2/2023).");
            }

            if (DateTime.TryParseExact(facilityDTO.Booking_Date_To, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
            {
                entity.Booking_Date_To = toDate;
            }
            else
            {
                return BadRequest("Invalid date format for Booking_Date_To. Use the format d/M/yyyy (e.g., 21/2/2023).");
            }

            entity.Booked_By = facilityDTO.Booked_By;
            entity.Booking_Status = facilityDTO.Booking_Status;

            _context.SaveChanges();

            var facilityResponse = new FacilityDTO
            {
                Facility_Description = entity.Facility_Description,
                Booking_Date_From = entity.Booking_Date_From.ToString("d/M/yyyy"),
                Booking_Date_To = entity.Booking_Date_To.ToString("d/M/yyyy"),
                Booked_By = entity.Booked_By,
                Booking_Status = entity.Booking_Status
            };

            return Ok(facilityResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.Bookings.FirstOrDefault(e => e.Booking_Id == id);
            if (entity == null)
                return Problem(detail: "Booking Id: " + id + " is not found.", statusCode: 400);
            _context.Bookings.Remove(entity);
            _context.SaveChanges();
            var bookingDTO = new BookingDTO
            {
                BookingID = entity.Booking_Id,
                Facility_Description = entity.Facility_Description,
                Booking_Date_From = entity.Booking_Date_From.ToString("d/M/yyyy"),
                Booking_Date_To = entity.Booking_Date_To.ToString("d/M/yyyy"),
                Booked_By = entity.Booked_By,
                Booked_Status = entity.Booking_Status
            };
            return Ok(bookingDTO);
        }
    }
}
