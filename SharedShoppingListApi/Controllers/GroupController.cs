﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingListApi.Data;
using SharedShoppingListApi.Dtos;
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
        public async Task<ActionResult<ServiceResponse<List<GroupsDto>>>> GetGroups(GetGroupsDto getGroupsDto)
        {
            var serviceResponse = new ServiceResponse<List<GroupsDto>>();

            if(getGroupsDto.UserId == 0)
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "User Id is required";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }
            
            List<GroupsDto> groupsDto = new List<GroupsDto>();

            var groupsFromUser = _mainDbContext.Groups.Where(group => group.Members.Any(member => member.Id == getGroupsDto.UserId));

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

            if (string.IsNullOrEmpty(createGroupDto.Name) || string.IsNullOrEmpty(createGroupDto.Description))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Group props are empty";
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
    }
}
