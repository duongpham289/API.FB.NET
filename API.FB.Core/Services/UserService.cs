using API.FB.Core.Entities;
using API.FB.Core.Interfaces.Repository;
using API.FB.Core.Interfaces.Services;
using API.FB.Core.FBAttribute;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.FB.Core.Services
{
    public class UserService : IUserService
    {
        #region DECLARE

        IUserRepository _userRepository; 
        protected ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Xuất dữ liệu dưới dạng Excel
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userFilter"></param>
        /// <param name="check">Biến check export hay paging (true - paging, false - export)</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(03/09/2021)
        public dynamic ExportUser(int pageIndex, int pageSize, string userFilter, bool check)
        {
            var stream = new MemoryStream();
            var users = _userRepository.GetPaging(pageIndex, pageSize, userFilter, check);

            var genderList = new List<string> { "Nữ", "Nam", "Khác", string.Empty };

            var properties = typeof(User).GetProperties();
            using (var package = new ExcelPackage(stream))
            {

                var workSheet = package.Workbook.Worksheets.Add("Danh Sách Nhân Viên");

                // Chình tiêu đề trong bảng.

                // STT
                workSheet.Cells[3, 1].Value = "STT";
                workSheet.Cells[3, 1].Style.Font.Bold = true;
                workSheet.Cells[3, 1].Style.Fill.SetBackground(Color.LightGray);
                workSheet.Cells[3, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);


                var column = 2;

                foreach (var prop in properties)
                {
                    var propMISAExport = prop.GetCustomAttributes(typeof(MISAPropExport), true);

                    //Xét các trường có được export không?
                    if (propMISAExport.Length == 1)
                    {

                        // định dạng ngày/tháng/năm
                        if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                        {
                            workSheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            workSheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        workSheet.Cells[3, column].Value = (propMISAExport[0] as MISAPropExport).Name;
                        workSheet.Cells[3, column].Style.Font.Bold = true;
                        workSheet.Cells[3, column].Style.Fill.SetBackground(Color.LightGray);
                        workSheet.Cells[3, column].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                        column++;
                    }
                }

                // Chỉnh bản ghi vào hàng, cell
                for (int i = 0; i < users.Count; i++)
                {
                    workSheet.Cells[i + 4, 1].Value = i + 1;
                    workSheet.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                    int col = 2;

                    foreach (var prop in properties)
                    {
                        var propMISAExport = prop.GetCustomAttributes(typeof(MISAPropExport), true);

                        //Xét các trường có được export không?
                        if (propMISAExport.Length == 1)
                        {

                            if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                            {
                                var tmp = users[i].GetType().GetProperty(prop.Name).GetValue(users[i], null);
                                workSheet.Cells[i + 4, col].Value = tmp == null ? "" : Convert.ToDateTime(tmp).ToString("dd/MM/yyyy");
                            }
                            else if ((propMISAExport[0] as MISAPropExport).Name == "Giới tính")
                            {
                                var genderName = users[i].GetType().GetProperty(prop.Name).GetValue(users[i], null);
                                workSheet.Cells[i + 4, col].Value = genderList[genderName != null ? (int)genderName : 3];
                            }
                            else
                            {
                                workSheet.Cells[i + 4, col].Value = users[i].GetType().GetProperty(prop.Name).GetValue(users[i], null);
                            }

                            workSheet.Cells.AutoFitColumns();
                            workSheet.Cells[i + 4, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                            col++;
                        }
                    }
                }

                // Chỉnh tiêu đề cho workSheet
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;
                workSheet.Cells[1, 1].Value = "DANH SÁCH NHÂN VIÊN";
                workSheet.Cells[1, 1].Style.Font.Size = 16;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                package.Save();
            }

            stream.Position = 0;
            return stream;
        }
        #endregion

        #region ValidateData

        ///// <summary>
        ///// Validate dữ liệu
        ///// </summary>
        ///// <param name="customer">Dữ liệu cần validate</param>
        ///// <returns></returns>
        ///// CreatedBy: PHDUONG(27/08/2021)
        //protected bool ValidateCustom(User user)
        //{
        //    //Kiểm tra thông tin của nhân viên đã hợp lệ chưa
        //    //1.Mã nhân viên ko được phép để trống
        //    if (user.UserCode == "" || user.UserCode == null)
        //    {
        //        var errorObj = new
        //        {
        //            userMsg = Resources.ResourceVN.EmployeeCodeValidateError_Msg,
        //            errorCode = "misa-001",
        //            moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
        //            traceId = ""
        //        };
        //        _serviceResult.IsValid = false;
        //        _serviceResult.Data = errorObj;
        //        return _serviceResult.IsValid;
        //    }

        //    //2. Mã nhân viên không được phép trùng
        //    if (user.UserId == Guid.Empty && _userRepository.IsDuplicated(user.UserCode))
        //    {
        //        var errorObj = new
        //        {
        //            userMsg = Resources.ResourceVN.EmployeeCodeDuplicateValidateError_Msg,
        //            errorCode = "misa-001",
        //            moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
        //            traceId = ""
        //        };
        //        _serviceResult.IsValid = false;
        //        _serviceResult.Data = errorObj;
        //        return _serviceResult.IsValid;
        //    }

        //    //3.Đơn vị ko được phép để trống
        //    if (user.DepartmentId == null && user.DepartmentName == null)
        //    {
        //        var errorObj = new
        //        {
        //            userMsg = Resources.ResourceVN.DepartmentEmptyError_Msg,
        //            errorCode = "misa-001",
        //            moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
        //            traceId = ""
        //        };
        //        _serviceResult.IsValid = false;
        //        _serviceResult.Data = errorObj;
        //        return _serviceResult.IsValid;
        //    }

        //    return _serviceResult.IsValid;

        //}
        #endregion
    }
}
