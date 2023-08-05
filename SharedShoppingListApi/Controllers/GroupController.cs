using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedShoppingListApi.Data;
using SharedShoppingListApi.Dtos;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SharedShoppingListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly MainDbContext _mainDbContext;

        public GroupController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        [HttpGet("get_groups")]
        public async Task<ActionResult<ServiceResponse<List<GroupsDto>>>> GetGroups(string uniqueUserId)
        {
            var serviceResponse = new ServiceResponse<List<GroupsDto>>();

            if (string.IsNullOrEmpty(uniqueUserId))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User Id is required";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var user = await _mainDbContext.Users
                .Include(u => u.Groups)
                .FirstOrDefaultAsync(u => u.UniqueId == uniqueUserId);

            if (user == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "User not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            List<GroupsDto> groupsDto = user.Groups.Select(ug => new GroupsDto
            {
                Id = ug.Id,
                Name = ug.Name,
                Description = ug.Description
            }).ToList();

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "OK";
            serviceResponse.Data = groupsDto;

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPost("create_group")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateGroup(CreateGroupDto createGroupDto)
        {
            var serviceResponse = new ServiceResponse<int>();

            if (string.IsNullOrEmpty(createGroupDto.Name))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "The group must have a name";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.UniqueId == createGroupDto.UniqueUserId);

            if (user == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "User not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            Models.Group newGroup = new Models.Group
            {
                Name = createGroupDto.Name,
                Description = createGroupDto.Description
            };

            newGroup.Members.Add( user );

            _mainDbContext.Groups.Add(newGroup);
            await _mainDbContext.SaveChangesAsync();

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "Successfully created a group";
            serviceResponse.Data = newGroup.Id;

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }
        [HttpPost("join_group")]
        public async Task<ActionResult<ServiceResponse>> JoinGroup(JoinGroupDto joinGroupDto)
        {
            var serviceResponse = new ServiceResponse();

            var groupFromDb = await _mainDbContext.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == joinGroupDto.GroupId);

            if (groupFromDb == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "Group not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.UniqueId == joinGroupDto.UniqueUserId);

            if (user == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "User not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (groupFromDb.Members.Any(ug => ug.UniqueId == joinGroupDto.UniqueUserId))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User is already a member of this group";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            groupFromDb.Members.Add( user );

            await _mainDbContext.SaveChangesAsync();

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = $"Successfully joined the group: {groupFromDb.Name}";

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [HttpGet("get_group")]
        public async Task<ActionResult<ServiceResponse<Models.Group>>> GetGroup(int groupId, string uniqueUserId)
        {
            var serviceResponse = new ServiceResponse<Models.Group>();

            var groupFromDb = await _mainDbContext.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (groupFromDb == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "Group not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.UniqueId == uniqueUserId);

            if (user == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "User not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (!groupFromDb.Members.Any(ug => ug.UniqueId == uniqueUserId))
            {
                serviceResponse.StatusCode = 401;
                serviceResponse.Message = "You are not in this group";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Use ReferenceHandler.Preserve to handle object cycles
                WriteIndented = true // Optional: Format JSON for better readability
            };

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "Success";
            serviceResponse.Data = groupFromDb;



            return Content(JsonSerializer.Serialize(serviceResponse, options), "application/json");
        }


        [HttpPost("leave_group")]
        public async Task<ActionResult<ServiceResponse>> LeaveGroup(LeaveGroupDto leaveGroupDto)
        {
            var serviceResponse = new ServiceResponse();

            var groupFromDb = await _mainDbContext.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == leaveGroupDto.GroupId);

            if (groupFromDb == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "Group not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.UniqueId == leaveGroupDto.UniqueUserId);

            if (user == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "User not found";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            var userGroup = groupFromDb.Members.FirstOrDefault(ug => ug.UniqueId == leaveGroupDto.UniqueUserId);

            if (userGroup == null)
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User is not a member of this group";
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            groupFromDb.Members.Remove(userGroup);

            await _mainDbContext.SaveChangesAsync();

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = $"Successfully left the group: {groupFromDb.Name}";

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }
    }
}
