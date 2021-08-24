using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class danmu
{
    public string Danmu;
    public string ID;
}

public class LoadDanMu
{
    public List<danmu> dic =new List<danmu>();
}

public class CharacterList
{
    public List<Character> dic = new List<Character>();
}
public class Character
{
    public int Sex;
    public string Name;

}
public class BillList
{

}
public class Bill
{
    public string time;
    public int leixing;
    public string info;
    public int price;
}
public class Order1List
{
    public List<Order1> dic = new List<Order1>();
}
public class Order1
{
    //public int 
    public string time;
    public string goodsinfo;
    public string totalprice;
    public int goodsnum;
    public int state;
}

public class Loads
{
    public Dictionary<int,string> dic =new Dictionary<int,string>();
}

public class LoadScreenshot
{
    public List<string> dic =new List<string>();
}

//public class Sys
public class JsonMgr : MonoBehaviour {

    static public void SaveJsonString(string JsonString, string path)    //保存Json格式字符串
    {//写入Json数据
        if (File.Exists(path) == true)
        {
            File.Delete(path);
        }

        string onlyPath = GetOnlyPath(path);
        if (!Directory.Exists(onlyPath))
        {
            Directory.CreateDirectory(onlyPath);
        }

        FileInfo file = new FileInfo(path);   
        StreamWriter writer = file.CreateText();
        writer.Write(JsonString);
        writer.Close();
        writer.Dispose();
    }

    static public string GetJsonString(string path)     //从文件里面读取json数据
    {//读取Json数据
        StreamReader reader = new StreamReader(path);
        string jsonData = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        return jsonData;
    }

    public static string GetOnlyPath(string path)
    {
        string[] bufPath = path.Split('/');
        string name = bufPath[bufPath.Length - 1];
        string onlyPath = path.Replace(name, "");
        //string abPath = info.m_prefabName.Replace("/" + abName, "");
        //string[] bufAbName = abName.Split('.');
        return onlyPath;
    }
}
