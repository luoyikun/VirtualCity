using UnityEngine;
using System.Linq;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSV
{
    public string[] header;
    public List<string> lines;
}

public static class CSVReader
{
    private static readonly string splitter = "[liyu]";
    private static readonly string[] splitters = new string[] { splitter };
    public static CSV Read(string text)
    {
        CSV csv = new CSV();
        text = text.Trim().Replace("\r", "") + "\n";

        // read cells
        csv.lines = new List<string>();
        bool startCell = false;
        StringBuilder line = new StringBuilder();
        for (int i = 0; i < text.Length; ++i)
        {
            char c = text[i];
            if (c == '"')
            {
                if (text[i + 1] == '"') i++;
                else
                {
                    startCell = !startCell;
                    continue;
                }
            }
            else if (!startCell && c == ',')
            {
                line.Append(splitter);
                continue;
            }
            else if (!startCell && c == '\n')
            {
                csv.lines.Add(line.ToString());
                line.Length = 0;
                continue;
            }
            line.Append(c);
        }
        string lastLine = line.ToString().Trim();
        if (!string.IsNullOrEmpty(lastLine)) csv.lines.Add(lastLine);

        // add line number
        //csv.lines = csv.lines.Select((t, index) => string.Format("{0}{2}{1}", (index > 0 ? (index+1).ToString() : "line"), t, splitter)).ToList();

        // get header
        csv.header = ParseLine(csv.lines[0]);
        csv.lines.RemoveAt(0);

        return csv;
    }

    public static string[] ParseLine(string line)
    {
        return line.Split(splitters, StringSplitOptions.None);
    }

}

public class CSVMgr
{
    public static List<string[]> GetDataByPath(string path)
    {
        string tipString = GetJsonString(path);

        List<string[]> rows = new List<string[]>();
        CSV csv = CSVReader.Read(tipString);
        csv.lines.ForEach(line => rows.Add(CSVReader.ParseLine(line)));
        return rows;
    }

    public static List<string[]> GetData(string tipString)
    {

        List<string[]> rows = new List<string[]>();
        CSV csv = CSVReader.Read(tipString);
        csv.lines.ForEach(line => rows.Add(CSVReader.ParseLine(line)));
        return rows;
    }


    public static List<string[]> GetDataFromRes(string path)
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        string tipString = ta.text;

        List<string[]> rows = new List<string[]>();
        CSV csv = CSVReader.Read(tipString);
        csv.lines.ForEach(line => rows.Add(CSVReader.ParseLine(line)));
        return rows;
    }

    static public string GetJsonString(string path)     //从文件里面读取json数据
    {//读取Json数据
        StreamReader reader = new StreamReader(path);
        string jsonData = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        return jsonData;
    }

}