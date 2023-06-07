using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CETHotelProject_CY.Data;
using CETHotelProject_CY.Models;
using Microsoft.AspNetCore.Authorization;
using CETHotelProject_CY.Models.ViewModels;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CETHotelProject_CY.Areas.User
{
    [Authorize]
    [Area("User")]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoomController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Reserve(int? id)
        {
            if (id == null || _db.Room == null)
            {
                return NotFound();
            }

            var room = await _db.Room
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            DateTime dateFrom = DateTime.ParseExact(HttpContext.Request.Query["dateIn"], "d.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime dateOut = DateTime.ParseExact(HttpContext.Request.Query["dateOut"], "d.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            double totalDays = (dateOut - dateFrom).TotalDays + 1;

            ReserveRoomViewModel vm = new ReserveRoomViewModel()
            {
                Room = room,
                DateFrom = dateFrom,
                DateTo = dateOut,
                TotalPrice = totalDays * room.DayPrice,
                TotalDays = totalDays

            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DoReservation(ReserveRoomViewModel vm)
        {
            var room = await _db.Room
                .FirstOrDefaultAsync(m => m.Id == vm.RoomId);

            var applicationUser = await _userManager.GetUserAsync(User);

            Reservation reservation = new Reservation()
            {
                UserId = _userManager.GetUserId(User),
                ApplicationUser = applicationUser,
                EndDate = vm.DateTo,
                StartDate = vm.DateFrom,
                RoomId = vm.RoomId,
                TotalDays = vm.TotalDays,
                TotalPaid = vm.TotalPrice,
                Room = room
            };
            _db.Add(reservation);
            await _db.SaveChangesAsync();

            return View(reservation);
        }

        public IActionResult Check(SearchRoomViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.DateFrom.ToString()) || string.IsNullOrEmpty(vm.DateTo.ToString()))
            {
                TempData["error"] = "The start or end date cannot be blank";
                return LocalRedirect("/");
            }

            if (vm.DateFrom >= vm.DateTo)
            {
                TempData["error"] = "The start date cannot be later than the end date.";
                return LocalRedirect("/");
            }


            var roomsBooked = from b in _db.Reservation
                              where
                              (((vm.DateFrom >= b.StartDate) && (vm.DateFrom <= b.EndDate)) ||
                              ((vm.DateTo >= b.StartDate) && (vm.DateTo <= b.StartDate)) ||
                              ((vm.DateFrom <= b.StartDate) && (vm.DateTo >= b.StartDate) && (vm.DateTo <= b.EndDate)) ||
                              ((vm.DateFrom >= b.StartDate) && (vm.DateFrom <= b.EndDate) && (vm.DateTo >= b.EndDate)) ||
                              ((vm.DateFrom <= b.StartDate) && (vm.DateTo >= b.EndDate)))
                              select b;

            var availableRooms = _db.Room.Where(room => !roomsBooked.Any(booked => booked.RoomId == room.Id) && (vm.roomType != RoomType.Any ? room.Type == vm.roomType : true) && (vm.MaximumGuests >= room.Capacity))
                .ToList();

            vm.Room = availableRooms;
            return View(vm);
        }

        private bool RoomExists(int id)
        {
          return (_db.Room?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
