using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public class StreakService : IStreakService {
    private readonly IRepository<Streak> _streakRepo;

    public StreakService(IRepository<Streak> repo) {
        _streakRepo = repo;
    }

    public Streak? GetStreak(int userID) {
        return _streakRepo.GetBy(s => s.UserID == userID).FirstOrDefault();
    }

    public bool IncreaseStreak(int userID) {
        Streak? streak = _streakRepo.GetBy(s => s.UserID == userID).FirstOrDefault();
        
        // If no streak is registered yet, create it and set to 1
        if (streak is null) {
            _streakRepo.Add(new Streak(userID, 1));
            _streakRepo.SaveChanges();
            return true;
        }
        
        // Else increase streak
        streak.Count++;
        _streakRepo.Update(streak);
        _streakRepo.SaveChanges();
        return true;
    }
}
