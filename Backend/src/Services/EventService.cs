using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public class EventService : IEventService
{
    private readonly IRepository<Event> _repo;

    public EventService(IRepository<Event> repository)
    {
        _repo = repository;
    }

    public Event Create(NewEventRequest req)
    {
        Event Event = new Event(req.Title!, req.Description!, req.Date);
        _repo.Add(Event);
        _repo.SaveChanges();
        return Event;  
    }

    public bool Delete(int id)
    {
        Event? x = _repo.GetById(id);
        if (x is null)
        {
            return false;
        }

        _repo.Delete(x);
        _repo.SaveChanges();
        return true;
    }

    public IEnumerable<Event> GetAll()
    {
        return _repo.GetAll();
    }

    public Event? GetById(int id)
    {
        return _repo.GetById(id);
    }

    public IEnumerable<Event> GetByTitle(string title)
    {
        return _repo.GetBy(e => e.Title.Contains(title));
    }

    public IEnumerable<Event> GetByDate(DateTime date)
    {
        return _repo.GetBy(e => e.Date == date);
    }

    public IEnumerable<Event> GetBy(Expression<Func<Event, bool>> predicate)
    {
        return _repo.GetBy(predicate);
    }

    public Event? Update(int id, UpdateEventRequest req)
    {
        Event? x = _repo.GetById(id);
        if (x is null)
        {
            return null;
        }

        x.Title = req.Title;
        x.Description = x.Description;
        x.Date = x.Date;

        _repo.Update(x);
        _repo.SaveChanges();

        return x;
    }
}