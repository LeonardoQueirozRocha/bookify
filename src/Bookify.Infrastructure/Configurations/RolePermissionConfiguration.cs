using Bookify.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(rulePermission =>
            new
            {
                rulePermission.RoleId,
                rulePermission.PermissionId
            });

        builder.HasData(
            new RolePermission
            {
                RoleId = Role.Registered.Id,
                PermissionId = Permission.UserRead.Id
            });
    }
}