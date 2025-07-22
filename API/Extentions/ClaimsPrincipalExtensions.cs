using System;
using System.Security.Claims;

namespace API.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static string GetMemberId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Cannot get memberId for Token");
    }
}
