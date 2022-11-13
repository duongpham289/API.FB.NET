using Dapper;
using Microsoft.Extensions.Configuration;
using API.FB.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Infrastructure.Repository
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region DECLARE

        protected IConfiguration _configuration;
        protected IDbConnection _dbConnection;
        string _className;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            _className = typeof(MISAEntity).Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách Thực thể từ DataBase
        /// </summary>
        /// <returns>List Thực thể</returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public List<MISAEntity> GetAll()
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var entities = _dbConnection.Query<MISAEntity>($"Proc_Get{_className}s", commandType: CommandType.StoredProcedure);

                return entities.AsList();
            }
        }

        /// <summary>
        /// Lấy danh sách Thực thể từ DataBase
        /// </summary>
        /// <returns>List Thực thể</returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public MISAEntity GetById(Guid entityId)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_className}Id", entityId);
                var entities = _dbConnection.QueryFirstOrDefault<MISAEntity>($"Proc_Get{_className}ById", param: parameters, commandType: CommandType.StoredProcedure);

                return entities;
            }
        }

        /// <summary>
        /// Thêm mới Thực thể vào DataBase
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public int Add(MISAEntity entity)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                var dynamicParam = new DynamicParameters();

                //Đọc từng prop của obj:
                var properties = entity.GetType().GetProperties();

                //Duyệt từng prop:
                foreach (var prop in properties)
                {
                    var propName = prop.Name;

                    var propValue = prop.GetValue(entity);

                    dynamicParam.Add($"@{propName}", propValue);
                }
                var rowsEffect = _dbConnection.Execute($"Proc_Insert{_className}", param: dynamicParam, commandType: CommandType.StoredProcedure);

                return rowsEffect;
            }
        }

        /// <summary>
        /// Sửa Thực thể từ DataBase
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public int Update(MISAEntity entity, Guid entityId)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {

                var dynamicParam = new DynamicParameters();


                //Đọc từng prop của obj:
                var properties = entity.GetType().GetProperties();

                //Duyệt từng prop:
                foreach (var prop in properties)
                {
                    var propName = prop.Name;

                    var propValue = prop.GetValue(entity);

                    dynamicParam.Add($"@{propName}", propValue);

                }

                var rowsEffect = _dbConnection.Execute($"Proc_Update{_className}", param: dynamicParam, commandType: CommandType.StoredProcedure);

                return rowsEffect;
            }
        }

        /// <summary>
        /// Xóa Thực thể từ DataBase
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public int Delete(Guid entityId)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_className}Id", entityId);
                var rowsEffect = _dbConnection.Execute($"Proc_Delete{_className}ById", param: parameters, commandType: CommandType.StoredProcedure);

                return rowsEffect;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId">Danh sách Id</param>
        /// <returns></returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public int DeleteList(List<Guid> listId)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                _dbConnection.Open();
                var transaction = _dbConnection.BeginTransaction();

                var rowsEffect = 0;
                foreach (var id in listId)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add($"@{_className}Id", id.ToString());
                    rowsEffect += _dbConnection.Execute($"Proc_Delete{_className}ById", param: parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                }

                transaction.Commit();

                if (rowsEffect == listId.Count)
                    return rowsEffect;
                else
                    return 0;

            }
        }

        /// <summary>
        /// Check trùng code
        /// </summary>
        /// <param name="entityProperty">Thuộc tính thực thể</param>
        /// <returns>true - Có mã trùng, false - Ko có mã trùng</returns>
        /// CreatedBy: PHDUONG(27/08/2021)
        public bool IsDuplicated(string entityProperty)
        {
            using (_dbConnection = new MySqlConnection(_configuration.GetConnectionString("SqlConnection")))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_className}Property", entityProperty != null ? entityProperty : String.Empty);

                parameters.Add("@IsExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                _dbConnection.Execute($"Proc_Check{_className}PropertyDuplicate", param: parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<Boolean>("@IsExist");
            }
        }
        #endregion
    }
}
