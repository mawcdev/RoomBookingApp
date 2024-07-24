using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // Arrange
            var date = new DateTime(2024, 07, 26);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { RoomId = 1, Date = date, FullName = "Booking 1", Email = "booking1@room.com" });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1), FullName = "Booking 2", Email = "booking2@room.com" });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // Act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // Assert
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, q => q.Id == 2);
            Assert.Contains(availableRooms, q => q.Id == 3);
            Assert.DoesNotContain(availableRooms, q => q.Id == 1);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("ShouldSaveTest")
                .Options;

            var roomBooking = new RoomBooking { 
                RoomId = 1, 
                Date = new DateTime(2024, 07, 25), 
                Email = "booking3@room.com", 
                FullName = "Booking 3" 
            };

            using var context = new RoomBookingAppDbContext(dbOptions);

            var roomBookingService = new RoomBookingService(context);
            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
            Assert.Equal(roomBooking.Email, booking.Email);
            Assert.Equal(roomBooking.FullName, booking.FullName);
        }
    }
}