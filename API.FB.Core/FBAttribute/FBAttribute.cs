using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.FBAttribute
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

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
<<<<<<< Updated upstream
=======

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {_maxFileSize} bytes.";
        }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This extension is not allowed!";
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
>>>>>>> Stashed changes
    }


    /// <summary>
    /// Cờ Khóa chính
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method)]
    public class PrimaryKey : Attribute
    {
    }

    /// <summary>
    /// Cờ ngày tạo
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method)]
    public class CreateDate : Attribute
    {
    }

    /// <summary>
    /// Cờ ngày sửa
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method)]
    public class ModifiedDate : Attribute
    {
    }
}
