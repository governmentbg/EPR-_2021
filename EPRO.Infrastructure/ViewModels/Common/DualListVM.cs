using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Common
{
    public class DualListVM
    {
        public string ParentId { get; set; }
        public int ParentIntId { get { return int.Parse(this.ParentId); } }

        public string ParentDescription { get; set; }

        public string ElementsLabel { get; set; }
        public string BackUrl { get; set; }

        public ICollection<SelectListItem> Items { get; set; }

        public string[] SelectedItems { get; set; }

        public void UpdateNullables()
        {
            SelectedItems = SelectedItems ?? new string[] { };
        }
    }
}
