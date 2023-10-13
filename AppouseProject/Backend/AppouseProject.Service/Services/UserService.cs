using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Abstract.UnitOfWorks;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Entities;
using AppouseProject.Core.EntityConst;
using AppouseProject.Core.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppouseProject.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotaRepository _quotaRepository;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork, IQuotaRepository quotaRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _quotaRepository = quotaRepository;
        }

        public async Task<bool> AnyAsync(Expression<Func<AppUser, bool>> expression)
        {
            bool user = await _userManager.Users.AnyAsync(expression);
            return user;
        }

        public async Task<CustomResponseDto<NoContentDto>> ChangeEmailAsync(string userName, string newEmail)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return CustomResponseDto<NoContentDto>.Failure(404, "UserName bulunamadı");
            }
            string token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            if (!result.Succeeded)
            {
                return CustomResponseDto<NoContentDto>.Failure(500, "Bir hata meydana geldi işlem başarısız");
            }
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);
        }

        public async Task<CustomResponseDto<NoContentDto>> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return CustomResponseDto<NoContentDto>.Failure(404, "UserName bulunamadı");
            }
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                return CustomResponseDto<NoContentDto>.Failure(400, "Hatalı şifre");
            }
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(204);


        }

        public async Task<CustomResponseDto<NoContentDto>> ChangeRoleAsync(string userId, string newRole)
        {
            if (newRole != UserType.Standart.ToString() && newRole != UserType.Premium.ToString())
            {
                return CustomResponseDto<NoContentDto>.Failure(400, "Hatalı role ataması");
            }
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if(currentRoles.First() == newRole)
                    {
                        return CustomResponseDto<NoContentDto>.Failure(400, "Hatalı role ataması");

                    }
                    var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, newRole);

                        int Id = (int)user.Id;
                        _quotaRepository.QuotaUpdate(Id, (int)UserQuota.Standart, newRole);

                        _unitOfWork.CommitAsync();
                        return CustomResponseDto<NoContentDto>.Success(204);
                    }
                    else
                    {
                        // Handle error
                        return CustomResponseDto<NoContentDto>.Failure(500, "ErrorAction");
                    }
                }
                else
                {
                    return CustomResponseDto<NoContentDto>.Failure(404, "User Not Found");

                }
            }
            catch (Exception ex)
            {

                return CustomResponseDto<NoContentDto>.Failure(500, ex.Message);
            }
        }

        public async Task<CustomResponseDto<AppUserWithRoleModel>> CreateUserAsync(SignInModel model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,

            };
            var result = await _userManager.CreateAsync(user, model.Password.Trim());
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponseDto<AppUserWithRoleModel>.Failure(400, errors);
            }
            var type = model.UserType;

            if (!_roleManager.RoleExistsAsync($"{type}").Result)
            {
                AppRole role = new AppRole()
                {
                    Name = $"{type}"
                };
                IdentityResult roleResult = await _roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, $"{type}");
                }
            }

            int storageSpaceByte = 0;

            if (type == UserType.Standart && type==null)
            {
                storageSpaceByte = UserQuota.Standart;
            }
            else
            {
                storageSpaceByte = UserQuota.Premium;
            }
            var quota = new Quota()
            {
                Id = user.Id,
                UsedSpaceByte = 0,
                StorageSpaceByte = storageSpaceByte,
            };
            _quotaRepository.AddAsync(quota);
            _unitOfWork.CommitAsync();


            var mapping = new AppUserWithRoleModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserType = type,
            };
            return CustomResponseDto<AppUserWithRoleModel>.Success(201, mapping);
        }

        public async Task<CustomResponseDto<IEnumerable<AppUserWithRoleModel>>> GetAllAsync()
        {
            var appRoleModels = new List<AppUserWithRoleModel>();
            var userList = await _userManager.Users.ToListAsync();
            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    var appRoleModel = new AppUserWithRoleModel
                    {
                        Id=user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        UserType=roleName,


                    };
                    appRoleModels.Add(appRoleModel);
                }

            }
            return CustomResponseDto<IEnumerable<AppUserWithRoleModel>>.Success(200, appRoleModels);
        }

        public async Task<CustomResponseDto<AppUserWithRoleModel>> GetByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var role = await _userManager.GetRolesAsync(user);
            string roleName = "";
            foreach(var item in role)
            {
                roleName = item;
            }
            var appRoleModel = new AppUserWithRoleModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserType = roleName,

            };
            return CustomResponseDto<AppUserWithRoleModel>.Success(200, appRoleModel);
        }

        public async Task<CustomResponseDto<AppUserWithRoleModel>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var role = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return CustomResponseDto<AppUserWithRoleModel>.Failure(404, "UserName bulunamadı");
            }
            var mapping = new AppUserWithRoleModel()
            {
                Id = user.Id,
                UserName = userName,
                Email = userName,
                UserType = role.FirstOrDefault()
            };

            return CustomResponseDto<AppUserWithRoleModel>.Success(200, mapping);
        }
    }
}
