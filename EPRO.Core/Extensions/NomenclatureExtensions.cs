using EPRO.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EPRO.Core.Extensions
{
    public static class NomenclatureExtensions
    {
        /// <summary>
        /// Creates SelectList from IQueryable<ICommonNomenclature>
        /// </summary>
        /// <param name="model">Common Nomenclature list of entities</param>
        /// <param name="addDefaultElement">Add 'Choose' to list</param>
        /// <paramref name="addAllElement">Add 'All' to list</param>
        /// <paramref name="orderByNumber">Order by order number</param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectList(this IQueryable<ICommonNomenclature> model, bool addDefaultElement = false, bool addAllElement = false, bool orderByNumber = true)
        {
            DateTime today = DateTime.Today;

            Expression<Func<ICommonNomenclature, object>> order = x => x.OrderNumber;
            if (!orderByNumber)
            {
                order = x => x.Label;
            }

            var result = model
                .Where(x => x.IsActive)
                .Where(x => x.DateStart <= today)
                .Where(x => (x.DateEnd ?? today) >= today)
                .OrderBy(order)
                .Select(x => new SelectListItem()
                {
                    Text = x.Label,
                    Value = x.Id.ToString()
                }).ToList() ?? new List<SelectListItem>();

            if (addDefaultElement)
            {
                result = result
                    .Prepend(new SelectListItem() { Text = "Избери", Value = null })
                    .ToList();
            }

            if (addAllElement)
            {
                result = result
                    .Prepend(new SelectListItem() { Text = "Всички", Value = "-2" })
                    .ToList();
            }

            return result;
        }

        public static string GetIOReqClass(this Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata model)
        {
            string result = "";

            try
            {
                if (model.ContainerType != null)
                {
                    var Member = model.ContainerType.GetMember(model.PropertyName);
                    var reqTypes = new[] { typeof(RequiredAttribute) };
                    var hasIOreq = Member[0].CustomAttributes.Any(a => reqTypes.Contains(a.AttributeType));
                    if (hasIOreq)
                    {
                        result = "io-req";
                    }
                }
            }
            catch { }

            return result;
        }
    }
}
