using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.Data
{
    class ComboBoxEnumWithDescription
    {
        private string text;
        public object e { get; private set; }
        public ComboBoxEnumWithDescription(object e)
        {
            this.e = e;
            FieldInfo fi = e.GetType().GetField(e.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    text = ((DescriptionAttribute)attrs[0]).Description;
                    return;
                }
            }
            text = null;
        }

        public override string ToString()
        {
            if (text != null) return text;
            return base.ToString();
        }
    }
}
