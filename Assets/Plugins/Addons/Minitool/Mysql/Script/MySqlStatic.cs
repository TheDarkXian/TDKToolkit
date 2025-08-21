using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class MySqlStatic
{
    #region ����ɾ�Ĳ�
    //��ѯ����ȡ����
    public static MySqlDataReader Search(MySqlConnection mysql, string tableName, string selectKey)
    {
        //select age,id,name
        //sql���ѡ��gametest��
        string sqlString = $"select {selectKey} from " + tableName;
        MySqlCommand cmd = new MySqlCommand(sqlString, mysql);
        MySqlDataReader reader = cmd.ExecuteReader();

        return reader;
    }

    //��ѯ����ȡ����
    public static MySqlDataReader Search(MySqlConnection mysql, string tableName, string[] selectKey)
    {
        string value = "";
        for (int i = 0; i < selectKey.Length - 1; i++)
        {

            value += selectKey[i] + ",";
        }
        value += selectKey[selectKey.Length - 1];
        //select age,id,name
        //sql���ѡ��gametest��
        return Search(mysql, tableName, value);
    }
    //��������
    public static int AddData(MySqlConnection mysql, string table, string[] key, string[] value)
    {
        if (key.Length != value.Length)
        {
            string debug = "";
            debug += " key ";
            foreach (var i in key)
            {

                debug += "{" + i + "} ";
            }
            debug += "\n value ";
            foreach (var j in value)
            {
                debug += "{" + j + "}";
            }
            Debug.Log(debug + "������ֵ������������");

            return 0;
        }

        //�ڱ�player������ID = id,LV = level,Name = name
        //string sql = "insert into player(ID,LV,Name) values('" + id + "','" + level + "','" + name + "')";
        string command = "insert into " + table;
        command += "(";
        for (int i = 0; i < key.Length - 1; i++)
        {
            string keyone = key[i];
            command += keyone + ",";
        }
        command += key[key.Length - 1] + ") values(";
        for (int i = 0; i < value.Length - 1; i++)
        {
            string valueone = value[i];

            command += valueone + ",";
        }
        command += value[value.Length - 1] + ")";
        int result = ExecuteNonQuery(mysql, command);
        return result;

    }
    public static int AddData(MySqlConnection mysql, string table, string key, string value)
    {
        if (key.Split(",").Length != value.Split(",").Length)
        {
            string debug = "";
            debug += " key ";
            foreach (var i in key)
            {

                debug += "{" + i + "} ";
            }
            debug += "\n value ";
            foreach (var j in value)
            {
                debug += "{" + j + "}";
            }
            Debug.Log(debug + "������ֵ������������");

            return 0;
        }
        string[] keylist = key.Split(",");
        string[] valuelist = value.Split(",");
        //�ڱ�player������ID = id,LV = level,Name = name
        //string sql = "insert into player(ID,LV,Name) values('" + id + "','" + level + "','" + name + "')";
        return AddData(mysql, table, keylist, valuelist);
    }
    //��������
    public static int UpdateData(MySqlConnection mysql, string table, string valueAndKey, string target)
    {
        //���±�player��ID = 2������ ,����LV = 9,Name = zhang
        //"update player set LV='9',Name='zhang' where {target}"
        int result = 0;
        string sql = $"update {table} set {valueAndKey} where {target}";//���ĵ�sql����
        result = ExecuteNonQuery(mysql, sql);
        return result;

    }
    public static int UpdateData(MySqlConnection mysql, string table, string key, string value, string target)
    {
        string[] keylist = key.Split(",");
        string[] valuelist = value.Split(",");
        return UpdateData(mysql, table, keylist, valuelist, target);

    }

    public static int UpdateData(MySqlConnection mysql, string table, string[] key, string[] value, string target)
    {
        //���±�player��ID = 2������ ,����LV = 9,Name = zhang
        //"update player set LV='9',Name='zhang' where {target}"
        if (key.Length != value.Length)
        {

            string debug = "";
            debug += " key ";
            foreach (var i in key)
            {

                debug += "{" + i + "} ";
            }
            debug += "\n value ";
            foreach (var j in value)
            {
                debug += "{" + j + "}";
            }
            Debug.Log(debug + "������ֵ������������");

            return 0;

        }
        List<string> keyvalues = new List<string>();
        for (int i = 0; i < key.Length - 1; i++)
        {
            keyvalues.Add(key[i] + "=" + "'" + value[i].Trim().Replace("'", "") + "' ,");
        }
        keyvalues.Add(key[key.Length - 1] + "=" + "'" + value[key.Length - 1].Trim().Replace("'", "") + "'");
        string keyvalue = "";
        foreach (var i in keyvalues)
        {

            keyvalue += i;
        }
        return UpdateData(mysql, table, keyvalue, target);

    }
    //ɾ������

    public static int DeleteData(MySqlConnection mysql, string table, string targetKey, string targetValue)
    {
        string[] targetKeyList = targetKey.Split(',');
        string[] targetValueList = targetValue.Split(',');
        return DeleteData(mysql, table, targetKeyList, targetValueList);
    }
    public static int DeleteData(MySqlConnection mysql, string table, string[] targetKey, string[] targetValue)
    {
        if (targetKey.Length != targetValue.Length)
        {
            Debug.LogError(targetKey.Length + "��" + targetValue.Length + "ֵ������");
            return 0;
        }
        string targetstr = "";
        for (int i = 0; i < targetKey.Length - 1; i++)
        {

            targetstr += targetKey[i] + "=" + targetValue[i] + " AND ";

        }
        targetstr += targetKey[targetKey.Length - 1] + "=" + targetValue[targetValue.Length - 1];

        return DeleteData(mysql, table, targetstr);
    }
    public static int DeleteData(MySqlConnection mysql, string table, string target)
    {
        int reslut = 0;
        //ɾ����sql���������ɾ��player��id=20��һ������
        string sql = $"delete from {table} where {target}";
        reslut = ExecuteNonQuery(mysql, sql);
        return reslut;
    }
    public static int ExecuteNonQuery(MySqlConnection myslq, string command)
    {

        int result = 0;
        Debug.Log("���ݿ�����:" + command);
        try
        {
            MySqlCommand cmd = new MySqlCommand(command, myslq);
            result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = null;
        }
        catch
        {
            //throw new Exception(E.Message);
        }

        return result;
    }

    #endregion

    public static int AddanColumn(MySqlConnection sql, string tablename, string keyname, string typestr = "VARCHAR(100)")
    {


        //"ALTER TABLE users ADD COLUMN email VARCHAR(100);
        string command = $"ALTER TABLE {tablename} ADD COLUMN {keyname} {typestr}";
        int result = 0;
        if (TableExistInPrimaryKey(sql, tablename, keyname))
        {
            result = -1;

        }
        else
        {
            result = ExecuteNonQuery(sql, command);
            result = 2;
        }

        return result;
    }

    public static int DeleteColumn(MySqlConnection sql, string tablename, string targetkey)
    {

        int result = 0;
        string command = $"alter table {tablename} drop  {targetkey}";
        if (TableExistInPrimaryKey(sql, tablename, targetkey))
        {

            result = ExecuteNonQuery(sql, command);
            result = 2;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    public static bool TableExistInPrimaryKey(MySqlConnection sql, string tableName, string keyName)
    {
        bool exists = false;
        // ��ѯ�����Ƿ����ָ���ֶ�
        string query = @"
                SELECT COLUMN_NAME 
                FROM information_schema.columns 
                WHERE table_name = @tableName 
                AND column_name = @columnName 
                AND table_schema = DATABASE();";

        using (MySqlCommand command = new MySqlCommand(query, sql))
        {
            // ���Ӳ�����ֹ SQL ע��
            command.Parameters.AddWithValue("@tableName", tableName);
            command.Parameters.AddWithValue("@columnName", keyName);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    exists = true;  // �����ѯ��������ݣ���ʾ�ֶδ���
                }
            }
        }



        return exists;
    }

    public static void Close(ref MySqlConnection sql)
    {
        if (sql != null)
        {
            sql.Close();
            sql.Dispose();
            sql = null;
        }
    }

    public static MySqlConnection ConnectToMySQL
        (string ip, string port, string user,
        string database, string password, string charset = "utf8")
    {

        //���ݿ��ַ���˿ڡ��û��������ݿ���������
        string sqlSer = $"server = {ip};port = {port};user= {user}" +
            $";database = {database};password = {password};charset={charset}";
        //��������
        MySqlConnection conn = new MySqlConnection(sqlSer);
        try
        {
            conn.Open();
            Debug.Log("------���ݿ� " + user + " ���ӳɹ�------");

        }
        catch (System.Exception e)
        {
            conn = null;
            Debug.Log("Error:" + e.Message);
        }


        return conn;
    }
}
