namespace mpPrToTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Autodesk.AutoCAD.DatabaseServices;
    using ModPlus;
    using ModPlusAPI.Windows;
    using mpProductInt;

    public partial class FindProductsProgress
    {
        private readonly ObjectId[] _objectIds;
        private readonly Transaction _tr;
        private SynchronizationContext _context;

        public List<mpProductInt.Specification.SpecificationItem> SpecificationItems;

        public FindProductsProgress(ObjectId[] objectIds, Transaction tr)
        {
            InitializeComponent();
            _objectIds = objectIds;
            _tr = tr;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = objectIds.Length;
            ProgressBar.Value = 0;
            ContentRendered += FindProductsProgress_ContentRendered;
        }

        private async void FindProductsProgress_ContentRendered(object sender, EventArgs e)
        {
            _context = SynchronizationContext.Current;

            var task = new Task(FindProducts);
            task.Start();
            await task;
            DialogResult = true;
            Close();
        }

        private void FindProducts()
        {
            try
            {
                SpecificationItems = new List<mpProductInt.Specification.SpecificationItem>();

                var products = new List<DbProduct>();
                var productsByAttr = new List<mpProductInt.Specification.SpecificationItem>();

                var counts = new List<int>();
                var countsByAttr = new List<int>();

                for (var i = 0; i < _objectIds.Length; i++)
                {
                    // Проверяем - если это блок и он имеет атрибуты для спецификации
                    if (ProductsToTable.HasAttributesForSpecification(_tr, _objectIds[i]))
                    {
                        // post progress
                        _context.Post(
                            _ =>
                        {
                            ProgressBar.Value = i;
                            ProgressText.Text = $"{i}/{_objectIds.Length}";
                        }, null);

                        var specificationItemByBlockAttributes =
                            ProductsToTable.GetProductFromBlockByAttributes(_tr, _objectIds[i]);
                        if (specificationItemByBlockAttributes != null)
                        {
                            if (!productsByAttr.Any(p => p.Equals(specificationItemByBlockAttributes)))
                            {
                                productsByAttr.Add(specificationItemByBlockAttributes);
                                countsByAttr.Add(1);
                            }
                            else
                            {
                                for (var j = 0; j < productsByAttr.Count; j++)
                                {
                                    if (productsByAttr[j].Equals(specificationItemByBlockAttributes))
                                        countsByAttr[j]++;
                                }
                            }
                        }
                    }
                    else //// Иначе пробуем читать из расширенных данных
                    {
                        var entity = (Entity) _tr.GetObject(_objectIds[i], OpenMode.ForRead);
                        if (!entity.IsModPlusProduct()) 
                            continue;

                        // post progress
                        _context.Post(
                            _ =>
                        {
                            ProgressBar.Value = i;
                            ProgressText.Text = $"{i}/{_objectIds.Length}";
                        }, null);

                        if (XDataHelpersForProducts.NewFromEntity(entity) is MpProductToSave mpProductToSave)
                        {
                            var productFromSaved = DbProduct.GetProductFromSaved(mpProductToSave);
                            if (productFromSaved != null)
                            {
                                if (!products.Contains(productFromSaved))
                                {
                                    products.Add(productFromSaved);
                                    counts.Add(1);
                                }
                                else
                                {
                                    var index = products.IndexOf(productFromSaved);
                                    counts[index]++;
                                }
                            }
                        }
                    }
                }

                if (!products.Any() & !productsByAttr.Any())
                {
                    DialogResult = false;
                    Close();
                }

                // Для продуктов собранных из атрибутов вставляем количество
                for (var i = 0; i < productsByAttr.Count; i++)
                {
                    productsByAttr[i].Count = countsByAttr[i].ToString();
                }

                // Добавляем продукты собранные из расширенных данных
                for (var j = 0; j < products.Count; j++)
                {
                    var specificationItem = products[j].GetSpecificationItem(counts[j]);
                    SpecificationItems.Add(specificationItem);
                }

                // Добавляем продукты, собранные из атрибутов
                foreach (var specificationItem in productsByAttr)
                {
                    SpecificationItems.Add(specificationItem);
                }

                // Сортировка по значению Позиции
                SpecificationItems.Sort(new mpProductInt.Specification.AlphanumComparatorFastToSortByPosition());
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
