using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mapster;
using LegoProjectApiV2.DAL.Parts;
using LegoProjectApiV2.Services.Users;
using LegoProjectApiV2.Models.Entities;
using LegoProjectApiV2.Models.DTOs.Brick;
using LegoProjectApiV2.Models.DTOs.Color;
using LegoProjectApiV2.Models.DTOs.User;
using LegoProjectApiV2.Tools;
using LegoProjectApiV2.Exceptions;

namespace LegoProjectApiV2.Services.Bricks
{
    public class BrickService : IBrickService
    {
        private readonly Parts_IDAL _dal;
        private readonly IUserService _userService;
        private readonly ApiServiceResponseProducer _responseProducer;
        public IConfiguration Configuration { get; }
        public BrickService(
            Parts_IDAL dal,
            JwtHandler jwtHandler,
            IConfiguration configuration,
            IUserService userService
            )
        {
            _dal = dal;
            _responseProducer = new ApiServiceResponseProducer();
            _userService = userService;
            Configuration = configuration;
        }

        public async Task<ApiServiceResponse<List<BrickDTO>>> GetBricks()
        {
            List<Part> allParts = new List<Part>();
            List<BrickDTO> allBricks = new List<BrickDTO>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                allParts = await _dal.GetPartsAsync();
                serviceMessage = "All bricks displayed.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching bricks. Please try again later.";
            }
            allBricks = BricksToDTOs(allParts);
            return _responseProducer.ProduceResponse<List<BrickDTO>>(serviceMessage, allBricks, serviceException);
        }

        public async Task<ApiServiceResponse<List<BrickDTO>>> GetBricksByCategoryId(int categoryId)
        {
            List<Part> parts = new List<Part>();
            List<BrickDTO> bricks = new List<BrickDTO>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                parts = await _dal.GetPartsByCategoryIdAsync(categoryId);
                serviceMessage = "All bricks displayed.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching bricks. Please try again later.";
            }
            bricks = BricksToDTOs(parts);
            return _responseProducer.ProduceResponse<List<BrickDTO>>(serviceMessage, bricks, serviceException);
        }

        public async Task<ApiServiceResponse<List<BrickDTO>>> GetBricksByListId(int listId, string userEmail)
        {
            List<PartList> partLists = new List<PartList>();
            List<BrickDTO> bricks = new List<BrickDTO>();
            Exception serviceException = null;
            string serviceMessage;
            UserDTOExtended user = await _userService.GetUserByEmail(userEmail);
            if (user.ID != 0)
            {
                try
                {
                    partLists = await _dal.GetPartsByListIdAsync(listId, user.ID);
                    partLists.ForEach(pl =>
                    {
                        bricks.Add(pl.Part.Adapt<BrickDTO>());
                        bricks.Last().Colors = new List<ColorDTO>();
                        bricks.Last().Colors.Add(pl.Color.Adapt<ColorDTO>());
                        bricks.Last().Quantity = pl.Quantity;
                        bricks.Last().Material = pl.Part.Material.Name;
                    });
                    serviceMessage = "All user bricks displayed.";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Something went wrong while fetching bricks. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
            }
            return _responseProducer.ProduceResponse<List<BrickDTO>>(serviceMessage, bricks, serviceException);
        }

        public async Task<ApiServiceResponse<List<BrickDTO>>> SearchBricks(string pattern)
        {
            List<Part> parts = new List<Part>();
            List<BrickDTO> bricks = new List<BrickDTO>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                parts = await _dal.SearchPartsAsync(pattern.ToLower());
                if (parts.Count != 0) serviceMessage = "All found bricks displayed.";
                else serviceMessage = "Nothing found.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while searching for bricks. Please try again later.";
            }
            bricks = BricksToDTOs(parts);
            return _responseProducer.ProduceResponse<List<BrickDTO>>(serviceMessage, bricks, serviceException);
        }

        public async Task<ApiServiceResponse<BrickDTO>> GetBrickById(string id)
        {
            Part partById = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                partById = await _dal.PartById(id);
                if (partById.ID != null)
                {
                    serviceMessage = $"Brick with #ID: {partById.ID} was found";
                }
                else
                {
                    serviceMessage = $"Brick with #ID: {id} wasn`t found";
                    serviceException = new Exception(serviceMessage);
                }
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"An error occured while searching for brick #{id}. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, partById.Adapt<BrickDTO>(), serviceException);
        }

        public async Task<ApiServiceResponse<BrickExtendedDTO>> GetBrickDetailsById(string id)
        {
            Part partById = null;
            BrickExtendedDTO brickExtendedDTO = new BrickExtendedDTO();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                partById = await _dal.PartById(id);
                if (partById.ID != null)
                {
                    brickExtendedDTO = partById.Adapt<BrickExtendedDTO>();
                    if (partById.Colors != null)
                    {
                        brickExtendedDTO.ColorIDs = new List<int>();
                        foreach (PartColor pc in partById.Colors)
                        {
                            brickExtendedDTO.ColorIDs.Add(pc.ColorID);
                        }
                    }
                    serviceMessage = $"Brick with #ID: {partById.ID} was found";
                }
                else
                {
                    serviceMessage = $"Brick with #ID: {id} wasn`t found";
                    serviceException = new Exception(serviceMessage);
                }
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"An error occured while searching for brick #{id}. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, brickExtendedDTO, serviceException);
        }

        public async Task<ApiServiceResponse<BrickDTO>> CreateBrick(BrickExtendedDTO brickDTO, string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            if (await _userService.CheckAdminRole(userEmail))
            {
                Part newPart = brickDTO.Adapt<Part>();
                Part partFromDB = null;
                if (ModelValidator.Validate(newPart))
                {
                    try
                    {
                        partFromDB = await _dal.PartById(brickDTO.ID);
                        if (partFromDB.ID == null)
                        {
                            if (!(brickDTO.ColorIDs?.Any() ?? false))
                            {
                                newPart.Colors = new List<PartColor>();
                                brickDTO.ColorIDs.ForEach(c =>
                                {
                                    PartColor pc = new PartColor();
                                    pc.ColorID = c;
                                    pc.PartID = newPart.ID;
                                    newPart.Colors.Add(pc);
                                });
                            }

                            partFromDB = await _dal.PartAddAsync(newPart);
                            serviceMessage = $"New brick added successfully. Refresh to see the result.";
                        }
                        else
                        {
                            partFromDB = new Part();
                            serviceMessage = $"Brick with this ID already exists.";
                            serviceException = new Exception(serviceMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        serviceException = ex;
                        serviceMessage = "Can`t add new brick. Please try again later.";
                    }
                }
                else
                {
                    serviceMessage = "This brick data is incorrect.";
                    serviceException = new ValidationException("ModelState is invalid.");
                }
                return _responseProducer.ProduceResponse(serviceMessage, partFromDB.Adapt<BrickDTO>(), serviceException);
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
                return _responseProducer.ProduceResponse(serviceMessage, new BrickDTO(), serviceException);
            }
        }

        public async Task<ApiServiceResponse<BrickDTO>> UpdateBrick(BrickExtendedDTO brickDTO, string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            bool saveResult = false;
            BrickDTO updatedBrick = new BrickDTO();
            if (await _userService.CheckAdminRole(userEmail))
            {
                Part partToUpdate = brickDTO.Adapt<Part>();
                partToUpdate.Colors = new List<PartColor>();
                if (!(brickDTO.ColorIDs?.Any() ?? false))
                {
                    brickDTO.ColorIDs.ForEach(color =>
                    {
                        PartColor pc = new PartColor();
                        pc.ColorID = color;
                        pc.PartID = partToUpdate.ID;
                        partToUpdate.Colors.Add(pc);
                    });
                }
                if (ModelValidator.Validate(partToUpdate))
                {
                    try
                    {
                        saveResult = await _dal.PartUpdateAsync(partToUpdate);
                        serviceMessage = $"Part with #ID: {partToUpdate.ID} was updated.";
                    }
                    catch (Exception ex)
                    {
                        serviceException = ex;
                        serviceMessage = "Cant save changes. Please try again later.";
                    }
                }
                else
                {
                    serviceMessage = "This brick data is incorrect.";
                    serviceException = new ValidationException("ModelState is invalid.");
                }
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
            }
            if (saveResult)
            {
                Part partFromDB = await _dal.PartById(brickDTO.ID);
                if (partFromDB.ID != null)
                {
                    updatedBrick = partFromDB.Adapt<BrickDTO>();
                    updatedBrick.Material = partFromDB.Material.Name;
                    updatedBrick.Colors = new List<ColorDTO>();
                    partFromDB.Colors.ToList().ForEach(color => {
                        ColorDTO colorDTO = color.Color.Adapt<ColorDTO>();
                        updatedBrick.Colors.Add(colorDTO);
                    });
                }
            }
            return _responseProducer.ProduceResponse(serviceMessage, updatedBrick, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> DeleteBrick(string id, string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            bool deleteResult = false;
            if (await _userService.CheckAdminRole(userEmail))
            {
                try
                {
                    deleteResult = await _dal.PartDeleteAsync(id);
                    serviceMessage = $"Part with #ID: {id} was deleted";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Cant save changes. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
            }
            return _responseProducer.ProduceResponse(serviceMessage, deleteResult, serviceException);
        }


        private List<BrickDTO> BricksToDTOs(List<Part> parts)
        {
            List<BrickDTO> bricks = parts.Adapt<List<BrickDTO>>();
            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Material = parts[i].Material.Name;
                bricks[i].Colors = new List<ColorDTO>();
                parts[i].Colors.ToList().ForEach(color => {
                    ColorDTO colorDTO = color.Color.Adapt<ColorDTO>();
                    bricks[i].Colors.Add(colorDTO);
                });
            }
            return bricks;
        }
        public void FillCloud()
        {
            string[] files = Directory.GetFiles(@"D:\Step\LegoProject\bricks");
            Cloudinary cloudinary = new Cloudinary(Configuration.GetSection("CloudinaryUrl").Value);
            cloudinary.Api.Secure = true;
            var uploadParams = new ImageUploadParams()
            {
                Folder = "lego-project",
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            Array.ForEach(files, f => {
                uploadParams.File = new FileDescription(f);
                cloudinary.Upload(uploadParams);
            });
        }
        public void FillDB()
        {
            string[] files = Directory.GetFiles(@"D:\Step\LegoProject\bricks");
            Array.ForEach(files, f => {
                _dal.SetHasImgTrue(Path.GetFileNameWithoutExtension(f));
            });
        }
    }
}
