using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public interface IEventService
{
  IEnumerable<Event> GetAll();
  IEnumerable<Event> GetBy(Expression<Func<Event, bool>> predicate);
  Event? GetById(int id);

  IEnumerable<Event> GetByTitle(string title);

  IEnumerable<Event> GetByDate(DateTime date);

  Event Create(NewEventRequest req);

  Event? Update(int id, UpdateEventRequest req);
  bool Delete(int id);

  IEnumerable<Event> GetByDay(DateOnly date);

}

