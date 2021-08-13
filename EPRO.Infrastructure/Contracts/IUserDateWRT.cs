using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.Contracts
{
    public interface IUserDateWRT
    {
        string UserId { get; set; }
        DateTime DateWrt { get; set; }
    }
}
