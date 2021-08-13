using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.Constants
{
    /// <summary>
    /// Константи за управление на потребители
    /// </summary>
    public class AccountConstants
    {
        /// <summary>
        /// Роли в системата
        /// </summary>
        public class Roles
        {
            /// <summary>
            /// Редактор
            /// </summary>
            public const string EDIT = "EDIT";

            /// <summary>
            /// Локален администратор
            /// </summary>
            public const string ADMIN = "ADMIN";

            /// <summary>
            /// Глобален администратор
            /// </summary>
            public const string GLOBAL_ADMIN = "GLOBAL_ADMIN";
        }
    }
}
