using Mapster;
using LegoProjectApiV2.DAL.Lists;
using LegoProjectApiV2.Models.Entities;
using LegoProjectApiV2.Models.DTOs.List;
using LegoProjectApiV2.Services.Users;
using LegoProjectApiV2.Models.DTOs.User;
using System.ComponentModel.DataAnnotations;
using LegoProjectApiV2.Models.DTOs.Brick;
using LegoProjectApiV2.Tools;

namespace LegoProjectApiV2.Services.Lists
{
    public class ListService : IListService
    {
        private readonly Lists_IDAL _dal;
        private readonly IUserService _userService;
        private readonly ApiServiceResponseProducer _responseProducer;
        public ListService(Lists_IDAL dal, IUserService userService)
        {
            _dal = dal;
            _responseProducer = new ApiServiceResponseProducer();
            _userService = userService;
        }

        public async Task<ApiServiceResponse<List<ListDTO>>> GetListsByUserEmail(string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            List<List> listsByUserId = new List<List>();
            List<ListDTO> lists = new List<ListDTO>();
            if (user.ID != 0)
            {         
                try
                {
                    listsByUserId = await _dal.GetListsByUserIdAsync(user.ID);
                    lists = listsByUserId.Adapt<List<ListDTO>>();
                    for (int i = 0; i < lists.Count; i++)
                    {
                        lists[i].UniqueCount = listsByUserId[i].Parts.Count;
                    }
                    serviceMessage = "All lists displayed.";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Something went wrong while fetching user lists. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception("Email wasn`t found.");
            }
            return _responseProducer.ProduceResponse<List<ListDTO>>(serviceMessage, lists, serviceException);
        }

        public async Task<ApiServiceResponse<ListDTO>> GetListById(int id, string userEmail)
        {
            List listById = null;
            Exception serviceException = null;
            string serviceMessage;
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            if (user.ID != 0)
            {
                try
                {
                    listById = await _dal.ListById(id, user.ID);
                    if (listById.ID != 0)
                        serviceMessage = $"List with #ID: {listById.ID} was found";
                    else
                        serviceMessage = $"List with #ID: {id} wasn`t found";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = $"An error occured while searching for list #{id}. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception("Email wasn`t found.");
            }
            return _responseProducer.ProduceResponse(serviceMessage, listById.Adapt<ListDTO>(), serviceException);
        }

        public async Task<ApiServiceResponse<ListDTO>> CreateList(NewListDTO newListDTO, string userEmail)
        {
            List newList = newListDTO.Adapt<List>();
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            List listFromDB = null;
            Exception serviceException = null;
            string serviceMessage;
            if(user.ID != 0)
            {
                newList.UserID = user.ID;
                if (ModelValidator.Validate(newList))
                {
                    try
                    {
                        listFromDB = await _dal.ListAddAsync(newList);
                        serviceMessage = $"New list added successfully.";
                    }
                    catch (Exception ex)
                    {
                        serviceException = ex;
                        serviceMessage = "Can`t save changes. Please try again later.";
                    }
                }
                else
                {
                    serviceMessage = "This list data is incorrect.";
                    serviceException = new ValidationException("ModelState is invalid.");
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }     
            return _responseProducer.ProduceResponse(serviceMessage, listFromDB.Adapt<ListDTO>(), serviceException);
        }

        public async Task<ApiServiceResponse<bool>> UpdateList(ListDTO listToEdit, string userEmail)
        {
            List listToUpdate = listToEdit.Adapt<List>();
            bool saveResult = false;
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            Exception serviceException = null;
            string serviceMessage;
            if (user.ID != 0)
            {
                if (ModelValidator.Validate(listToUpdate))
                {
                    try
                    {
                        saveResult = await _dal.ListUpdateAsync(listToUpdate, user.ID);
                        serviceMessage = $"Changes saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        serviceException = ex;
                        serviceMessage = "Cant save changes. Please try again later.";
                    }
                }
                else
                {
                    serviceMessage = "This list data is incorrect.";
                    serviceException = new ValidationException("ModelState is invalid.");
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }            
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> DeleteList(int id, string userEmail)
        {
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            Exception serviceException = null;
            string serviceMessage;
            bool deleteResult = false;
            if (user.ID != 0)
            {
                try
                {
                    deleteResult = await _dal.ListDeleteAsync(id, user.ID);
                    serviceMessage = $"List #{user.ID}_{id} deleted successfully";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Cant save changes. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }
            return _responseProducer.ProduceResponse(serviceMessage, deleteResult, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> AddBrick(int id, BrickListDTO brick, string userEmail)
        {
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            Exception serviceException = null;
            string serviceMessage;
            bool saveResult = false;
            if (user.ID != 0)
            {
                PartList partList = brick.Adapt<PartList>();
                partList.ListID = id;
                partList.PartID = brick.BrickID;
                try
                {
                    if (partList.ListID == -1)
                    {
                        List newList = new List();
                        newList.Name = "New List";
                        newList.UserID = user.ID;
                        List listFromDB = await _dal.ListAddAsync(newList);
                        if (listFromDB.ID != 0)
                        {
                            partList.ListID = listFromDB.ID;
                        }
                        else
                        {
                            serviceMessage = "Something went wrong while creating new list. Please try again later.";
                            serviceException = new Exception(serviceMessage);
                            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
                        }
                    }
                    saveResult = await _dal.ListAddPartAsync(partList, user.ID);
                    serviceMessage = "Brick added successfully.";
                }
                catch (Exception ex)
                {
                    serviceMessage = "Cant save changes. Please try again later.";
                    serviceException = ex;
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }

        public async Task<ApiServiceResponse<int>> AddBricks(int id, List<BrickListDTO> bricks, string userEmail)
        {
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            Exception serviceException = null;
            string serviceMessage;
            int saveResult = 0;
            if (user.ID != 0)
            {
                try
                {
                    bricks.ForEach(async brick =>
                    {
                        PartList partList = brick.Adapt<PartList>();
                        partList.ListID = id;
                        partList.PartID = brick.BrickID;
                        await _dal.ListAddPartAsync(partList, user.ID);            
                    });
                    saveResult = bricks.Count;
                    serviceMessage = saveResult + " bricks added.";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Cant save changes. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> EditBrickInList(int id, BrickListDTO brick, string userEmail)
        {
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            Exception serviceException = null;
            string serviceMessage;
            bool saveResult = false;
            if (user.ID != 0)
            {
                PartList partList = brick.Adapt<PartList>();
                partList.ListID = id;
                partList.PartID = brick.BrickID;
                try
                {
                    saveResult = await _dal.ListEditPartAsync(partList, user.ID);
                    serviceMessage = "Brick saved successfully.";
                }
                catch (Exception ex)
                {
                    serviceMessage = "Cant save changes. Please try again later.";
                    serviceException = ex;
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> DeleteBrickFromList(int id, BrickListDeleteDTO brick, string userEmail)
        {
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            Exception serviceException = null;
            string serviceMessage;
            bool saveResult = false;
            if (user.ID != 0)
            {
                PartList partList = brick.Adapt<PartList>();
                partList.ListID = id;
                partList.PartID = brick.BrickID;
                try
                {
                    saveResult = await _dal.ListDeletePartAsync(partList, user.ID);
                    serviceMessage = "Brick deleted successfully.";
                }
                catch (Exception ex)
                {
                    serviceMessage = "Cant save changes. Please try again later.";
                    serviceException = ex;
                }
            }
            else
            {
                serviceMessage = "Email wasn`t found.";
                serviceException = new Exception(serviceMessage);
            }
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }
    }
}
