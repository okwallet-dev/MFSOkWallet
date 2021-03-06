﻿using Dapper;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IApplicationUserRepository : IBaseRepository<ApplicationUser>
    {
        ApplicationUser validateLogin(string userName, string password);
        string GetTransAmtLimit(string createUser);
        object IsProceedToController(List<string> userInfos);
        object GetAppUserListDdl();
        object GetAllApplicationUserList(string roleName);
        PasswordPolicy GetPasswordPolicy();
    }

    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        //private static string dbUser;
        //public ApplicationUserRepository(MainDbUser objMainDbUser)
        //{
        //    dbUser = objMainDbUser.DbUser;
        //}
        MainDbUser mainDbUser = new MainDbUser();

        public ApplicationUser validateLogin(string userName, string password)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("UNAME", OracleDbType.Varchar2, ParameterDirection.Input, userName);
                    dyParam.Add("PWD", OracleDbType.Varchar2, ParameterDirection.Input, password);
                    dyParam.Add("LOGIN_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

                    IList<ApplicationUser> result = SqlMapper.Query<ApplicationUser>(conn, mainDbUser.DbUser + "PR_MFS_VALIDATELOGIN", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(conn);
                    if (result.Count == 0)
                    {
                        ApplicationUser obj = conn.QueryFirstOrDefault<ApplicationUser>("Select " + this.GetCamelCaseColumnList(new ApplicationUser()) + " from " + mainDbUser.DbUser + "Application_User where username='" + userName + "'");
                        obj.Is_validated = false;
                        return obj;
                    }
                    else
                    {
                        result[0].Is_validated = true;
                        return result[0];
                    }
                }
            }
            catch (Exception e)
            {
                return new ApplicationUser() { Is_validated = false };
            }

        }

        public string GetTransAmtLimit(string createUser)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("createUser", OracleDbType.Varchar2, ParameterDirection.Input, createUser);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<string>(conn, mainDbUser.DbUser + "SP_Get_TransAmtLimitByUser", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    conn.Close();
                    return result;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public object IsProceedToController(List<string> userInfos)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_WHO", OracleDbType.Varchar2, ParameterDirection.Input, userInfos[0]);
                    dyParam.Add("ROLE_ID", OracleDbType.Int32, ParameterDirection.Output, null, 32767);
                    dyParam.Add("FORCE_LG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    SqlMapper.Query(conn, mainDbUser.DbUser + "PR_PROCEED_LOGIN", param: dyParam, commandType: CommandType.StoredProcedure);
                    conn.Close();
                    var roleId = dyParam.oracleParameters[1].Value.ToString();
                    var fg = dyParam.oracleParameters[2].Value.ToString();
                    return Tuple.Create(roleId, fg);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public object GetAppUserListDdl()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"select t.username as ""label"", t.id ""value"" from" + mainDbUser.DbUser + "application_user t";
                    var result = connection.Query<CustomDropDownModel>(query).ToList();
                    this.CloseConnection(connection);
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public object GetAllApplicationUserList(string roleName)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = null;
                    //string query = @"Select a.name, a.username,r.name as RoleName, a.mobile_no ,a.email_id,a.log_in_status,a.pstatus,a.id from" + mainDbUser.DbUser + "application_user a inner join" + mainDbUser.DbUser + "role r on a.role_id=r.id";
                    if ((roleName == "Admin") || (roleName == "System Admin") || (roleName == "Super Admin"))
                    {
                        query = @"Select a.name, a.username,r.name as RoleName, a.mobile_no ,a.email_id,a.log_in_status,a.pstatus,a.id from" + mainDbUser.DbUser + "application_user a inner join" + mainDbUser.DbUser + "role r on a.role_id=r.id";
                    }
                    else
                    {
                        query = @"Select a.name, a.username,r.name as RoleName, a.mobile_no ,a.email_id,a.log_in_status,a.pstatus,a.id from" + mainDbUser.DbUser + "application_user a inner join" + mainDbUser.DbUser + "role r on a.role_id=r.id where r.Name in ('Branch Teller','Branch KYC Maker','Branch KYC Checker')";
                    }
                    var result = connection.Query<dynamic>(query).ToList();
                    this.CloseConnection(connection);
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public PasswordPolicy GetPasswordPolicy()
        {
            PasswordPolicy result = new PasswordPolicy();
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select Pass_min_length as PassMinLength,Pass_max_length as PassMaxLength,Pass_alpha_lower as PassAlphaLower,Pass_alpha_upper as PassAlphaUpper,pass_number as PassNumber,Pass_special_char as PassSpecialChar,pass_history_take as PassHistoryTake from" + mainDbUser.DbUser + "global_config";
                    result = connection.Query<PasswordPolicy>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
