using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OpenFileByOtherExe : MonoBehaviour {

    [UnityEditor.Callbacks.OnOpenAssetAttribute(1)]
    public static bool ClickOnce(int instanceID, int line)
    {
        return false;
    }

    [UnityEditor.Callbacks.OnOpenAssetAttribute(2)]
    public static bool ClickTwice(int instanceID, int line)
    {
        string path = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));
        string name = Application.dataPath + "/" + path.Replace("Assets/", "");

        if (name.EndsWith(".csv"))
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "D:/Program Files (x86)/Rons Place Apps/Rons Editor/Editor.WinGUI.exe";
            startInfo.Arguments = name;
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }
        return false;
    }
}
