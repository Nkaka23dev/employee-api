using TheEmployeeAPI.Domain.Enums;

namespace TheEmployeeAPI.Domain.DTOs.Authentication;

public static class UserRoles
{
    public const string Admin = nameof(UserRoleEnum.Admin);
    public const string Manager = nameof(UserRoleEnum.Manager);
    public const string Employee = nameof(UserRoleEnum.Employee);

    public static List<string> All => [Admin, Manager, Employee];
}