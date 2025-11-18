using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Room {
    public Room(string name) {
        this.Name = name;
    }
    
    [Key]
    [Column(Order=0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public string Name { get; set; }
    
    public ICollection<RoomBooking> RoomBookings { get; set; } = new List<RoomBooking>();
}
