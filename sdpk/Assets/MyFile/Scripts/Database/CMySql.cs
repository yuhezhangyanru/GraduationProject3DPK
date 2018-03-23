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
     static string id = "root";  //***��Ҫ��***
     static string pwd = "123";  //����
     static string database = "unity";//���ݿ���  
     //static string result = "";   //mySQL�İ汾
      
     //public static string strCommand = "Select road_id from roadtb ;";  
     public static DataSet MyObj;  

     public static void ConnectDB()  //�������ݿ�Ĳ���
     {
         string connectionString = string.Format("Server = {0}; Database = {1}; User ID = {2}; Password = {3};",host,database,id,pwd);  
            openSqlConnection(connectionString);    
            //��ȡ���ݺ���
            ReadRoadTb();  //��ȡ��·��Ϣ
            ReadRecordTb();//��ȡ������¼
            //��ȡϵͳ������Ϣ
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
    /// ɾ���������е�����
    /// </summary>
    /// <param name="tableName"></param>
    public static void ClearTable(string tableName)
    {
        string str = "delete from " + tableName;
        GetDataSet(str);
    }

    /// <summary>
    /// �ṩ�������ݿ�ĺ���, �û�Ҫд���������ṩд�����
    /// </summary>
    /// <param name="tableName">Ҫ����ı���</param>
    /// <param name="param">��¼��������</param>
    public static void WriteDB(string tableName, string [] param)
    {
        string str = "insert into "+tableName+" values(";
        string s1 = "'";
        for(int i = 0; i < param.Length; i ++)
        {
            str += s1 + param[i] + s1;   //ע�⣺д�ַ������͵�ʱ��Ҫ����''
            if (i != param.Length - 1) 
                str += ", ";
        }
        str += ")";
        Debug.Log("��ǰ�� SQL���L��" + str);
        GetDataSet(str);
    }


    /// <summary>
    /// �ṩ�������ñ�ķ������ڹرս����ʱ��ִ��
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
   

    //��ȡ���ݺ����� ��roadtb ���е����ݵ���
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
                    //��ȡ�ֶ�
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
                    RoadTb.RoadList.Add(RoadTb.road);  //�ӵ�������ȥ
                    print("road id" + reader.GetUInt32(0) + " 2: " + reader.GetString(1) + " 3: " + reader.GetString(2) + " 4:" + reader.GetString(3) + " 5:" + reader.GetString(4)+ " 6:" + reader.GetUInt32(5) + " 7:" + reader.GetUInt32(6) + " 8:" + reader.GetUInt32(7));
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("��ѯʧ���ˣ�");
        }
        finally
        {
            reader.Close();
        }         
    }
    
    /// <summary>
    /// ��ȡ�÷ּ�¼��
    /// </summary>
    public static void ReadRecordTb()
    {
        RecordTb.RecList.Clear(); //��ձ��е���������       
        MySqlCommand mySqlCommand = new MySqlCommand("Select * from recordtb;", dbConnection);
        MySqlDataReader reader = mySqlCommand.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    //��ȡ�ֶ�
                    RecordTb.record.score = reader.GetString(0);
                    RecordTb.record.time = reader.GetString(1);
                    RecordTb.record.cost = reader.GetString(2);
                    RecordTb.record.level = reader.GetString(3); //�����a,b,c,d
                    RecordTb.RecList.Add(RecordTb.record);
                 }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("��ѯʧ���ˣ�");
        }
        finally
        {
            reader.Close();
        }        



        //RecordTb.RecList.Clear(); //��ձ��е���������
        //MySqlCommand mySqlCommand = new MySqlCommand("Select * from recordtb;", dbConnection);
        //MySqlDataReader reader = mySqlCommand.ExecuteReader();
        //try
        //{
        //    while (reader.Read())
        //    {
        //        if (reader.HasRows)
        //        {
        //            //��ȡ�ֶ�
        //            RecordTb.record.score = reader.GetString(0);
        //            RecordTb.record.time = reader.GetString(1);
        //            RecordTb.RecList.Add(RecordTb.record);
        //            print("score: " + reader.GetString(0) + " time: " + reader.GetString(1));
        //        }
        //    }
        //}
        //catch (Exception)
        //{
        //    Console.WriteLine("��ѯʧ���ˣ�");
        //}
        //finally
        //{
        //    reader.Close();
        //}
    }

   
    /// <summary>
    ///���ձ������ȥ������������List string�У���Ҫʱ��ֱ��ʹ��List
    ///ע�����Ե�����¼���л���¼���еı����Ч
    /// </summary>
    /// <param name="tableName" >�������</param>
    public static List<string> GetOneColumnTab(string tableName)
    {
        SettingTb.Element.Clear();  //����б�
        List<string> tableData = new List<string>();   //���ݱ��е�����
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
                    tableData.Add(row[column].ToString());  //��ʱһ��ת��Ϊ�ַ���
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
                    //��ȡ�ֶ�
                    RecordTb.record.score = reader.GetString(0);
                    RecordTb.record.time = reader.GetString(1);
                    RecordTb.RecList.Add(RecordTb.record);
                    print("score: " + reader.GetString(0) + " time: " + reader.GetString(1));
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("��ѯʧ���ˣ�");
        }
        finally
        {
            reader.Close();
        }
    }
}  
