using Backend.Models;

namespace Backend.Services;

public interface IStreakService {
    Streak? GetStreak(int userID);
    bool IncreaseStreak(int userID);
}
