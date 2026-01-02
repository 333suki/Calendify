using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public class ProfileService : IProfileService
{
    private readonly IRepository<User> _repo;

    public ProfileService(IRepository<User> repository)
    {
        _repo = repository;
    }

    public bool UsernameExists(string username)
    {
        return _repo.GetBy(u => u.Username == username)
                    .Any();
    }


    public User? GetProfile(int userId)
    {
        return _repo
            .GetBy(u => u.ID == userId)
            .FirstOrDefault();
    }

    public User UpdateProfile(int userID, UpdateProfileRequest req)
    {
        var user = _repo
            .GetBy(u => u.ID == userID)
            .FirstOrDefault();

        if (user is null)
            return null;

        if (req.Username is not null)
        {
            bool usernameTaken = _repo.GetBy(u => u.Username == req.Username && u.ID != userID)
                                        .Any();

            if (usernameTaken)
                return null;

            user.Username = req.Username;
        }

        if (req.Password is not null)
        {
            user.Password = HashUtils.Sha256Hash(req.Password);
        }

        _repo.SaveChanges();
        return user;
    }




}
