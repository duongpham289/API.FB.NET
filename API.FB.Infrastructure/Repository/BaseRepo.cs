using API.FB.Core.FBAttribute;
using API.FB.Core.Interfaces.Repository;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Infrastructure.Repository
{
    public class BaseRepo<T> : IBaseRepo<T>
    {
        /// <summary>
        /// Xử lí kết nối với database 
        /// </summary>
        IConfiguration _configuration;
        readonly string _connectionString = string.Empty;
        protected MySqlConnection _sqlConnection;
        string _tableName;
        public BaseRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            // Khai báo thông tin database:
            _connectionString = _configuration.GetConnectionString("SqlConnection");
            // Khỏi tạo kết nối đến database --> sử dụng mySqlConnector
            _sqlConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(T).Name.ToLower();
        }
        public List<T> Get()
        {
            // Thực hiện khai báo câu lệnh truy vấn SQL:
            var sqlCommand = $"SELECT * FROM {_tableName} ORDER BY CreatedDate DESC";

            // Thực hiện câu truy vấn:
            var entities = _sqlConnection.Query<T>(sqlCommand);

            // Trả về dữ liệu dạng List:
            return entities.ToList();
        }

        public T GetById(Guid entityId)
        {
            // Thực hiện khai báo câu lệnh truy vấn SQL:
            var sqlQuery = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @entityId";
            var parameters = new DynamicParameters();
            parameters.Add("@entityId", entityId);

            // Thực hiện câu truy vấn:
            var entities = _sqlConnection.QueryFirstOrDefault<T>(sqlQuery, param: parameters);

            // Trả về dữ liệu dạng List:
            return entities;
        }

        public int Insert(T entity)
        {
            // Khỏi tạo câu lệnh 
            var columnNames = "";
            var columnParams = "";
            // Lấy ra tất cả các properties của class:
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                // Tên của prop:
                var propName = prop.Name;
                // Giá trị của prop:
                var propValue = prop.GetValue(entity);
                // Kiểu dữ liệu của prop:
                var propType = prop.PropertyType;
                // Kiểm tra prop hiện tại có phải là khóa chính hay không, nếu đúng thì gán lại giá trị mới cho prop:
                var isPrimarykey = prop.IsDefined(typeof(PrimaryKey), true);
                if (isPrimarykey == true && propType == typeof(Guid) && (Guid)propValue == Guid.Empty)
                {
                    prop.SetValue(entity, Guid.NewGuid());
                }
                // Kiểm tra prop hiện tại có phải là Ngày tạo hay không, nếu đúng thì gán lại giá trị mới cho prop:
                var isCreateDate = prop.IsDefined(typeof(CreateDate), true);
                if (isCreateDate == true)
                {
                    prop.SetValue(entity, DateTime.Now);
                }
                // Bồ sung cột hiện tại vào chuỗi câu truy vấn cột dữ liệu:
                columnNames += $" {propName},";
                columnParams += $"@{propName},";
            }
            // Xóa dấu phẩy cuối cùng của chuỗi
            columnNames = columnNames.Remove(columnNames.Length - 1, 1);
            columnParams = columnParams.Remove(columnParams.Length - 1, 1);
            var sqlCommand = $"INSERT INTO {_tableName}({columnNames}) VALUES ({columnParams})";
            var rowAffects = _sqlConnection.Execute(sqlCommand, param: entity);
            return rowAffects;
        }

        public int Update(Guid entityId, T entity)
        {
            var setParams = "";
            var parameter = new DynamicParameters();
            // Lấy ra tất cả các properties của class:
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                // Tên của prop:
                var propName = prop.Name;
                // Giá trị của prop:
                var propValue = prop.GetValue(entity);
                // Kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                // Kiểm tra prop hiện tại có phải là khóa chính hay không, nếu đúng thì gán lại giá trị cho prop:
                var isPrimarykey = prop.IsDefined(typeof(PrimaryKey), true);
                if (isPrimarykey == true && propType == typeof(Guid))
                {
                    prop.SetValue(entity, entityId);
                }

                // Kiểm tra prop hiện tại có phải là Ngày sửa hay không, nếu đúng thì gán lại giá trị mới cho prop:
                var isModifiedDate = prop.IsDefined(typeof(ModifiedDate), true);
                if (isModifiedDate == true)
                {
                    prop.SetValue(entity, DateTime.Now);
                }
                setParams += $"{propName} = @{propName},";
                parameter.Add($"@{propName}", propValue);
            }
            // Xóa dấu phẩy cuối cùng của chuỗi
            setParams = setParams.Remove(setParams.Length - 1, 1);
            var sqlCommand = $"UPDATE {_tableName} SET {setParams} WHERE  {_tableName}Id = @entityId";
            parameter.Add("@entityId", entityId);
            var rowAffects = _sqlConnection.Execute(sqlCommand, param: parameter);
            return rowAffects;
        }

        public int Delete(Guid entityId)
        {
            // Khỏi tạo câu truy vấn:
            var sqlQuery = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @entityId";
            var parameters = new DynamicParameters();
            parameters.Add("@entityId", entityId);
            // Thực hiện câu truy vấn: 
            var res = _sqlConnection.Execute(sqlQuery, param: parameters);
            return res;
        }

    }
}
