﻿using Dapper;
using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface IBillCollectionCommonRepository : IBaseRepository<TblCashEntry>
    {
        //object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit);
        //string GetTransactionNo();
        //TblCashEntry GetDestributorDepositByTransNo(string transNo);
        //object DataInsertToTransMSTandDTL(TblCashEntry cashEntry);
        object GetFeaturePayDetails(int featureId);
        object GetSubMenuDDL(int featureId);
    }
    public class BillCollectionCommonRepository : BaseRepository<TblCashEntry>, IBillCollectionCommonRepository
    {

        MainDbUser mainDbUser = new MainDbUser();

        public object GetFeaturePayDetails(int featureId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    //string query = @"Select * from " + mainDbUser.DbUser + "FEATURE_PAY where FEATURE_ID= " + featureId;
                    string query = @"Select fp.*, 
                        case  fc.name when 'Utility Bill Collection' then 11
                          when 'Tuition Fee Collection' then 12
                            when 'Credit Card Bill Collection' then 13
                              when 'Other Bill/Fee Collection' then 14
                          end as ParentPenuId from " + mainDbUser.DbUser + "FEATURE_PAY fp inner join " + mainDbUser.DbUser + "feature f on fp.feature_id = f.id inner join" + mainDbUser.DbUser + "feature_category fc on f.category_id = fc.id where FEATURE_ID= " + featureId;
                    var result = connection.Query<dynamic>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetSubMenuDDL(int featureId)
        {
            //List<CustomDropDownModel> monthYearList = new List<CustomDropDownModel>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select name as label,subMenuId as value from " + mainDbUser.DbUser + "feature_pay_submenu where FEATURE_ID= " + featureId + " order by serial,name";
                    var result = connection.Query<CustomDropDownModel>(query);
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public string GetTransactionNo()
        //{
        //    try
        //    {
        //        using (var connection = this.GetConnection())
        //        {
        //            var parameter = new OracleDynamicParameters();
        //            parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
        //            var result = SqlMapper.Query<string>(connection, mainDbUser.DbUser + "SP_Get_TransactionNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

        //            connection.Close();

        //            return result;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public TblCashEntry GetDestributorDepositByTransNo(string transNo)
        //{
        //    try
        //    {
        //        using (var connection = this.GetConnection())
        //        {
        //            var parameter = new OracleDynamicParameters();
        //            parameter.Add("transNo", OracleDbType.Varchar2, ParameterDirection.Input, transNo);
        //            parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
        //            var result = SqlMapper.Query<TblCashEntry>(connection, mainDbUser.DbUser+ "SP_Get_DepositByTransNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //            connection.Close();

        //            return result;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public object DataInsertToTransMSTandDTL(TblCashEntry cashEntry)
        //{
        //    try
        //    {
        //        using (var connection = this.GetConnection())
        //        {
        //            var parameter = new OracleDynamicParameters();
        //            parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(cashEntry.TransNo));
        //            parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.AcNo);
        //            parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, cashEntry.Amount);
        //            parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
        //            parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
        //            parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
        //            parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
        //            parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.EntryBranchCode);
        //            parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.CheckedUser);

        //            SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_INSERT_TBL_CASH_ENTRY", param: parameter, commandType: CommandType.StoredProcedure);


        //            connection.Close();
        //            string flag = parameter.oracleParameters[4].Value != null ? parameter.oracleParameters[4].Value.ToString() : null;
        //            string successOrErrorMsg = null;
        //            if (flag == "0")
        //            {
        //                successOrErrorMsg = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
        //            }
        //            else
        //            {
        //                successOrErrorMsg = flag;
        //            }
        //            return successOrErrorMsg;
        //        }


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
