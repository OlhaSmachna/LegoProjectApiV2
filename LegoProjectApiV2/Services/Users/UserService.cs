using System.ComponentModel.DataAnnotations;
using Mapster;
using LegoProjectApiV2.DAL.Users;
using LegoProjectApiV2.Models.DTOs.User;
using LegoProjectApiV2.Models.Entities;
using LegoProjectApiV2.Tools;

namespace LegoProjectApiV2.Services.Users
{
    public class UserService : IUserService
    {
        private readonly Users_IDAL _dal;
        private readonly JwtHandler _jwtHandler;
        private readonly ApiServiceResponseProducer _responseProducer;
        private static readonly string ADMIN_ROLE = "ADMIN";
        private static readonly string DEV_ROLE = "DEVELOPER";

        public UserService(Users_IDAL dal, JwtHandler jwtHandler)
        {
            _dal = dal;
            _jwtHandler = jwtHandler;
            _responseProducer = new ApiServiceResponseProducer();
        }

        public async Task<ApiServiceResponse<List<UserDTO>>> GetUsers()
        {
            List<User> allUsers = new List<User>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                allUsers = await _dal.GetUsersAsync();
                serviceMessage = "All users displayed.";
            }
            catch(Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching users. Please try again later.";
            }
            return _responseProducer.ProduceResponse<List<UserDTO>>(serviceMessage, allUsers.Adapt<List<UserDTO>>(), serviceException);
        }

        public async Task<ApiServiceResponse<UserDTO>> GetUserById(int id)
        {
            User userById = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                userById = await _dal.UserById(id); 
                if (userById.ID != 0)
                    serviceMessage = $"User with #ID: {userById.ID} was found";
                else
                    serviceMessage = $"User with #ID: {id} wasn`t found";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"An error occured while searching for user #{id}. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, userById.Adapt<UserDTO>(), serviceException);
        }

        public async Task<UserDTOExtended> GetUserByEmail(string userEmail)
        {
            User userByEmail = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                userByEmail = await _dal.UserByEmailAsync(userEmail);
                if (userByEmail.ID != 0)
                    serviceMessage = $"User with email: {userByEmail.Email} was found";
                else
                    serviceMessage = $"User with email: {userEmail} wasn`t found";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"An error occured while searching for user with email: {userEmail}. Please try again later.";
            }
            return userByEmail.Adapt<UserDTOExtended>();
        }

        public async Task<ApiServiceResponse<AddResponceUserDTO>> CreateUser(AddRequestUserDTO addRequestUserDTO)
        {
            User newUser = addRequestUserDTO.Adapt<User>();
            User userFromDB = null;
            Exception serviceException = null;
            string serviceMessage;

            newUser.PasswordHash = Hasher.CreateHash_SHA512(addRequestUserDTO.Password);
            if (
                ModelValidator.Validate(newUser)
                && addRequestUserDTO.Password.Length >= 8
                && !String.IsNullOrWhiteSpace(addRequestUserDTO.Password)
                )
            {
                try
                {
                    userFromDB = await _dal.UserByEmailAsync(addRequestUserDTO.Email);
                    if (userFromDB.ID == 0)
                    {
                        userFromDB = await _dal.UserAddAsync(newUser);
                        serviceMessage = $"Registration successful.";
                    }
                    else
                    {
                        userFromDB = new User();
                        serviceException = new Exception();
                        serviceMessage = $"This email is already registred.";
                    }

                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Can`t register new user. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "User data is incorrect.";
                serviceException = new ValidationException("ModelState is invalid.");
            }          
            return _responseProducer.ProduceResponse(serviceMessage, userFromDB.Adapt<AddResponceUserDTO>(), serviceException);
        }
        
        public async Task<ApiServiceResponse<bool>> UpdateUser(EditRequestUserDTO editRequestUserDTO)
        {
            User userToUpdate = editRequestUserDTO.Adapt<User>();  
            bool saveResult = false;
            Exception serviceException = null;
            string serviceMessage;

            if (
                ModelValidator.Validate(userToUpdate)
                && editRequestUserDTO.NewPassword.Length >= 8
                && !String.IsNullOrWhiteSpace(editRequestUserDTO.NewPassword)
                )
            {
                userToUpdate.PasswordHash = Hasher.CreateHash_SHA512(editRequestUserDTO.NewPassword);
                try
                {
                    saveResult = await _dal.UserUpdateAsync(userToUpdate);
                    serviceMessage = $"User with #ID: {userToUpdate.ID} was updated.";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Cant save changes. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "New password is incorrect.";
                serviceException = new ValidationException("ModelState is invalid.");
            }
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> DeleteUser(int id)
        {
            Exception serviceException = null;
            string serviceMessage;
            bool deleteResult = false;
            try
            {
                deleteResult = await _dal.UserDeleteAsync(id);
                serviceMessage = $"User with #ID: {id} was deleted";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Cant save changes. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, deleteResult, serviceException);
        }

        public async Task<ApiServiceResponse<LoginResponseUserDTO>> Login(LoginRequestUserDTO loginRequestUserDTO)
        {
            User userByEmail = null;
            LoginResponseUserDTO loginResponseUserDTO = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                userByEmail = await _dal.UserByEmailAsync(loginRequestUserDTO.Email);
                if (userByEmail.ID == 0) { 
                    loginResponseUserDTO = new LoginResponseUserDTO();
                    serviceMessage = $"Email not found.";
                }
                else if(!Hasher.CheckPassword(userByEmail.PasswordHash, loginRequestUserDTO.Password))
                {
                    loginResponseUserDTO = new LoginResponseUserDTO();
                    serviceMessage = $"Incorrect password.";
                }
                else
                {
                    loginResponseUserDTO = await GetResponseWithTokens(userByEmail);
                    serviceMessage = $"Login successful.";
                }            
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"Login failed. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, loginResponseUserDTO, serviceException);
        }

        public async Task<ApiServiceResponse<LoginResponseUserDTO>> RefreshToken(RefreshTokenRequestUserDTO refreshTokenRequestUserDTO)
        {
            User userByEmail = null;
            LoginResponseUserDTO loginResponseUserDTO = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                userByEmail = await _dal.UserByEmailAsync(refreshTokenRequestUserDTO.Email);
                if (userByEmail.ID != 0 && userByEmail.RefreshToken.Token == refreshTokenRequestUserDTO.RefreshToken)
                {
                    if (userByEmail.RefreshToken.Expires > DateTime.Now)
                    {
                        loginResponseUserDTO = await GetResponseWithTokens(userByEmail);
                        serviceMessage = $"Token refreshed successfully.";
                    }
                    else
                    {
                        serviceException = new UnauthorizedAccessException("Token expired.");
                        serviceMessage = $"Token expired. Please relogin.";
                    }
                }
                else
                {
                    loginResponseUserDTO = new LoginResponseUserDTO();
                    serviceException = new UnauthorizedAccessException("Token invalid.");
                    serviceMessage = $"Token invalid. Please relogin.";                   
                }
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"Login failed. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, loginResponseUserDTO, serviceException);
        }

        public async Task<LoginResponseUserDTO> GetResponseWithTokens(User user)
        {
            RefreshToken refreshToken = _jwtHandler.IssueRefreshToken();
            user.RefreshToken = refreshToken;
            string token = _jwtHandler.IssueAccessToken(user);
            await _dal.RefreshTokenAsync(user);

            LoginResponseUserDTO loginResponseUserDTO = user.Adapt<LoginResponseUserDTO>();
            loginResponseUserDTO.Token = token;
            loginResponseUserDTO.RefreshToken = refreshToken.Token;
            return loginResponseUserDTO;
        }

        public async Task<bool> CheckAdminRole(string userEmail)
        {
            User userByEmail = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                userByEmail = await _dal.UserByEmailAsync(userEmail);
                if (userByEmail.ID != 0)
                    serviceMessage = $"User with email: {userByEmail.Email} was found";
                else
                    serviceMessage = $"User with email: {userEmail} wasn`t found";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"An error occured while checking user role. Please try again later.";
            }
            return userByEmail.Role.Name == ADMIN_ROLE || userByEmail.Role.Name == DEV_ROLE;
        }
    
    }
}
