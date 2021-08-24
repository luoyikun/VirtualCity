using Framework.Event;
using Framework.UI;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EnHtSelectType
{
    Home,
    Building,
}
public class homeselectbuildpanel : UGUIPanel {

    public Transform m_togglePar;
    //public LoopListView2 m_scrollHome;
    public LoopListView2 m_scrollCommonHouse;
    EnHtSelectType m_selectType = EnHtSelectType.Home;

    int m_maxItem = 0;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < m_togglePar.childCount; i++)
        {
            TabEvent tab = m_togglePar.GetChild(i).GetComponent<TabEvent>();
            tab.m_onValueChange = OnValueChange;
        }


        m_togglePar.GetChild(0).GetComponent<TabEvent>().SetOn(true);

        //m_maxItem = 
        //m_scrollHome.InitListView(DataMgr.homeProperties.Count, OnGetItemByIndexHome);
        //m_scrollCommonHouse.InitListView(DataMgr.devlopmentProperties.Count, OnGetItemByIndexCommonHouse);
        m_maxItem = DataMgr.homeProperties.Count;
        m_scrollCommonHouse.InitListView(DataMgr.homeProperties.Count, OnGetItemByIndexCommonHouse);
    }

    public override void OnOpen()
    {
        EventManager.Instance.AddEventListener(Common.EventStr. BuildHomeTown, OnEvBuildHomeTown);
    }

    public override void OnClose()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeTown);
    }

    void OnEvBuildHomeTown(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isBuild = exdata.GetData();
        if (isBuild == false)
        {
            if (UIManager.Instance.IsTopPanel(m_type))
            {
                UIManager.Instance.PopSelf();
            }
        }
    }
    void OnValueChange(string id, bool value, Transform trans)
    {
        if (value == true)
        {
            switch (id)
            {
                case "0":
                    m_maxItem = DataMgr.homeProperties.Count;

                    m_selectType = EnHtSelectType.Home;
                    break;
                case "1":
                    m_maxItem = DataMgr.devlopmentProperties.Count;

                    m_selectType = EnHtSelectType.Building;

                    break;
                default:
                    break;
            }
            trans.Find("Image").GetComponent<Image>().color = PublicFunc.StringToColor("ffffff");
            trans.Find("Label").GetComponent<Text>().color = PublicFunc.StringToColor("ffffff");
        }
        else
        {
            trans.Find("Image").GetComponent<Image>().color = PublicFunc.StringToColor("0A7AE8");
            trans.Find("Label").GetComponent<Text>().color = PublicFunc.StringToColor("0A7AE8");
        }

        m_scrollCommonHouse.SetListItemCount(m_maxItem);
        m_scrollCommonHouse.RefreshAllShownItem();
    }

    LoopListViewItem2 OnGetItemByIndexCommonHouse(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= m_maxItem)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("Item");
        HomeTownItem homeTown = item.GetComponent<HomeTownItem>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            ClickListener.Get(item.gameObject).onClick = OnBtnClickCommonHouseItem;
            //itemScript.Init();
        }

        StartCoroutine(YieldSetCommonBuildInfo(homeTown, index));
    
        
        return item;
    }


    IEnumerator YieldSetCommonBuildInfo(HomeTownItem homeTown,int index)
    {
        homeTown.SetCommonBuildInfo(index, m_selectType);
        yield return null;
    }

    LoopListViewItem2 OnGetItemByIndexHome(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= DataMgr.homeProperties.Count)
        {
            return null;
        }

        
        LoopListViewItem2 item = listView.NewListViewItem("Item");
        //ListItem2 itemScript = item.GetComponent<ListItem2>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            ClickListener.Get(item.gameObject).onClick = OnBtnClickHomeItem;
            //itemScript.Init();
        }
        //itemScript.SetItemData(itemData, index);
        //item.transform.Find("Text").GetComponent<Text>().text = index.ToString();


        return item;
    }

    void OnBtnClickHomeItem(GameObject obj)
    {
        int idx = obj.GetComponent<LoopListViewItem2>().ItemIndex;
        Debug.Log("HomeItem:" + idx);
    }

    void OnBtnClickCommonHouseItem(GameObject obj)
    {
        int idx = obj.GetComponent<LoopListViewItem2>().ItemIndex;
        Debug.Log("CommonHouse:" + idx);
        switch (m_selectType)
        {
            case EnHtSelectType.Home:
                {
                    long id = (long)DataMgr.homeProperties[idx].id;
                    buildhometown.m_instance.CreateHomeByClick(id);
                    buildhometown.m_instance.SetBoxCollider(false);
                    buildhometown.m_instance.m_selectType = EnLand.Home;
                    UIManager.Instance.PushPanelDeleteSelf(UIPanelName.hometownborpanel, false, (param) =>
                    {
                        hometownborpanel bor = param.GetComponent<hometownborpanel>();
                        bor.SetHomeInfo(id, EnBoR.Build);
                    });
                }
                break;
            case EnHtSelectType.Building:
                {
                    long id = (long)DataMgr.devlopmentProperties[idx].id;
                    buildhometown.m_instance.CreateCommonBuildByClick(id);
                    buildhometown.m_instance.SetBoxCollider(false);
                    buildhometown.m_instance.m_selectType = EnLand.CommonBuild;
                    UIManager.Instance.PushPanelDeleteSelf(UIPanelName.hometownborpanel, false, (param) =>
                    {
                        hometownborpanel bor = param.GetComponent<hometownborpanel>();
                        bor.SetCommonBuildInfo(id, EnBoR.Build);
                    });
                }
                break;
            default:
                break;
        }
    }
}
