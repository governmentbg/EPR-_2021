using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.Contracts
{
    public interface IExpiredInfo
    {
        string UserExpiredId { get; set; }
        DateTime? DateExpired { get; set; }
        string DescriptionExpired { get; set; }
    }
}
