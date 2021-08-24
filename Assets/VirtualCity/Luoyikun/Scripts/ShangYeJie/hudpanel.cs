using System;
using Framework.Tools;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class hudpanel : UGUIPanel
{

    public static hudpanel m_instance;
    private BufferPool m_pool;
    private BufferPool m_ImageText;
    Transform m_trans;
    Dictionary<string, GameObject> m_dicHead = new Dictionary<string, GameObject>();
    private Dictionary<long, GameObject> m_dicChatImage = new Dictionary<long, GameObject>();
    public bl_HUDText HUDRoot;

    public GameObject TextPrefab;
    public GameObject TextImagePrefab;
    private void Awake()
    {
        m_instance = this;
    }
    private void Start()
    {

        m_trans = this.transform;
    }
    public override void OnOpen()
    {

    }

    public override void OnClose()
    {

    }

    public void ClearAll()
    {
        foreach (var item in m_dicHead)
        {
            m_pool.Recycle(item.Value);
        }
        m_dicHead.Clear();
    }
    public void SetCamera(Camera cam)
    {
        HUDRoot.m_Cam = cam;
    }
    public void CreateOneHeadUi(long id, string name, Transform followTrans)
    {
        if (m_pool == null)
        {
            m_pool = new BufferPool(TextPrefab, transform, 1);
        }

        GameObject tempObj = m_pool.GetObject();
        tempObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        bl_Text textInfo = tempObj.GetComponent<bl_Text>();

        HUDTextInfo info4 = new HUDTextInfo(followTrans, name);
        info4.Color = Color.white;
        info4.Size = 1;
        info4.Speed = 200;
        info4.VerticalAceleration = 1;
        info4.VerticalPositionOffset = 0.0f;//偏移量
        info4.VerticalFactorScale = 1.2f;
        info4.Side = bl_Guidance.Right;
        info4.TextPrefab = TextPrefab;
        info4.FadeSpeed = 500;
        
        info4.ExtraDelayTime = float.MaxValue;//存在时间
        info4.AnimationType = bl_HUDText.TextAnimationType.None;
        //Send the information
        HUDRoot.CreateNewText(textInfo, info4);

        m_dicHead[id.ToString()] = tempObj;
    }

    public void CreateOneHeadUi(string id, string name, Transform followTrans)
    {
        if (m_pool == null)
        {
            m_pool = new BufferPool(TextPrefab, transform, 1);
        }

        GameObject tempObj = m_pool.GetObject();
        tempObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        bl_Text textInfo = tempObj.GetComponent<bl_Text>();

        HUDTextInfo info4 = new HUDTextInfo(followTrans, name);
        info4.Color = Color.white;
        info4.Size = 1;
        info4.Speed = 200;
        info4.VerticalAceleration = 1;
        info4.VerticalPositionOffset = 0.0f;//偏移量
        info4.VerticalFactorScale = 1.2f;
        info4.Side = bl_Guidance.Right;
        info4.TextPrefab = TextPrefab;
        info4.FadeSpeed = 500;

        info4.ExtraDelayTime = float.MaxValue;//存在时间
        info4.AnimationType = bl_HUDText.TextAnimationType.None;
        //Send the information
        HUDRoot.CreateNewText(textInfo, info4);

        m_dicHead[id] = tempObj;
    }

    public void CreateOneHeadUi(long id, string text, Transform followTrans, string mType)
    {
        if (m_ImageText == null)
        {
            m_ImageText = new BufferPool(TextImagePrefab, transform, 1);
        }

        if (mType == "Chat")
        {
            GameObject tempObj = m_ImageText.GetObject(); ;
            //GameObject tempObj = m_ImageText.GetObject();
            //tempObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //bl_Text textInfo = tempObj.GetComponent<bl_Text>();
            tempObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            bl_Text textInfo = tempObj.GetComponent<bl_Text>();
            int idx = 0;
            for (int i = 0; i < text.Length; i++,idx++)
            {
                if (idx == 8)
                {
                    text =text.Insert(i, "&&");
                    idx = -2;
                }
            }

            text = text.Replace("&&", Environment.NewLine);
            //text.Replace("&&", "\n");
            HUDTextInfo info4 = new HUDTextInfo(followTrans, text);
            info4.Color = Color.white;
            info4.Size = 1;
            info4.Speed = 200;
            info4.VerticalAceleration = 1;
            info4.VerticalPositionOffset = 0.35f;
            info4.VerticalFactorScale = 1.2f;
            info4.Side = bl_Guidance.Right;
            info4.TextPrefab = TextImagePrefab;
            info4.FadeSpeed = 500;
            info4.ExtraDelayTime = 5;
            info4.AnimationType = bl_HUDText.TextAnimationType.None;
            info4.ExtraFloatSpeed = 100000;
            //Send the information
            HUDRoot.CreateNewText(textInfo, info4, mType);
        }
    }
    public void Recycle(long id)
    {
        if (m_pool == null)
        {
            m_pool = new BufferPool(TextPrefab, transform, 1);
        }
        if (m_dicHead.ContainsKey(id.ToString()))
        {
            HUDRoot.RemoveText(m_dicHead[id.ToString()].transform);
            m_pool.Recycle(m_dicHead[id.ToString()]);
            m_dicHead.Remove(id.ToString());
        }
    }

    public void SetVisible(long id, bool isVisible)
    {
        m_dicHead[id.ToString()].SetActive(isVisible);
    }

    public void SetVisible(string id, bool isVisible)
    {
        m_dicHead[id].SetActive(isVisible);
    }
}
