using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public class RoomService : IRoomService
{
    private readonly IRepository<Room> _repo;

    public RoomService(IRepository<Room> repository)
    {
        _repo = repository;
    }

    public IEnumerable<Room> GetRooms()
    {
        return _repo.GetAll();
    }

    public bool CreateRoom(string name)
    {
        Room? exists = _repo.GetBy(u => u.Name == name).FirstOrDefault();

        if (exists is not null)
        {
            return false;
        }

        var room = new Room(name);
        _repo.Add(room);
        _repo.SaveChanges();
        return true;
    }

    public bool DeleteRoom(int id)
    {
        var room = _repo.GetById(id);
        if (room is null)
            return false;

        _repo.Delete(room);
        _repo.SaveChanges();
        return true;
    
    }

    public bool UpdateRoom(int id, string name)
    {

        var room = _repo.GetById(id);
        if (room is null)
            return false;

        room.Name = name;
        _repo.SaveChanges();
        return true;
    }
}
