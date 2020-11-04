namespace mpPrToTable
{
    using System;
    using System.Collections.Generic;
    using ModPlusAPI.Abstractions;
    using ModPlusAPI.Enums;

    /// <inheritdoc/>
    public class ModPlusConnector : IModPlusPlugin
    {
        /// <inheritdoc/>
        public SupportedProduct SupportedProduct => SupportedProduct.AutoCAD;

        /// <inheritdoc/>
        public string Name => "mpPrToTable";

#if A2013
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2013";
#elif A2014
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2014";
#elif A2015
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2015";
#elif A2016
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2016";
#elif A2017
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2017";
#elif A2018
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2018";
#elif A2019
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2019";
#elif A2020
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2020";
#elif A2021
        /// <inheritdoc/>
        public string AvailProductExternalVersion => "2021";
#endif

        /// <inheritdoc/>
        public string FullClassName => string.Empty;

        /// <inheritdoc/>
        public string AppFullClassName => string.Empty;

        /// <inheritdoc/>
        public Guid AddInId => Guid.Empty;

        /// <inheritdoc/>
        public string LName => "Изделия в таблицу";

        /// <inheritdoc/>
        public string Description => "Плагин позволяет заполнить таблицу спецификации выбранными изделиями ModPlus";

        /// <inheritdoc/>
        public string Author => "Пекшев Александр aka Modis";

        /// <inheritdoc/>
        public string Price => "0";

        /// <inheritdoc/>
        public bool CanAddToRibbon => true;

        /// <inheritdoc/>
        public string FullDescription => "Плагин собирает данные для спецификации из расширенных данных продуктов (блоки, созданные плагином \"Вставить изделие\", или примитивы AutoCAD с расширенными данными, добавленными плагином \"Вставить изделие\"), а также из блоков, имеющих атрибуты для спецификации. Имеется возможность указать строку, с которой начнется заполнение таблицы";

        /// <inheritdoc/>
        public string ToolTipHelpImage => string.Empty;

        /// <inheritdoc/>
        public List<string> SubPluginsNames => new List<string>();

        /// <inheritdoc/>
        public List<string> SubPluginsLNames => new List<string>();

        /// <inheritdoc/>
        public List<string> SubDescriptions => new List<string>();

        /// <inheritdoc/>
        public List<string> SubFullDescriptions => new List<string>();

        /// <inheritdoc/>
        public List<string> SubHelpImages => new List<string>();

        /// <inheritdoc/>
        public List<string> SubClassNames => new List<string>();
    }
}