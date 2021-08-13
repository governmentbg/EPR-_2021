using System;

namespace EPRO.Components
{
    public class MenuItemAttribute : Attribute
    {
        public string Value { get; set; }

        public MenuItemAttribute(string value)
        {
            this.Value = value;
        }
    }
}
