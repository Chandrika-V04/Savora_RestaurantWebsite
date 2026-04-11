using System;
namespace Restuarant.Models{
    public class Reservation{
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int NoOfPeople { get; set; }
        public string Location { get; set; }
        public string SeatingPreference { get; set; }
        public string EventType { get; set; }
        public string DecorTheme { get; set; }
        public string FoodPreference { get; set; }
        public string Status { get; set; }
    }
}