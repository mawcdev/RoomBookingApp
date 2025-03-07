﻿using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext _context;
        public RoomBookingService(RoomBookingAppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Room> GetAvailableRooms(DateTime dateTime)
        {
            
            return _context.Rooms.Where(q => !q.RoomBookings.Any(x => x.Date == dateTime)).ToList();
        }

        public void Save(RoomBooking roomBooking)
        {
            _context.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
