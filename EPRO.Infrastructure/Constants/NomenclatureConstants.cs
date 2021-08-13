namespace EPRO.Infrastructure.Constants
{
    public static class NomenclatureConstants
    {
        public const string AssemblyQualifiedName = "EPRO.Infrastructure.Data.Models.Nomenclatures.{0}, EPRO.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        /// <summary>
        /// Вид запис на отвод
        /// </summary>
        public class EntryType
        {
            /// <summary>
            /// Въведен през Rest.API услуги
            /// </summary>
            public const int API = 1;

            /// <summary>
            /// Въведен от потребител през административен панел
            /// </summary>
            public const int User = 2;
        }

        /// <summary>
        /// Видове отводи
        /// </summary>
        public class DismissalTypes
        {
            /// <summary>
            /// Отвод
            /// </summary>
            public const int Otvod = 1;

            /// <summary>
            /// Самоотвод
            /// </summary>
            public const int SamoOtvod = 2;
        }

        /// <summary>
        /// Тип обект за присъединяване на файлово съдържание
        /// </summary>
        public class SourceType
        {
            /// <summary>
            /// Отводи
            /// </summary>
            public const int Dismissal = 1;
        }

        /// <summary>
        /// Стойности за видове операции в журнал на промените
        /// </summary>
        public class AuditOperations
        {
            public const string Add = "Добавяне";
            public const string Edit = "Редакция";
            public const string Patch = "Актуализация";
            public const string Delete = "Изтриване";
            public const string View = "Преглед";
        }
    }
}
