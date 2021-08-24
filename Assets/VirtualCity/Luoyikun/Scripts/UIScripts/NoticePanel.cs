using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NoticePanelJson
{
    public string title;
    public string content;
}

public class SeviceTermJson
{
    public string v;
}

public class NoticePanel : UGUIPanel {
    public RectTransform m_rectContent;
    public GameObject m_btnAccept;
    public Text m_text;
    public Text m_title;
    public static NoticePanelJson m_info = new NoticePanelJson();
    public static NoticePanelJson m_GouWuJiangLi=new NoticePanelJson();
    public static NoticePanelJson m_DaiLiJiangLi=new NoticePanelJson();
    public static NoticePanelJson m_RankHelp=new NoticePanelJson();
    public static SeviceTermJson m_sevice = new SeviceTermJson();
    public ContentSizeFitter m_con;
    public RectTransform m_rect;
    // Use this for initialization
    void Start () {
        ClickListener.Get(m_btnAccept).onClick = OnBtnAccept;

        
    }

    public void OnBtnAccept(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
        //NewGuideMgr.Instance.StartOneNewGuide();
    }

    public override void OnClose()
    {
        m_title.text = "";
        m_text.text = "";
    }

    public override void OnOpen()
    {
        m_title.text = "";
        m_text.text = "";
        m_rect.localPosition = Vector3.zero;
    }
    public void ShowNotice()
    {
        
        m_title.text = "公告";
        //m_text.text = m_info.title + "\n\u3000\u3000" + m_info.content;
        m_text.text = m_info.content;
        m_rect.localPosition = Vector3.zero;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// type 0 = 购物奖励帮助
    /// type 1 = 代理奖励帮助
    public void ShowCommonNotice(int type)
    {
        
        switch (type)
        {
            case 0:
                m_title.text = "帮助";
                m_text.text = m_GouWuJiangLi.content;
                break;
            case 1:
                m_title.text = "帮助";
                m_text.text = m_DaiLiJiangLi.content;
                break;
            case 2:
                m_title.text = "帮助";
                m_text.text = m_RankHelp.content;
                break;
        }
        m_rect.localPosition = Vector3.zero;
    }
    public void ShowTermsOfService()
    {
       
        m_title.text = "服务条款";
        m_text.text = m_sevice.v;
        m_rect.localPosition = Vector3.zero;

    }
}

