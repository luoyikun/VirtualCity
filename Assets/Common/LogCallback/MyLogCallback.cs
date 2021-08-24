using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyLogCallback : MonoSingleton<MyLogCallback> {

    FileInfo fileInfo;
    string content = "";
    FileStream writer;
    System.Text.UTF8Encoding encoding;
    // Use this for initialization
    void Start()
    {


        string path;
#if UNITY_EDITOR
        path = Application.persistentDataPath;
#else
        path = Application.persistentDataPath;
#endif
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        fileInfo = new FileInfo(path + "/log.txt");
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        
        writer = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        encoding = new System.Text.UTF8Encoding();
        Application.logMessageReceived += LogCallback;


    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        string content = "";
        content += System.DateTime.Now + ":" + type.ToString() + ": " + "\r\n" +
         "condition" + ": " + condition + "\r\n" +
         "stackTrace" + ": " + stackTrace + "\r\n" +
         "--------------------------------------" + "\r\n";

//        switch (type)
//        {
//            case LogType.Error:
//                content += type.ToString() + ": " + "\r\n" +
//            "condition" + ": " + condition + "\r\n" +
//            "stackTrace" + ": " + stackTrace + "\r\n" +
//            "--------------------------------------" + "\r\n";
//                break;
//            case LogType.Assert:
//                content += type.ToString() + ": " + "\r\n" +
//"condition" + ": " + condition + "\r\n" +
//"stackTrace" + ": " + stackTrace + "\r\n" +
//"--------------------------------------" + "\r\n";
//                break;
//            case LogType.Warning:
//                content += type.ToString() + ": " + "\r\n" +
//"condition" + ": " + condition + "\r\n" +
//"stackTrace" + ": " + stackTrace + "\r\n" +
//"--------------------------------------" + "\r\n";
//                break;
//            case LogType.Log:
//                content += type.ToString() + ": " + "\r\n" +
//"condition" + ": " + condition + "\r\n" +
//"stackTrace" + ": " + stackTrace + "\r\n" +
//"--------------------------------------" + "\r\n";
//                break;
//            case LogType.Exception:
//                content += type.ToString() + ": " + "\r\n" +
//"condition" + ": " + condition + "\r\n" +
//"stackTrace" + ": " + stackTrace + "\r\n" +
//"--------------------------------------" + "\r\n";
//                break;
//            default:
//                break;
//        }

        
        writer.Write(encoding.GetBytes(content), 0, encoding.GetByteCount(content));
        writer.Flush();
    }

    void Stop()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying == false)
        {
            if (writer != null)
            {
                writer.Close();
            }
            //Application.logMessageReceived -= LogCallback;
        }
#endif
    }
    void OnDestroy()
    {
        writer.Close();
        Application.logMessageReceived -= LogCallback;
    }
}
