using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.Contracts
{
    /// <summary>
    /// Текущ контекст на изпълнение на операциите
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Идентификатор на потребител
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Идентификатор на съд, ако е за всички съдилища е null
        /// </summary>
        int? CourtId { get; }

        /// <summary>
        /// Електронна поща на потребител
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Имена на потребител
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Проверка за налична роля на потребител
        /// </summary>
        /// <param name="role">Наименование на роля</param>
        /// <returns></returns>
        bool IsUserInRole(string role);

    }
}
