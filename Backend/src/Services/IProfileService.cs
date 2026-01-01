using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public interface IProfileService
{
    public bool UsernameExists(string username);

  User? GetProfile(int id);

  User UpdateProfile(int userID, UpdateProfileRequest updateRequest);


}

