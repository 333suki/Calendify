using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public interface IRoomService
{
    IEnumerable<Room> GetRooms();
    bool CreateRoom(string req);
    bool DeleteRoom(int id);
    bool UpdateRoom(int id, string name);

}
