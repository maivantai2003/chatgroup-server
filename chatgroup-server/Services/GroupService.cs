using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using System.Collections.Generic;
using System.Net.WebSockets;
using tryAGI.OpenAI;

namespace chatgroup_server.Services
{
    public class GroupService:IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RedisService _redisService;
        private readonly IUserContextService _userContextService;
        public GroupService(IGroupRepository groupRepository, IUnitOfWork unitOfWork,RedisService redisService, IUserContextService userContextService)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _redisService = redisService;   
            _userContextService = userContextService;
        }

        public async Task<ApiResponse<GroupUserDto?>> GetGroupByIdAsync(int groupId)
        {
            try
            {

                var result = await _groupRepository.GetGroupByIdAsync(groupId);
                return ApiResponse<GroupUserDto?>.SuccessResponse("Danh sách thành viên nhóm", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<GroupUserDto?>.ErrorResponse("Error", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<IEnumerable<GroupUserDto>>> GetAllGroupsAsync(int userId)
        {
            Console.WriteLine("access: "+_userContextService.GetCurrentUserId());
            string cacheKey = CacheKeys.GroupsOfUser(userId);
            try
            {
                var groupsOfUser = await _redisService.GetCacheAsync<IEnumerable<GroupUserDto>>(cacheKey);
                if (groupsOfUser != null && groupsOfUser.Any()) {
                    return ApiResponse<IEnumerable<GroupUserDto>>.SuccessResponse("Danh sách nhóm", groupsOfUser);
                }
                var result=await _groupRepository.GetAllGroupsAsync(userId);
                await _redisService.SetCacheAsync(cacheKey, result,TimeSpan.FromMinutes(CacheKeys.Time));
                return ApiResponse<IEnumerable<GroupUserDto>>.SuccessResponse("Danh sách nhóm",result);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<GroupUserDto>>.ErrorResponse("Error", new List<string>()
                {
                    ex.Message  
                });
            }
        }
        public async Task<ApiResponse<Group>> AddGroupAsync(Group group)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _groupRepository.AddGroupAsync(group);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.GroupsOfUser(_userContextService.GetCurrentUserId()));
                return ApiResponse<Group>.SuccessResponse("Tạo nhóm thành công",group);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Group>.ErrorResponse("Tạo nhóm không thành công", new List<string>()
                {
                    ex.Message  
                });
            }
        }

        public async Task<ApiResponse<Group>> UpdateGroupAsync(Group group)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _groupRepository.UpdateGroup(group);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.GroupsOfUser(_userContextService.GetCurrentUserId()));
                return ApiResponse<Group>.SuccessResponse("Cập nhật nhóm thành công",group);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Group>.SuccessResponse("Cập nhật nhóm không thành công", group);
            }
        }

        public async Task<bool> DeleteGroupAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //_groupRepository.DeleteGroup(group);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.GroupsOfUser(_userContextService.GetCurrentUserId()));
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
    }
}
