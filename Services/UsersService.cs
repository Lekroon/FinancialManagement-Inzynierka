using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class UsersService : IUsersService
{
    private readonly List<User> _listOfUsers;

    public UsersService()
    {
        _listOfUsers = new List<User>();
    }
    
    public UserResponse AddUser(UserAddRequest? userAddRequest)
    {
        // Validation: userAddRequest parameter cannot be null
        if (userAddRequest == null)
        {
            throw new ArgumentNullException(nameof(userAddRequest));
        }
        
        // Model validations
        ValidationHelper.ModelValidation(userAddRequest);
        
        // Convert object from UserAddRequest to User type
        var user = userAddRequest.ToUser();

        // Validation: user Login, Email and PhoneNumber cannot be duplicated
        if (_listOfUsers.Any(userInList => userInList.Login == user.Login))
        {
            throw new ArgumentException("User login already exists");
        }
        if (_listOfUsers.Any(userInList => userInList.Email == user.Email))
        {
            throw new ArgumentException("User email already exists");
        }
        if (_listOfUsers.Any(userInList => userInList.PhoneNumber == user.PhoneNumber && 
                                           !string.IsNullOrEmpty(userInList.PhoneNumber)))
        {
            throw new ArgumentException("User phone number already exists");
        }
        
        // Generating new id for user
        user.UserId = Guid.NewGuid();
        
        _listOfUsers.Add(user);

        return user.ToUserResponse();
    }

    public List<UserResponse> GetAllUsers()
    {
        return _listOfUsers.Select(user => user.ToUserResponse()).ToList();
    }

    public UserResponse? GetUserByUserId(Guid? userId)
    {
        return _listOfUsers.FirstOrDefault(user => user.UserId == userId)?.ToUserResponse();
    }

    public UserResponse? GetUserByUserLoginOrEmail(string? loginOrEmail)
    {
        return _listOfUsers.FirstOrDefault(user => user.Login == loginOrEmail || 
                                                   user.Email == loginOrEmail)?.ToUserResponse();
    }
}