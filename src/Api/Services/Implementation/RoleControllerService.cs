using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using Domain.Roles;
using Infrastructure.Persistence.Repositories;
using LanguageExt;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Implementation
{
    public class RoleControllerService(IRoleRepository roleRepository) : IRoleControllerService
    {
        public async Task<Option<RoleDto>> Get(Guid roleId, CancellationToken cancellationToken)
        {
            var entity = await roleRepository.GetByIdAsync(new RoleId(roleId), cancellationToken);
            return entity.Match(
                r => RoleDto.FromDomainModel(r),
                () => Option<RoleDto>.None);
        }
    }
}
