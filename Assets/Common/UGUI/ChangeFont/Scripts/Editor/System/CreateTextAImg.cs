using BlueToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class CreateTextAImg : Editor
{

    [MenuItem("GameObject/UI/Image")]
    static void CreatImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localPosition = Vector3.zero;
                Selection.activeTransform = go.transform;
            }
        }
    }

    [MenuItem("GameObject/UI/Text")]
    static void CreatText()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                go.GetComponent<Text>().raycastTarget = false;
                go.GetComponent<Text>().font = ToolCacheManager.GetFont();
                go.GetComponent<Text>().color = Color.black;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localPosition = Vector3.zero;
                Selection.activeTransform = go.transform;
            }
        }
    }

    [MenuItem("GameObject/UI/Button")]
    static void CreatButton()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Button", typeof(Button));
                Image img = go.AddComponent<Image>();
                Button btn = go.GetComponent<Button>();
                btn.targetGraphic = img;

                GameObject goText = new GameObject("Text", typeof(Text));
                goText.GetComponent<Text>().raycastTarget = false;
                goText.GetComponent<Text>().font = ToolCacheManager.GetFont();
                goText.GetComponent<Text>().text = "按钮";
                goText.GetComponent<Text>().color = Color.black;
                goText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                goText.transform.SetParent(go.transform);
                goText.transform.localPosition = Vector3.zero;


                //go.GetComponent<Text>().raycastTarget = false;
                //go.GetComponent<Text>().font = ToolCacheManager.GetFont();
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localPosition = Vector3.zero;
                Selection.activeTransform = go.transform;
            }
        }
    }

}
