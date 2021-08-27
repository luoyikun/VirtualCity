using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EnInOutDoor
{
    InDoor,
    OutDoor
}
public class homeunitselectpanel : UGUIPanel {

    public GameObject m_btnBuy;
    public LoopListView2 m_scrollPage;

    public LoopListView2 m_scrollItem;

    bool m_isScrollInit = false;

    public Transform m_pagePar;
    int m_selectIdx = 0; //选中的page

    //List<string> m_listPage = new List<string>();
    //Dictionary<string, List<PartProperties>> m_dicItem = new Dictionary<string, List<PartProperties>>();

    public GameObject m_btnLayer;
    int m_curLayer = 1;

    public Text m_textLayer;
    EnInOutDoor m_enInOut = EnInOutDoor.InDoor;

    public Transform m_togglePar;

    int m_maxItem = 0;
    int m_maxPage = 0;

    List<HouseParts> m_listHomeUnit = new List<HouseParts>() { new HouseParts()};
    Dictionary<EnInOutDoor, Dictionary<long, List<HouseParts>>> m_dicShow = new Dictionary<EnInOutDoor, Dictionary<long, List<HouseParts>>>()
    {
        {EnInOutDoor.InDoor,new Dictionary<long, List<HouseParts>>() },
        {EnInOutDoor.OutDoor,new Dictionary<long, List<HouseParts>>() },
    };
    // Use this for initialization
    void Start() {

        m_dicShow[EnInOutDoor.InDoor] = new Dictionary<long, List<HouseParts>>();
        m_dicShow[EnInOutDoor.OutDoor] = new Dictionary<long, List<HouseParts>>();


        ClickListener.Get(m_btnBuy).onClick = OnBtnBuy;
        ClickListener.Get(m_btnLayer).onClick = OnBtnLayer;
        


        for (int i = 0; i < m_togglePar.childCount; i++)
        {
            TabEvent tab = m_togglePar.GetChild(i).GetComponent<TabEvent>();
            tab.m_onValueChange = OnValueChange;
        }
        

        //OnPageSet(0);
    }

    public void OnBtnBuy(GameObject obj)
    {
        UIManager.Instance.PushPanel(Vc.AbName.jiajushangchengpanel, false, false, (param) => {
            jiajushangchengpanel jjsc = param.GetComponent<jiajushangchengpanel>();
            jjsc.set_type((int)EnJjscType.Gold);
        }, true);

    }
    public override void OnOpen()
    {
        homepanel.m_instance.BtnExitBuildSet(true);
        if (HomeMgr.m_instance.m_isFirtEnter == true)
        {
            UpdateLayerIdx(1);
            HomeMgr.m_instance.m_isFirtEnter = false;
        }
        
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetHousePartMessage, OnNetRspGetHousePartMessage);

        if (AppConst.m_isOffline == true)
        {
            DataDeal();
            SetScroll();

            ScroolInit();
            m_togglePar.GetChild(0).GetComponent<TabEvent>().SetOn(true);
        }
        else {

            SendReqGetHousePartMessage();
        }
    }

    void OnNetRspGetHousePartMessage(byte[] buf)
    {
        RspGetHousePartMessage rsp = PBSerializer.NDeserialize<RspGetHousePartMessage>(buf);
        if (rsp.housePartsList == null)
        {
            m_listHomeUnit = new List<HouseParts>();
        }
        else
        {
            m_listHomeUnit = rsp.housePartsList;
        }
        DataDeal();
        SetScroll();

        ScroolInit();
        m_togglePar.GetChild(0).GetComponent<TabEvent>().SetOn(true);

        
    }

    void DataDeal()
    {
        m_dicShow[EnInOutDoor.InDoor].Clear();
        m_dicShow[EnInOutDoor.OutDoor].Clear();

        foreach (var item in DataMgr.m_dicHomeUnitInDoorPage)
        {
            m_dicShow[EnInOutDoor.InDoor][long.Parse(item.Key)] = new List<HouseParts>();
        }

        foreach (var item in DataMgr.m_dicHomeUnitOutDoorPage)
        {
            m_dicShow[EnInOutDoor.OutDoor][long.Parse(item.Key)] = new List<HouseParts>();
        }
        
        for (int i = 0; i < m_listHomeUnit.Count; i++)
        {
            HouseParts part = m_listHomeUnit[i];
            PartProperties partCeHua = DataMgr.m_dicPartProperties[(long)part.moudelId];

            if (m_dicShow[(EnInOutDoor)partCeHua.inOutType].ContainsKey(partCeHua.type) == false)
            {
                m_dicShow[(EnInOutDoor)partCeHua.inOutType][partCeHua.type] = new List<HouseParts>();
            }
            m_dicShow[(EnInOutDoor)partCeHua.inOutType][partCeHua.type].Add(part);
        }

        
    }
    public override void OnClose()
    {
        homepanel.m_instance.BtnExitBuildSet(false);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetHousePartMessage, OnNetRspGetHousePartMessage);
    }


    void ScroolInit()
    {
        m_selectIdx = 0;
        m_maxPage = DataMgr.m_dicHomeUnitInDoorPage.Count;
        m_enInOut = EnInOutDoor.InDoor;
        m_maxItem = m_dicShow[EnInOutDoor.InDoor][0].Count;

        m_scrollPage.SetListItemCount(m_maxPage);
        m_scrollPage.RefreshAllShownItem();

        m_scrollItem.SetListItemCount(m_maxItem);
        m_scrollItem.RefreshAllShownItem();

        for (int i = 0; i < m_pagePar.childCount; i++)
        {
            homeunitselectPage item = m_pagePar.GetChild(i).GetComponent<homeunitselectPage>();
            item.SetSelect(m_selectIdx);
        }
    }
    void OnValueChange(string id, bool value, Transform trans)
    {
        m_selectIdx = 0; // 每次都归零
        if (value == true)
        {
            switch (id)
            {
                case "0":
                    m_maxPage = DataMgr.m_dicHomeUnitInDoorPage.Count;
                    m_enInOut = EnInOutDoor.InDoor;
                    m_maxItem = m_dicShow[EnInOutDoor.InDoor][0].Count;
                    break;
                case "1":
                    m_maxPage = DataMgr.m_dicHomeUnitOutDoorPage.Count;

                    m_enInOut = EnInOutDoor.OutDoor;
                    m_maxItem = m_dicShow[EnInOutDoor.OutDoor][0].Count;
                    break;
                default:
                    break;
            }
            trans.Find("Image").GetComponent<Image>().color = PublicFunc.StringToColor("ffffff");
            trans.Find("Label").GetComponent<Text>().color = PublicFunc.StringToColor("ffffff");

            m_scrollPage.SetListItemCount(m_maxPage);
            m_scrollPage.RefreshAllShownItem();

            m_scrollItem.SetListItemCount(m_maxItem);
            m_scrollItem.RefreshAllShownItem();

        }
        else
        {
            trans.Find("Image").GetComponent<Image>().color = PublicFunc.StringToColor("0A7AE8");
            trans.Find("Label").GetComponent<Text>().color = PublicFunc.StringToColor("0A7AE8");
        }



    }

    void OnBtnLayer(GameObject obj)
    {
        m_curLayer++;
        if (m_curLayer > HomeMgr.m_instance.m_maxLayer)
        {
            m_curLayer = 1;
        }


        TextLayerUpdate();

        HomeMgr.m_instance.SetLayerShow(m_curLayer);
    }
    public void DataInit()
    {
        m_selectIdx = 0;
    }
    public void SetScroll()
    {
        if (m_isScrollInit == false)
        {
            m_isScrollInit = true;
            m_scrollPage.InitListView(1, OnUpdatePage);
            m_scrollItem.InitListView(1, OnUpdateItem);
        }
    }


    LoopListViewItem2 OnUpdateItem(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= m_maxItem)
        {
            return null;
        }

        LoopListViewItem2 item = listView.NewListViewItem("Item");
        homeunitselectItem homeTown = item.GetComponent<homeunitselectItem>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            ClickListener.Get(item.gameObject).onClick = OnBtnItem;
            //itemScript.Init();
        }

        homeTown.OnUpdate(m_dicShow[m_enInOut][m_selectIdx][index]);
        //homeTown.UiUpdate(index, m_selectIdx, m_listPage[index]);
        //StartCoroutine(YieldSetCommonBuildInfo(homeTown, index));
        return item;

    }
    LoopListViewItem2 OnUpdatePage(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= m_maxPage)
        {
            return null;
        }

        LoopListViewItem2 item = listView.NewListViewItem("Item");
        homeunitselectPage homeTown = item.GetComponent<homeunitselectPage>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            ClickListener.Get(item.gameObject).onClick = OnBtnPage;
            //itemScript.Init();
        }

        if (m_enInOut == EnInOutDoor.InDoor)
        {
            homeTown.UiUpdate(index, m_selectIdx, DataMgr.m_dicHomeUnitInDoorPage[index.ToString()]);
        }
        else if (m_enInOut == EnInOutDoor.OutDoor)
        {
            homeTown.UiUpdate(index, m_selectIdx, DataMgr.m_dicHomeUnitOutDoorPage[index.ToString()]);
        }
           
        return item;

    }

    public void SendReqGetHousePartMessage()
    {
        ReqGetHousePartMessage req = new ReqGetHousePartMessage();
        req.accountId = (long)DataMgr.m_account.id;
        PublicFunc.ToGameServer(MsgIdDefine.ReqGetHousePartMessage, req);
    }

    void OnBtnItem(GameObject obj)
    {
        homeunitselectItem item = obj.GetComponent<homeunitselectItem>();

        HomeMgr.m_instance.CreateNewUnitByClick(item.m_info);
        homepanel.m_instance.BtnExitBuildSet(false);
    }
    void OnBtnPage(GameObject obj)
    {
        
        homeunitselectPage homeTown = obj.GetComponent<homeunitselectPage>();
        if (m_selectIdx == homeTown.m_idx)
        {
            return;
        }
        m_selectIdx = homeTown.m_idx;
        
        UpdateItemList();
        for (int i = 0; i < m_pagePar.childCount; i++)
        {
            homeunitselectPage item = m_pagePar.GetChild(i).GetComponent<homeunitselectPage>();
            item.SetSelect(m_selectIdx);
        }
    }

    void OnPageSet(int idxPage)
    {
        m_selectIdx = idxPage;
        for (int i = 0; i < m_pagePar.childCount; i++)
        {
            homeunitselectPage item = m_pagePar.GetChild(i).GetComponent<homeunitselectPage>();
            item.SetSelect(m_selectIdx);
        }
    }
    void UpdateItemList()
    {
        m_maxItem = m_dicShow[m_enInOut][m_selectIdx].Count;
        m_scrollItem.SetListItemCount(m_dicShow[m_enInOut][m_selectIdx].Count);
        m_scrollItem.RefreshAllShownItem();
    }

    public void UpdateLayerIdx(int floor)
    {
        m_curLayer = floor;
        TextLayerUpdate();
    }

    void TextLayerUpdate()
    {
        m_textLayer.text = m_curLayer.ToString();
    }
  

}
