using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Dtos;
using chatgroup_server.Common;

namespace chatgroup_server.Services
{
    public class GroupService:IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
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
            try
            {
                var result=await _groupRepository.GetAllGroupsAsync(userId);
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

        public async Task<bool> UpdateGroupAsync(Group group)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _groupRepository.UpdateGroup(group);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
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
