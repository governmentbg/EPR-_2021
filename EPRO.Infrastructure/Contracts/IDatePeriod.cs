using System;

namespace EPRO.Infrastructure.Contracts
{
    /// <summary>
    /// Author: Ivaylo Dimitrov
    /// Created: 09.01.2020
    /// Description: Интерфейс за достъп до полетата 
    /// с начална и крайна дата
    /// </summary>
    public interface IDatePeriod
    {
        /// <summary>
        /// Начало на периода на валидност
        /// </summary>
        DateTime DateStart { get; set; }

        /// <summary>
        /// Край на периода на валидност
        /// Ако е NULL, е валидна след начална дата
        /// </summary>
        DateTime? DateEnd { get; set; }
    }
}
