using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Framework.UI;

public class SaveTransInfo : EditorWindow
{

    [MenuItem("SaveTransInfo/SaveOriInfoTo")]
    static void SaveOriInfoTo()
    {
        GameObject obj = Selection.activeGameObject;
        Transform trans = obj.transform;

        string str = "'x':" + trans.localPosition.x + ",'y':" + trans.localPosition.y + ",'z':" + trans.localPosition.z;
        //JsonMgr.SaveJsonString(str, "e:/Test.txt");

        TextEditor te = new TextEditor();
        te.content = new GUIContent(str);
        te.SelectAll();
        te.Copy();
    }

    [MenuItem("SaveTransInfo/GetTransPosAndAngle #1")]
    static void GetTransPosAndAngle()
    {
        GameObject obj = Selection.activeGameObject;
        Transform trans = obj.transform;

        string str = "{\"x\":" + trans.localPosition.x + ",\"y\":" + trans.localPosition.y + ",\"z\":" + trans.localPosition.z + 
            ",\"dirX\":" + trans.localEulerAngles.x + ",\"dirY\":" + trans.localEulerAngles.y + ",\"dirZ\":" + trans.localEulerAngles.z + "}"
            ;
        //JsonMgr.SaveJsonString(str, "e:/Test.txt");

        TextEditor te = new TextEditor();
        te.content = new GUIContent(str);
        te.SelectAll();
        te.Copy();
    }

    [MenuItem("SaveTransInfo/GetTransPos")]
    static void GetTransPos()
    {
        GameObject obj = Selection.activeGameObject;
        Transform trans = obj.transform;

        string str = trans.localPosition.x.ToString("0.00") + "," + trans.localPosition.y.ToString("0.00") + "," + trans.localPosition.z.ToString("0.00");
        //JsonMgr.SaveJsonString(str, "e:/Test.txt");

        TextEditor te = new TextEditor();
        te.content = new GUIContent(str);
        te.SelectAll();
        te.Copy();
    }

    [MenuItem("SaveTransInfo/GetUIChildPath #`")]
    static void GetUIChildPath()
    {
        GameObject obj = Selection.activeGameObject;


        Transform trans = obj.transform;
        listParName.Clear();
        FindParName(trans);

        string parPath = "";
        for (int i = listParName.Count - 1; i >= 0; i--)
        {
            parPath += listParName[i];
            if (i != 0)
            {
                parPath += "/";
            }
        }

        Debug.Log(parPath);
         TextEditor te = new TextEditor();
        te.content = new GUIContent(parPath);
        te.SelectAll();
        te.Copy();
    }

    static List<string> listParName = new List<string>();
    public static  bool FindParName(Transform trans)
    {
        
        if (trans == null)
        {
            return false;
        }
        if (trans.GetComponent<UGUIPanel>() == null)
        {
            listParName.Add(trans.name);
            FindParName(trans.parent);
            return false;
        }
        return true;

    }
}