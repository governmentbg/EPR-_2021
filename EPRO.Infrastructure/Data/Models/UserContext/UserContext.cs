using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace EPRO.Infrastructure.Data.Models.UserContext
{
    public class UserContext : IUserContext
    {
        private ClaimsPrincipal User;

        public UserContext(IHttpContextAccessor _ca)
        {
            User = _ca.HttpContext.User;
        }


        public string UserId
        {
            get
            {
                string userId = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var subClaim = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                    if (subClaim != null)
                    {
                        userId = subClaim.Value;
                    }
                }

                return userId;
            }
        }
        public string Email
        {
            get
            {
                string email = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {

                    var claimEmail = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Name);

                    email = claimEmail?.Value;
                }

                return email;
            }
        }
       
        public string FullName
        {
            get
            {
                string fullName = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var subClaim = User.Claims
                        .FirstOrDefault(c => c.Type == CustomClaimType.FullName);

                    if (subClaim != null)
                    {
                        fullName = subClaim.Value;
                    }
                }

                return fullName;
            }
        }



        public int? CourtId
        {
            get
            {

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var _claimValue = User.Claims
                        .Where(c => c.Type == CustomClaimType.Court)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    if (_claimValue == "null")
                    {
                        return null;
                    }
                    if (_claimValue != null)
                    {
                        return int.Parse(_claimValue);
                    }
                }

                return null;
            }
        }

        public bool IsUserInRole(string role)
        {
            return User.IsInRole(role);
        }

    }
}

