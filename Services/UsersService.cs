using Entities;
using ServiceContracts;
using ServiceContracts.DTO.User;
using Services.Helpers;

namespace Services;

public class UsersService : IUsersService
{
    private readonly List<User> _listOfUsers;
    private readonly ICountriesService _countriesService;

    public UsersService(bool initialize = true)
    {
        _listOfUsers = new List<User>();
        _countriesService = new CountriesService();

        if (initialize)
        {
            MockData(); 
        }
    }
    
    private UserResponse ConvertUserToUserResponse(User user)
    {
        var userResponse = user.ToUserResponse();
        
        userResponse.CountryName = _countriesService.GetCountryByCountryId(user.CountryId)?.CountryName;

        return userResponse;
    }

    private void MockData()
    {
        _listOfUsers.AddRange(new List<User>
        {
            new()
            {
                UserId = Guid.Parse("144454D3-19D6-4081-8C21-2FCE41723461"),
                Login = "otomasello0",
                Password = "rT4\\q|`1a",
                Email = "jvauter0@topsy.com",
                PhoneNumber = "668-418-2014",
                IsActive = true,
                CountryId = Guid.Parse("83CE3D28-B429-41C4-9900-80F881DF6D28")
            },
            new()
            {
                UserId = Guid.Parse("7FC64B0D-089A-4016-BF62-B20B9E4B6531"),
                Login = "mstyle1",
                Password = "rP9,Nn1Ka/B",
                Email = "smcqueen1@360.cn",
                PhoneNumber = "522-761-0911",
                IsActive = true,
                CountryId = Guid.Parse("35EE3318-5D8A-4767-9C1F-D127D5DE7028")
            },
            new()
            {
                UserId = Guid.Parse("82177757-4F2C-4877-80B3-5CBF4CF49E9A"),
                Login = "kzisneros2",
                Password = "iI9}!<Ek%7(I9s",
                Email = "lunger2@woothemes.com",
                PhoneNumber = "788-798-1326",
                IsActive = false,
                CountryId = Guid.Parse("83CE3D28-B429-41C4-9900-80F881DF6D28")
            },
            new()
            {
                UserId = Guid.Parse("A5282568-7CE5-4114-A1E5-3ABA38750D2E"),
                Login = "wdunniom3",
                Password = "qE2+%}5u8Z!",
                Email = "cbarstowk3@sphinn.com",
                PhoneNumber = "483-660-9337",
                IsActive = false,
                CountryId = Guid.Parse("191F892C-EB4A-4E7B-A144-C0E1EB2BB8BE")
            },
            new()
            {
                UserId = Guid.Parse("DFE23AA9-7C09-4B3A-BD10-87D38FDD809B"),
                Login = "jsellstrom4",
                Password = "dM6!l=iwe",
                Email = "vlease4@hc360.com",
                PhoneNumber = "387-877-4751",
                IsActive = true,
                CountryId = Guid.Parse("35EE3318-5D8A-4767-9C1F-D127D5DE7028")
            },
            new()
            {
                UserId = Guid.Parse("71B3FCB5-1C86-418E-93BD-E6148153A130"),
                Login = "smacklin6",
                Password = "zE2=PW_~KN2",
                Email = "ctipper6@uol.com.br",
                PhoneNumber = "849-282-2461",
                IsActive = true,
                CountryId = Guid.Parse("6D4D8A43-A6EE-458B-B5A1-31A0A5699684")
            },
            new()
            {
                UserId = Guid.Parse("22DAA840-FCCC-4D3C-9240-7DFCE98EB05A"),
                Login = "tshadfourth9",
                Password = "mD6(J*4bR1J",
                Email = "bvsanelli9@huffingtonpost.com",
                PhoneNumber = "986-178-0357",
                IsActive = false,
                CountryId = Guid.Parse("191F892C-EB4A-4E7B-A144-C0E1EB2BB8BE")
            }
        });
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

        return ConvertUserToUserResponse(user);
    }

    public List<UserResponse> GetAllUsers()
    {
        return _listOfUsers.Select(ConvertUserToUserResponse).ToList();
    }

    public UserResponse? GetUserByUserId(Guid? userId)
    {
        var foundUser = _listOfUsers.FirstOrDefault(user => user.UserId == userId);

        return foundUser == null ? null : ConvertUserToUserResponse(foundUser);
    }

    public UserResponse? GetUserByUserLoginOrEmail(string? loginOrEmail)
    {
        var foundUser = _listOfUsers.FirstOrDefault(user => user.Login == loginOrEmail || 
                                                          user.Email == loginOrEmail);
        
        return foundUser == null ? null : ConvertUserToUserResponse(foundUser);

    }

    public UserResponse UpdateUser(UserUpdateRequest? userUpdateRequest)
    {
        if (userUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(userUpdateRequest));
        }

        ValidationHelper.ModelValidation(userUpdateRequest);
        
        // object to update
        var matchingUser = _listOfUsers.FirstOrDefault(user => user.UserId == userUpdateRequest.UserId);

        if (matchingUser == null)
        {
            throw new ArgumentException("Given user ID doesn't exists");
        }
        
        // email or password repeated
        if (matchingUser.Password == userUpdateRequest.Password)
        {
            throw new ArgumentException("New password must be different from current password");
        }
        
        if (matchingUser.Email == userUpdateRequest.Email)
        {
            throw new ArgumentException("New email must be different from current email");
        }
        
        // update user
        matchingUser.Password = userUpdateRequest.Password;
        matchingUser.Email = userUpdateRequest.Email;
        matchingUser.PhoneNumber = userUpdateRequest.PhoneNumber;

        return matchingUser.ToUserResponse();
    }
}