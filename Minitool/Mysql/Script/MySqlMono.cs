using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql;
using Sirenix.OdinInspector;
using MySql.Data.MySqlClient;
using System.Data;

public class MySqlMono : MonoBehaviour
{
    #region ���ݿ�����
    [FoldoutGroup("���ݿ���������")]
    public string ip;
    [FoldoutGroup("���ݿ���������")]
    public string port;
    [FoldoutGroup("���ݿ���������")]
    public string user;
    [FoldoutGroup("���ݿ���������")]
    public string databse;
    [FoldoutGroup("���ݿ���������")]
    public string password;
    [FoldoutGroup("���ݿ���������")]
    public string charset="utf8";
    [FoldoutGroup("���ݿ���������")]
    [Button("�������ݿ�")]
    [HideIf("connection")]
    public void ConnectSql() {
        CloseSql();
        connection =MySqlStatic.ConnectToMySQL
                (ip, port, user, databse, password, charset);
    
    }
    [FoldoutGroup("���ݿ���������")]
    [Button("�ر����ݿ�")]
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
    #region ��ɾ�Ĳ�

    [FoldoutGroup("��ѯ����")]
    public string searchtable;
    [FoldoutGroup("��ѯ����")]
    public string selectKey;
    [FoldoutGroup("��ѯ����")]
    [Button("��ѯ����")]
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
            DebugstrWriteLine("��ѯ����", getstr);
    }


    [FoldoutGroup("�������")]
    public string addtable;
    [FoldoutGroup("�������")]
    public string addkey;
    [FoldoutGroup("�������")]
    public string addvalue;
    [FoldoutGroup("�������")]
    [Button("�������")]
    [ShowIf("connection")]
    public void addData() {

      int result= MySqlStatic.AddData(connection, addtable, addkey, addvalue);
        string getstr = "";
        if (result <= 0)
        {

            getstr ="ʧ����"+ addtable + "���п����Ѿ�����" + addkey + " " + addvalue;
        }
        else {

            getstr = addtable + "��ӳɹ�!";
        }
        DebugstrWriteLine("[�������]",getstr);
    }


    [FoldoutGroup("ɾ������")]
    public string deletetable;
    [FoldoutGroup("ɾ������")]
    public string deletetargetkey;
    [FoldoutGroup("ɾ������")]
    public string deletetargetvalue;
    [FoldoutGroup("ɾ������")]
    [Button("ɾ������")]
    [ShowIf("connection")]
    public void Deletedata() {

        int result = MySqlStatic.DeleteData(connection,deletetable, deletetargetkey, deletetargetvalue);
        string getstr = "";
        if (result <= 0)
        {

            getstr = "ʧ����" + deletetable + "���п��ܲ�����" + deletetargetkey+" "+ deletetargetvalue;
        }
        else
        {

            getstr = deletetable + "ɾ���ɹ�!";
        }

        DebugstrWriteLine("[ɾ������]", getstr);

    }


    [FoldoutGroup("��������")]
    public string updatetable;
    [FoldoutGroup("��������")]
    public string updatekeylist;
    [FoldoutGroup("��������")]
    public string updatevaluelist;
    [FoldoutGroup("��������")]
    public string updatevalueTarget;
    [FoldoutGroup("��������")]
    [Button("��������")]
    [ShowIf("connection")]
    public void UpdateValue() {

        int result = MySqlStatic.UpdateData(connection, updatetable,updatekeylist,updatevaluelist,updatevalueTarget);
        string getstr = "";
        if (result <= 0)
        {

            getstr = "ʧ����" + updatetable + "���п��ܲ�����" + updatevalueTarget;
        }
        else
        {

            getstr = updatetable + "�������ݳɹ�!";
        }
        DebugstrWriteLine("[��������]", getstr);


    }
    #endregion

    #region ��
    [FoldoutGroup("���һ����")]
    public string addtablename;
    [FoldoutGroup("���һ����")]
    public string addtablecolkeyname;
    [FoldoutGroup("���һ����")]
    [Button("�����")]
    [ShowIf("connection")]
    public void AddAColToTable() {

        int result = 0;
        string getstr = "";
        result = MySqlStatic.AddanColumn(connection, addtablename, addtablecolkeyname);
            if (result <= 0)
            {
                getstr = "���ʧ����,����������������";

            }
            else { 
            
                getstr = addtablename+"��ӳɹ����� "+ addtablecolkeyname;
        }


        DebugstrWriteLine("[���һ����]",getstr);
    }
    [FoldoutGroup("ɾ��һ����")]
    public string deletetablename;
    [FoldoutGroup("ɾ��һ����")]
    public string deletetablecolkeyname;
    [FoldoutGroup("ɾ��һ����")]
    [Button("ɾ����")]
    [ShowIf("connection")]
    public void DeleteAColToTable()
    {

        int reslut =MySqlStatic.DeleteColumn(connection,deletetablename,deletetablecolkeyname);
        string getstr = "";
        if (reslut > 0) { getstr = "ɾ���ɹ��������" + deletetablecolkeyname; } else { getstr = "ɾ��ʧ���ˣ����ܸ�����û�������"; }

        DebugstrWriteLine("[ɾ��һ����]", getstr);
    }

    #endregion

    void DebugstrWriteLine(string commandName,string context) {

        Debugstr += $"{commandName}[start]\n{context}\n{commandName}[end]\n";
    
    }


    [FoldoutGroup("���ݿ�Debug���")]
    [Multiline(10)]
    public string Debugstr;
    [FoldoutGroup("���ݿ�Debug���")]
    [Button("�������")]
    public void ClearDebugstr()
    {
        Debugstr = "";
    }
}
