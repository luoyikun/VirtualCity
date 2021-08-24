using Framework.Event;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnBoR
{
    Build,
    Remove
}
public class hometownborpanel : UGUIPanel {
    public GameObject m_btnBuild;
    public GameObject m_btnRemove;
    public GameObject m_btnClose;

    public Image m_imgIcon;
    public Text m_textName;

    public Text m_textBuildTime;
    public Text m_textIncomeTime;
    public Text m_textType;
    public Text m_textIncome;
    public Text m_textNeedAsset;
    public Text m_TextRedPeckt;
    public Text m_textAsset;

    public GameObject m_onlyGold;
    public GameObject m_onlyDiamon;
    public GameObject m_goldADiamon;
    public static hometownborpanel m_instance = null;
    // Use this for initialization
    void Start () {
        m_instance = this;
        ClickListener.Get(m_btnBuild).onClick = OnBtnBuild;
        ClickListener.Get(m_btnRemove).onClick = OnBtnRemove;
        ClickListener.Get(m_btnClose).onClick = OnBtnClose;
    }

    public override void OnOpen()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeTown);
    }

    public override void OnClose()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeTown);
        m_instance = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buildId">新建：模型数据的id。。。。删除：地块的id</param>
    /// <param name="bOr"></param>
    public void SetCommonBuildInfo(long buildId,EnBoR bOr)
    {
        SetPanelInfoIsBuild(true);
        StartCoroutine(YieldSetCommonBuildInfo(buildId, bOr));
    }

    public void SetHomeInfo(long id, EnBoR bOr)
    {
        SetPanelInfoIsBuild(false);
        StartCoroutine(YieldSetHomeInfo(id, bOr));
    }

    void SetPanelInfoIsBuild(bool isBuild)
    {
        m_textBuildTime.transform.parent.gameObject.SetActive(isBuild);
        m_textIncomeTime.transform.parent.gameObject.SetActive(isBuild);
        m_textIncome.transform.parent.gameObject.SetActive(isBuild);
        m_textNeedAsset.transform.parent.gameObject.SetActive(isBuild);
    }
    IEnumerator YieldSetCommonBuildInfo(long buildId, EnBoR bOr)
    {

        DevlopmentProperties info = new DevlopmentProperties();
        switch (bOr)
        {
            case EnBoR.Build:
                m_btnBuild.SetActive(true);
                m_btnRemove.SetActive(false);
                break;
            case EnBoR.Remove:
                m_btnBuild.SetActive(false);
                m_btnRemove.SetActive(true);


                break;
            default:
                break;
        }

        info = DataMgr.m_dicDevlopmentProperties[buildId];



        ModelData modelData = JsonConvert.DeserializeObject<ModelData>(info.modleData);

        //加载图标
        AssetMgr.Instance.CreateSpr(modelData.name, "commonbuild", (spr) => { m_imgIcon.sprite = spr; });

        m_textName.text = info.cnName;
        m_textBuildTime.text = PublicFunc.GetTimeBySec((int)info.builtTime);
        m_textIncomeTime.text = PublicFunc.GetTimeBySec((int)info.maxIncomeTime);
        m_textType.text = "共用";
        m_textAsset.transform.parent.gameObject.SetActive(false);
        //m_textIncome.text = PublicFunc.GetTimeBySec((int)info.income);
        m_textIncome.text = ((int)(info.income)).ToString();
        m_TextRedPeckt.text ="<color=red>" +info.redPacketShow+"</color>";
        //m_textPrice.text = PublicFunc.GetTimeBySec((int)info.);
        m_textNeedAsset.text = info.scoreNeed.ToString();

        if (info.gold > 0 && info.diamond == 0)
        {
            m_onlyGold.SetActive(true);
            m_onlyDiamon.SetActive(false);
            m_goldADiamon.SetActive(false);

            m_onlyGold.transform.Find("Text").GetComponent<Text>().text = info.gold.ToString();
        }
        else if (info.gold == 0 && info.diamond > 0)
        {
            m_onlyGold.SetActive(false);
            m_onlyDiamon.SetActive(true);
            m_goldADiamon.SetActive(false);

            m_onlyDiamon.transform.Find("Text").GetComponent<Text>().text = info.diamond.ToString();
        }
        else if (info.gold > 0 && info.diamond > 0)
        {
            m_onlyGold.SetActive(false);
            m_onlyDiamon.SetActive(false);
            m_goldADiamon.SetActive(true);

            m_goldADiamon.transform.Find("TextGold").GetComponent<Text>().text = info.gold.ToString();
            m_goldADiamon.transform.Find("TextDiamo").GetComponent<Text>().text = info.diamond.ToString();
        }
        yield return null;
    }

    IEnumerator  YieldSetHomeInfo(long buildId, EnBoR bOr)
    {

        switch (bOr)
        {
            case EnBoR.Build:
                m_btnBuild.SetActive(true);
                m_btnRemove.SetActive(false);
                break;
            case EnBoR.Remove:
                m_btnBuild.SetActive(false);    
                m_btnRemove.SetActive(true);
                break;
            default:
                break;
        }


        HomeProperties info = DataMgr.m_dicHomeProperties[buildId];



        

        //加载图标
        AssetMgr.Instance.CreateSpr(info.icon, "homeicon", (spr) => { m_imgIcon.sprite = spr; });

        m_textName.text = info.name;
        
        m_textType.text = "住宅";
        m_textAsset.transform.parent.gameObject.SetActive(true);
        m_textAsset.text = info.score.ToString();
        if (info.gold > 0 && info.diamond == 0)
        {
            m_onlyGold.SetActive(true);
            m_onlyDiamon.SetActive(false);
            m_goldADiamon.SetActive(false);

            m_onlyGold.transform.Find("Text").GetComponent<Text>().text = info.gold.ToString();
        }
        else if (info.gold == 0 && info.diamond > 0)
        {
            m_onlyGold.SetActive(false);
            m_onlyDiamon.SetActive(true);
            m_goldADiamon.SetActive(false);

            m_onlyDiamon.transform.Find("Text").GetComponent<Text>().text = info.diamond.ToString();
        }
        else if (info.gold > 0 && info.diamond > 0)
        {
            m_onlyGold.SetActive(false);
            m_onlyDiamon.SetActive(false);
            m_goldADiamon.SetActive(true);

            m_goldADiamon.transform.Find("TextGold").GetComponent<Text>().text = info.gold.ToString();
            m_goldADiamon.transform.Find("TextDiamo").GetComponent<Text>().text = info.diamond.ToString();
        }
        


        yield return null;
 

    }

    void OnBtnBuild(GameObject obj)
    {

        //buildhometown.m_instance.SetBoxSelect(-1);
        //buildhometown.m_instance.SetBoxCollider(true);
        //buildhometown.m_instance.m_curBuild = null;


        //UIManager.Instance.PopSelf();

        buildhometown.m_instance.SendBuild();
    }

    void OnBtnRemove(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "是否要拆除建筑？");
        ispanel.m_ok = () => { buildhometown.m_instance.SendRemove(); };

    }

    void OnBtnClose(GameObject obj)
    {
        buildhometown.m_instance.SetBoxSelect("");
        buildhometown.m_instance.SetBoxCollider(true);
        if (buildhometown.m_instance.m_curBuild != null)
        {
            Destroy(buildhometown.m_instance.m_curBuild);
        }
        UIManager.Instance.PopSelf(false);
    }

    void OnEvBuildHomeTown(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isBuild = exdata.GetData();
        if (isBuild == false)
        {
            if (UIManager.Instance.IsTopPanel(m_type))
            {
                UIManager.Instance.PopSelf(false);
            }
        }
    }
}
