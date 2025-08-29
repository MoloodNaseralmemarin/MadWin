using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Permissions;
using MadWin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shop2City.Core.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        private MadWinDBContext _context;
        private readonly ILogger<PermissionService> _logger;
        public PermissionService(MadWinDBContext context, ILogger<PermissionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddPermissionsToRole(int roleId, List<int> permissions)
        {
           foreach(var permission in permissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    permissionId = permission,
                    roleId = roleId
                });
            }
            _context.SaveChanges();
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.roleId;
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    roleId=roleId,
                    userId=userId
                });
            }
            _context.SaveChanges();
        }

        public async Task<bool> CheckPermissionAsync(int permissionId, int userId)
        {


            List<int> UserRoles = await _context.UserRoles
                .Where(r => r.userId == userId)
                .Select(r => r.roleId)
                .ToListAsync();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _context.RolePermissions
                .Where(p => p.permissionId == permissionId)
                .Select(p => p.roleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }
        

        public bool CheckPermission(int permissionId)
        {
            List<int> UserRoles = _context.UserRoles
                .Select(r => r.roleId)
                .ToList();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _context.RolePermissions
                .Where(p => p.permissionId == permissionId)
                .Select(p => p.roleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public void DeleteRole(Role role)
        {
            UpdateRole(role);
        }

        public void EditRolesToUser(int userId, List<int> roleIds)
        {
            #region Delete All Roles User
            _context.UserRoles
               .Where(ur => ur.userId == userId)
               .ToList()
               .ForEach(ur => _context.UserRoles.Remove(ur));
            #endregion
            #region Add New Roles User
            AddRolesToUser(roleIds, userId);
            #endregion

        }

        public List<Permission> GetPermissions()
        {
            return _context.Permissions.ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);

        }
        public List<Role> GetRoles()
        {
            return _context
                .Roles
                .ToList();
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _context.RolePermissions
                .Where(rp => rp.roleId == roleId)
                .Select(rp => rp.permissionId)
                .ToList();
        }

        public void UpdatePermissionsToRole(int roleId, List<int> permissions)
        {
            _context.RolePermissions
                .Where(rp => rp.roleId == roleId)
                .ToList()
                .ForEach(rp => _context.RolePermissions
                .Remove(rp));

            AddPermissionsToRole(roleId, permissions);

        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }
    }
}
