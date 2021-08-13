namespace EPRO.Infrastructure.Contracts
{
    /// <summary>
    /// Author: Ivaylo Dimitrov
    /// Created: 09.01.2020
    /// Description: Интерфейс за достъп до полетата 
    /// на общи номенклатури без начална и крайна дата
    /// </summary>
    public interface IBaseNomenclature
    {
        /// <summary>
        /// Идентификатор на запис
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Код на номенклатурата
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Етикет на номенклатурата
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Описание на номенклатурата
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Пореден номер
        /// </summary>
        int OrderNumber { get; set; }

        /// <summary>
        /// Дали записа е активен
        /// </summary>
        bool IsActive { get; set; }
    }
}
