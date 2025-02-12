﻿using Oracle.ManagedDataAccess.Client;
using SM.Training.Common;
using SM.Training.SharedComponent.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Training.BookingCar.Dao
{
    public class CARDao : BaseDao
    {
        //get all Car
        public List<CAR> GetAllCar(int pageIndex, int pageSize)
        {
            var sql = @"SELECT 
                        CARCATEGORY.NAME AS NAME_CARCATEGORY, 
                        CAR.CAR_ID, 
                        CAR.CATEGORY_ID, 
                        CAR.COLOR, 
                        CAR.DESCRIPTION, 
                        CAR.PLATE_NUMBER, 
                        CAR.PRICE, 
                        CAR.NAME 
                      FROM CAR 
                      RIGHT JOIN CARCATEGORY ON CAR.CATEGORY_ID = CARCATEGORY.CARCATEGORY_ID 
                      WHERE CAR.DELETED = 0 
                      ORDER BY CAR_ID DESC";
            OracleCommand oracleCommand = new OracleCommand(sql);
            using (DataContext dataContext = new DataContext())
            {
                var count = 0;
                var query = dataContext.ExecutePaging<CAR>(oracleCommand, pageIndex - 1, pageSize, out count, true);
                return query;
            }
        }


        //get by id Car
        public CAR GetByIdCAR(int id)
        {
            var sql = "SELECT * FROM CAR WHERE DELETED = 0 AND CAR_ID = :id";
            OracleCommand oracleCommand = new OracleCommand(sql);
            oracleCommand.Parameters.Add(":id", id);
            using (DataContext dataContext = new DataContext())
            {
                var query = dataContext.ExecuteSelect<CAR>(oracleCommand).FirstOrDefault();
                return query;
            }
        }


        //insert Car
        public int InsertCAR(CAR item)
        {
            //var sql = "SELECT * FROM CAR WHERE PLATE_NUMBER LIKE '%:plateNumber%'";
            //OracleCommand oracleCommand = new OracleCommand(sql);
            //oracleCommand.Parameters.Add(":plateNumber", item.PLATE_NUMBER);
            using (DataContext dataContext = new DataContext())
            {
                dataContext.InsertItem<CAR>(item);
                return 1;
            }
        }


        //update Car
        public int UpdateCAR(CAR item)
        {
            using (DataContext dataContext = new DataContext())
            {
                var query = dataContext.UpdateItem<CAR>(item);
                return query;
            }
        }


        //get by id Service_Order - Time
        public List<SERVICE_ORDER> GetAllSelectSERVICE_ORDER(DateTime fromDate, DateTime toDate)
        {
            var sql = @"SELECT 
                        enOrder.CAR_ID
                      FROM SERVICE_ORDER enOrder
                      WHERE enOrder.DELETED = 0 
                      AND TO_CHAR(enOrder.PLANSTART_DTG, 'DD-MM-YYYY HH24:MI:SS') >= '{0:dd-MM-yyyy HH:mm:ss}'
                      AND TO_CHAR(enOrder.PLANEND_DTG, 'DD-MM-YYYY HH24:MI:SS') <= '{1:dd-MM-yyyy HH:mm:ss}'
                      AND STATUS IN (1, 2)
                      ";
            sql = String.Format(sql, fromDate, toDate);
            OracleCommand oracleCommand = new OracleCommand(sql);
            using (DataContext dataContext = new DataContext())
            {
                var query = dataContext.ExecuteSelect<SERVICE_ORDER>(oracleCommand);

                return query;
            }
        }
        //get all Car - select
        public List<CAR> GetAllSelectCAR(int id_Category, List<int> listNumber)
        {
            var sql = "SELECT * FROM CAR WHERE DELETED = 0 AND CATEGORY_ID = :id_Category AND CAR_ID NOT IN ({0}) ORDER BY CAR_ID ASC";
            sql = string.Format(sql, BuildInCondition(listNumber));
            OracleCommand oracleCommand = new OracleCommand(sql);
            oracleCommand.Parameters.Add(":id_Category", id_Category);
            using (DataContext dataContext = new DataContext())
            {
                var query = dataContext.ExecuteSelect<CAR>(oracleCommand);
                return query;
            }
        }

        //public List<int> GetAllSelectCAR(int id_Category, List<int> listNumber)
        //{
        //    using (DataContext dataContext = new DataContext())
        //    {
        //        return listNumber;
        //    }
        //}

        //search list id Car - select
        public List<CAR> SearchIdCAR(List<int> listID)
        {
            var sql = @"SELECT 
                        CARCATEGORY.NAME AS NAME_CARCATEGORY, 
                        CAR.CAR_ID, 
                        CAR.CATEGORY_ID, 
                        CAR.COLOR, 
                        CAR.DESCRIPTION, 
                        CAR.PLATE_NUMBER, 
                        CAR.PRICE, 
                        CAR.NAME 
                      FROM CAR 
                      RIGHT JOIN CARCATEGORY ON CAR.CATEGORY_ID = CARCATEGORY.CARCATEGORY_ID  
                      WHERE CAR.DELETED = 0 AND CAR.CAR_ID IN ({0}) ORDER BY CAR_ID ASC";
            sql = string.Format(sql, BuildInCondition(listID));
            using (DataContext dataContext = new DataContext())
            {
                var query = dataContext.ExecuteSelect<CAR>(sql);
                return query;
            }
        }
    }
}
