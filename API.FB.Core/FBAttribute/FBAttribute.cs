using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.MISAAttribute
{

    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute
    {
        public string _fieldName = string.Empty;
        public string _message = string.Empty;

        public MISARequired(string fieldName)
        {
            _message = $"Thông tin {fieldName} không được để trống!";
            _fieldName = fieldName;

        }
    }

    /// <summary>
    /// Thuộc tính xuất file excel
    /// </summary>
    /// CreatedBy: PHDUONG (03/09/2021)
    public class MISAPropExport : Attribute
    {
        public readonly string Name;
        public MISAPropExport(string name)
        {
            this.Name = name;
        }
    }
}
