using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MenuAssetBundle : Editor {

    public static StringBuilder m_abNameStr = new StringBuilder();
    static string toPath = "Assets/VirtualCity/AbName.cs";

    [MenuItem("Assetbundle/Make Tag")]
    public static void MakeTag()
    {
        m_abNameStr = new StringBuilder();
        m_abNameStr.Append("namespace Vc {");
        m_abNameStr.AppendLine();
        m_abNameStr.AppendLine("public class AbName {");
        PackageUtils.CheckAndRunAllCheckers(false, true);

        m_abNameStr.Append("}");
        m_abNameStr.Append("}");
        JsonMgr.SaveJsonString(m_abNameStr.ToString(), toPath);
    }

    [MenuItem("Assetbundle/Open persistentDataPath")]
    public static void OpenPersistentDataPath()
    {
        EditorUtils.ExplorerFolder(Application.persistentDataPath);
    }


    [MenuItem("Assetbundle/Open streamingAssetsPath")]
    public static void OpenStreamingAssetsPath()
    {
        EditorUtils.ExplorerFolder(Application.streamingAssetsPath);
    }

}
