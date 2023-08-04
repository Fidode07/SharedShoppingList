using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedShoppingListApi.Data;
using SharedShoppingListApi.Dtos;
using SharedShoppingListApi.Models;
using System.Text.RegularExpressions;

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

            if(string.IsNullOrEmpty(uniqueUserId))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User Id is required";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }
            
            List<GroupsDto> groupsDto = new List<GroupsDto>();

            var groupsFromUser = _mainDbContext.Groups.Where(group => group.Members.Any(member => member.UniqueId == uniqueUserId));

            foreach (var group in groupsFromUser)
            {
                groupsDto.Add(new GroupsDto
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description
                });
            }

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "OK";
            serviceResponse.Data = groupsDto;

            return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
        }
        [HttpPost("create_group")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateGroup(CreateGroupDto createGroupDto)
        {
            var serviceResponse = new ServiceResponse<int>();

            if (string.IsNullOrEmpty(createGroupDto.Name))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "The group must have a name";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            Models.Group newGroup = new Models.Group();

            _mainDbContext.Groups.Add(newGroup);
            await _mainDbContext.SaveChangesAsync();

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "Successfully created a group";
            serviceResponse.Data = newGroup.Id;

            return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
        }

        [HttpPost("join_group")]
        public async Task<ActionResult<ServiceResponse>> JoinGroup(JoinGroupDto joinGroupDto)
        {
            var serviceResponse = new ServiceResponse();

            var groupFromDb = await _mainDbContext.Groups.Where(x => x.Id == joinGroupDto.GroupId).FirstOrDefaultAsync();

            if (groupFromDb == null)
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Group not found";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            var user = await _mainDbContext.Users.Where(x => x.UniqueId == joinGroupDto.UniqueUserId).FirstOrDefaultAsync();

            if (user == null)
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User not found";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            groupFromDb.Members.Add(user);
            _mainDbContext.Groups.Update(groupFromDb);
            await _mainDbContext.SaveChangesAsync();

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = $"Successfully joined the group: {groupFromDb.Name}";

            return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
        }

        [HttpGet("get_group")]
        public async Task<ActionResult<ServiceResponse<Models.Group>>> GetGroup(int groupId, string uniqueUserId)
        {
            var serviceResponse = new ServiceResponse<Models.Group>();

            var groupFromDb = await _mainDbContext.Groups.Where(x => x.Id == groupId).FirstOrDefaultAsync();

            if (groupFromDb == null)
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Group not found";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            var user = await _mainDbContext.Users.Where(x => x.UniqueId == uniqueUserId).FirstOrDefaultAsync();

            if (user == null)
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User not found";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            if(!groupFromDb.Members.Any(x => x.UniqueId == uniqueUserId))
            {
                serviceResponse.StatusCode = 401;
                serviceResponse.Message = "You are not in this group";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = $"Success";
            serviceResponse.Data = groupFromDb;

            return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
        }
    }
}
