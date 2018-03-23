using UnityEngine;  
using System;  
using System.Collections;  
using System.Data;  
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using myfiles.scripts.tools;
using System.Collections.Generic;  

public class CMySql : MonoBehaviour {  
     public static MySqlConnection dbConnection;//Just like MyConn.conn in StoryTools before  
     static string host = "127.0.0.1";  
     static string id = "root";  //***不要变***
     static string pwd = "123";  //密码
     static string database = "unity";//数据库名  
     //static string result = "";   //mySQL的版本
      
     //public static string strCommand = "Select road_id from roadtb ;";  
     public static DataSet MyObj;  

     public static void ConnectDB()  //连接数据库的测试
     {
         string connectionString = string.Format("Server = {0}; Database = {1}; User ID = {2}; Password = {3};",host,database,id,pwd);  
            openSqlConnection(connectionString);    
            //读取数据函数
            ReadRoadTb();  //读取道路信息
            ReadRecordTb();//读取分数记录
            //读取系统设置信息
            SettingTb.Element = GetOneColumnTab("settingtb");
     }
    // On quit  
    public static void OnApplicationQuit() 
    {  
        closeSqlConnection();  
    }  
     
    // Connect to database  
    private static void openSqlConnection(string connectionString) 
    {  
        dbConnection = new MySqlConnection(connectionString);  
        dbConnection.Open();  
    }  
     
    // Disconnect from database  
    private static void closeSqlConnection() 
    {  
        dbConnection.Close();  
        dbConnection = null;  
    }   
      
    // MySQL Query  
    public static void doQuery(string sqlQuery) 
    {  
        IDbCommand dbCommand = dbConnection.CreateCommand();      
        dbCommand.CommandText = sqlQuery;  
        IDataReader reader = dbCommand.ExecuteReader();  
        reader.Close();  
        reader = null;  
        dbCommand.Dispose();  
        dbCommand = null;  
    }  
    #region Get DataSet  
    public  static DataSet GetDataSet(string sqlString)  
    {   
        DataSet ds = new DataSet();  
        try  
        {  
            MySqlDataAdapter da = new MySqlDataAdapter(sqlString, dbConnection);  
            da.Fill(ds);  
     
        }  
        catch (Exception ee)  
        {       
            throw new Exception("SQL:" + sqlString + "\n" + ee.Message.ToString());  
        }  
        return ds;  
    }  
    #endregion   

    /// <summary>
    /// 删除表中所有的数据
    /// </summary>
    /// <param name="tableName"></param>
    public static void ClearTable(string tableName)
    {
        string str = "delete from " + tableName;
        GetDataSet(str);
    }

    /// <summary>
    /// 提供插入数据库的函数, 用户要写出表名和提供写入参数
    /// </summary>
    /// <param name="tableName">要插入的表名</param>
    /// <param name="param">记录内容数组</param>
    public static void WriteDB(string tableName, string [] param)
    {
        string str = "insert into "+tableName+" values(";
        string s1 = "'";
        for(int i = 0; i < param.Length; i ++)
        {
            str += s1 + param[i] + s1;   //注意：写字符串类型的时候要加上''
            if (i != param.Length - 1) 
                str += ", ";
        }
        str += ")";
        Debug.Log("当前的 SQL语句L：" + str);
        GetDataSet(str);
    }


    /// <summary>
    /// 提供更新配置表的方法，在关闭界面的时候执行
    /// </summary>
    /// <param name="music"></param>
    /// <param name="hind_tip"></param>
    /// <param name="speed"></param>
    /// <param name="quality"></param>
    public static void UpdateSetting(string music, string hind_tip, string speed, string quality)
    {
        string str = "update settingtb set music = " + music + ", hind_tip = " + hind_tip + ", speed = " + speed + ", quality = " + quality;
        GetDataSet(str);
    }
   

    //读取数据函数， 将roadtb 表中的数据导入
    public static void ReadRoadTb()
    {
        MySqlCommand mySqlCommand = new MySqlCommand("Select * from roadtb;", dbConnection);
        MySqlDataReader reader = mySqlCommand.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    //读取字段
                    RoadTb.road.road_id = reader.GetInt32(0);
                    RoadTb.road.road_name = reader.GetString(1);
                    RoadTb.road.gold_pos.x = float.Parse(reader.GetString(2));
                    RoadTb.road.gold_pos.y = float.Parse(reader.GetString(3));
                    RoadTb.road.gold_pos.z = float.Parse(reader.GetString(4));
                    RoadTb.road.is_gold = reader.GetInt32(5);
                    RoadTb.road.monster = reader.GetInt32(6);
                    RoadTb.road.is_monster = reader.GetInt32(7);
                    RoadTb.road.box = reader.GetInt32(8);
                    RoadTb.road.is_box = reader.GetInt32(9);
                    RoadTb.road.box_pos.x =  float.Parse(reader.GetString(10));
                    RoadTb.road.box_pos.y = float.Parse(reader.GetString(11));
                    RoadTb.road.box_pos.z = float.Parse(reader.GetString(12));
                    RoadTb.road.mon_pos.x = float.Parse(reader.GetString(13));
                    RoadTb.road.mon_pos.y = float.Parse(reader.GetString(14));
                    RoadTb.road.mon_pos.z = float.Parse(reader.GetString(15));
                    RoadTb.RoadList.Add(RoadTb.road);  //加到队列中去
                    print("road id" + reader.GetUInt32(0) + " 2: " + reader.GetString(1) + " 3: " + reader.GetString(2) + " 4:" + reader.GetString(3) + " 5:" + reader.GetString(4)+ " 6:" + reader.GetUInt32(5) + " 7:" + reader.GetUInt32(6) + " 8:" + reader.GetUInt32(7));
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("查询失败了！");
        }
        finally
        {
            reader.Close();
        }         
    }
    
    /// <summary>
    /// 读取得分纪录表
    /// </summary>
    public static void ReadRecordTb()
    {
        RecordTb.RecList.Clear(); //清空表中的所有数据       
        MySqlCommand mySqlCommand = new MySqlCommand("Select * from recordtb;", dbConnection);
        MySqlDataReader reader = mySqlCommand.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    //读取字段
                    RecordTb.record.score = reader.GetString(0);
                    RecordTb.record.time = reader.GetString(1);
                    RecordTb.record.cost = reader.GetString(2);
                    RecordTb.record.level = reader.GetString(3); //存的是a,b,c,d
                    RecordTb.RecList.Add(RecordTb.record);
                 }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("查询失败了！");
        }
        finally
        {
            reader.Close();
        }        



        //RecordTb.RecList.Clear(); //清空表中的所有数据
        //MySqlCommand mySqlCommand = new MySqlCommand("Select * from recordtb;", dbConnection);
        //MySqlDataReader reader = mySqlCommand.ExecuteReader();
        //try
        //{
        //    while (reader.Read())
        //    {
        //        if (reader.HasRows)
        //        {
        //            //读取字段
        //            RecordTb.record.score = reader.GetString(0);
        //            RecordTb.record.time = reader.GetString(1);
        //            RecordTb.RecList.Add(RecordTb.record);
        //            print("score: " + reader.GetString(0) + " time: " + reader.GetString(1));
        //        }
        //    }
        //}
        //catch (Exception)
        //{
        //    Console.WriteLine("查询失败了！");
        //}
        //finally
        //{
        //    reader.Close();
        //}
    }

   
    /// <summary>
    ///按照表的名称去读表，结果存放在List string中，需要时候直接使用List
    ///注：仅对单条记录多列或多记录单列的表格有效
    /// </summary>
    /// <param name="tableName" >表的名字</param>
    public static List<string> GetOneColumnTab(string tableName)
    {
        SettingTb.Element.Clear();  //清空列表
        List<string> tableData = new List<string>();   //数据表中的内容
        string strSql = "Select * from " + tableName + ";";
        try
        {
            MySqlDataAdapter sda = new MySqlDataAdapter(strSql, dbConnection);
            DataSet ds = new DataSet();
            sda.Fill(ds, "table");
            sda.Dispose();
            dbConnection.Dispose();
            DataTable dt = ds.Tables["table"];
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Console.WriteLine(row[column]);
                    tableData.Add(row[column].ToString());  //暂时一律转换为字符串
                }
            }
            return tableData;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void ReadTable(string tableName)
    {
        MySqlCommand mySqlCommand = new MySqlCommand("Select * from " + tableName + ";", dbConnection);
        MySqlDataReader reader = mySqlCommand.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    //读取字段
                    RecordTb.record.score = reader.GetString(0);
                    RecordTb.record.time = reader.GetString(1);
                    RecordTb.RecList.Add(RecordTb.record);
                    print("score: " + reader.GetString(0) + " time: " + reader.GetString(1));
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("查询失败了！");
        }
        finally
        {
            reader.Close();
        }
    }
}  
