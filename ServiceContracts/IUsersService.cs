using ServiceContracts.DTO;
using ServiceContracts.DTO.User;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating User entity
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Adds a user object to the list of users
    /// </summary>
    /// <param name="userAddRequest">User object to add</param>
    /// <returns>Returns user object after adding it (including generated id)</returns>
    UserResponse AddUser(UserAddRequest? userAddRequest);

    /// <summary>
    /// Returns all users from list
    /// </summary>
    List<UserResponse> GetAllUsers();

    /// <summary>
    /// Returns user object based on given user id
    /// </summary>
    /// <param name="userId">Represents user id (Guid) that you want to search</param>
    /// <returns>Matching user object as UserResponse</returns>
    UserResponse? GetUserByUserId(Guid? userId);
    
    /// <summary>
    /// Returns user object based on given user login or email
    /// </summary>
    /// <param name="loginOrEmail">Represents user login or email that you want to search</param>
    /// <returns>Matching user object as UserResponse</returns>
    UserResponse? GetUserByUserLoginOrEmail(string? loginOrEmail);
    
    UserResponse UpdateUser(UserUpdateRequest? userUpdateRequest);
}