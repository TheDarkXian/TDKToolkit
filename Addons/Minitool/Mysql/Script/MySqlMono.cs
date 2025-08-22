using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql;
using Sirenix.OdinInspector;
using MySql.Data.MySqlClient;
using System.Data;

public class MySqlMono : MonoBehaviour
{
    #region 数据库设置
    [FoldoutGroup("数据库连接设置")]
    public string ip;
    [FoldoutGroup("数据库连接设置")]
    public string port;
    [FoldoutGroup("数据库连接设置")]
    public string user;
    [FoldoutGroup("数据库连接设置")]
    public string databse;
    [FoldoutGroup("数据库连接设置")]
    public string password;
    [FoldoutGroup("数据库连接设置")]
    public string charset="utf8";
    [FoldoutGroup("数据库连接设置")]
    [Button("连接数据库")]
    [HideIf("connection")]
    public void ConnectSql() {
        CloseSql();
        connection =MySqlStatic.ConnectToMySQL
                (ip, port, user, databse, password, charset);
    
    }
    [FoldoutGroup("数据库连接设置")]
    [Button("关闭数据库")]
    [ShowIf("connection")]
    public void CloseSql() {

        if (connection != null)
        {
            MySqlStatic.Close(ref connection);
        }
        connection = null;
    }
    MySqlConnection connection;
#endregion
    #region 增删改查

    [FoldoutGroup("查询数据")]
    public string searchtable;
    [FoldoutGroup("查询数据")]
    public string selectKey;
    [FoldoutGroup("查询数据")]
    [Button("查询数据")]
    [ShowIf("connection")]
    public void searchData() {
        string getstr = "";

        using (MySqlDataReader searchReader = MySqlStatic.Search(connection, searchtable, selectKey)) {

            while (searchReader.Read()) {


                for (int i = 0; i < searchReader.FieldCount; i++) {

                    getstr+=" "+searchReader.GetName(i)+ " "+searchReader.GetValue(i)+" ";
                }
                getstr += "\n";
            }
        }
            DebugstrWriteLine("查询数据", getstr);
    }


    [FoldoutGroup("添加数据")]
    public string addtable;
    [FoldoutGroup("添加数据")]
    public string addkey;
    [FoldoutGroup("添加数据")]
    public string addvalue;
    [FoldoutGroup("添加数据")]
    [Button("添加数据")]
    [ShowIf("connection")]
    public void addData() {

      int result= MySqlStatic.AddData(connection, addtable, addkey, addvalue);
        string getstr = "";
        if (result <= 0)
        {

            getstr ="失败了"+ addtable + "表中可能已经存在" + addkey + " " + addvalue;
        }
        else {

            getstr = addtable + "添加成功!";
        }
        DebugstrWriteLine("[添加数据]",getstr);
    }


    [FoldoutGroup("删除数据")]
    public string deletetable;
    [FoldoutGroup("删除数据")]
    public string deletetargetkey;
    [FoldoutGroup("删除数据")]
    public string deletetargetvalue;
    [FoldoutGroup("删除数据")]
    [Button("删除数据")]
    [ShowIf("connection")]
    public void Deletedata() {

        int result = MySqlStatic.DeleteData(connection,deletetable, deletetargetkey, deletetargetvalue);
        string getstr = "";
        if (result <= 0)
        {

            getstr = "失败了" + deletetable + "表中可能不存在" + deletetargetkey+" "+ deletetargetvalue;
        }
        else
        {

            getstr = deletetable + "删除成功!";
        }

        DebugstrWriteLine("[删除数据]", getstr);

    }


    [FoldoutGroup("更新数据")]
    public string updatetable;
    [FoldoutGroup("更新数据")]
    public string updatekeylist;
    [FoldoutGroup("更新数据")]
    public string updatevaluelist;
    [FoldoutGroup("更新数据")]
    public string updatevalueTarget;
    [FoldoutGroup("更新数据")]
    [Button("更新数据")]
    [ShowIf("connection")]
    public void UpdateValue() {

        int result = MySqlStatic.UpdateData(connection, updatetable,updatekeylist,updatevaluelist,updatevalueTarget);
        string getstr = "";
        if (result <= 0)
        {

            getstr = "失败了" + updatetable + "表中可能不存在" + updatevalueTarget;
        }
        else
        {

            getstr = updatetable + "更新数据成功!";
        }
        DebugstrWriteLine("[更新数据]", getstr);


    }
    #endregion

    #region 表
    [FoldoutGroup("添加一个列")]
    public string addtablename;
    [FoldoutGroup("添加一个列")]
    public string addtablecolkeyname;
    [FoldoutGroup("添加一个列")]
    [Button("添加列")]
    [ShowIf("connection")]
    public void AddAColToTable() {

        int result = 0;
        string getstr = "";
        result = MySqlStatic.AddanColumn(connection, addtablename, addtablecolkeyname);
            if (result <= 0)
            {
                getstr = "添加失败了,可能早就有这个列了";

            }
            else { 
            
                getstr = addtablename+"添加成功了列 "+ addtablecolkeyname;
        }


        DebugstrWriteLine("[添加一个列]",getstr);
    }
    [FoldoutGroup("删除一个列")]
    public string deletetablename;
    [FoldoutGroup("删除一个列")]
    public string deletetablecolkeyname;
    [FoldoutGroup("删除一个列")]
    [Button("删除列")]
    [ShowIf("connection")]
    public void DeleteAColToTable()
    {

        int reslut =MySqlStatic.DeleteColumn(connection,deletetablename,deletetablecolkeyname);
        string getstr = "";
        if (reslut > 0) { getstr = "删除成功了这个列" + deletetablecolkeyname; } else { getstr = "删除失败了，可能根本就没有这个列"; }

        DebugstrWriteLine("[删除一个列]", getstr);
    }

    #endregion

    void DebugstrWriteLine(string commandName,string context) {

        Debugstr += $"{commandName}[start]\n{context}\n{commandName}[end]\n";
    
    }


    [FoldoutGroup("数据库Debug面板")]
    [Multiline(10)]
    public string Debugstr;
    [FoldoutGroup("数据库Debug面板")]
    [Button("清理面板")]
    public void ClearDebugstr()
    {
        Debugstr = "";
    }
}
