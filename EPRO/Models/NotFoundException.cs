using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Models
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Търсения от Вас обект не беше намерен.") : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        //public NotFoundException()
        //{
        //}
    }
}
