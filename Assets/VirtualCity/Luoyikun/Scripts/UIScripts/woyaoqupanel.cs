using Framework.Event;
using Framework.UI;
using ProtoDefine;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NameAndNav
{
    public string name;
    public UnityAction act;
}

public enum EnNavQu
{
    MyHometown,
    TaHometown,
    ShangYeJie
}
public class woyaoqupanel : UGUIPanel
{
    public LoopListView2 m_scroll;
    public GameObject m_btnClose;
    int m_max;
    List<string> m_listName = new List<string>();
    bool m_isInit = false;

    static Dictionary<string, string> m_dicKeyAndCh = new Dictionary<string, string>();
    static Dictionary<string, List<string>> m_dicNav = new Dictionary<string, List<string>>();

    const string MyHometown = "MyHometown";
    const string TaHometown = "TaHometown";
    const string ShangYeJie = "ShangYeJie";
    const string ShopBuy = "ShopBuy";
    private const string SearchGoods = "SearchGoods";
    string m_curKeyInNav = "";
    bool m_isInitNavData = false;
    // Use this for initialization
    void Start()
    {
        //NavData();
        //NavData();
        ClickListener.Get(m_btnClose).onClick = OnBtnClose;
    }

    void OnBtnClose(GameObject obj)
    {
        if (m_isOpen == false)
        {
            return;
        }
        Debug.Log("点击了导航关闭");
        UIManager.Instance.PopSelf();
    }
    private void Awake()
    {

    }
    public static void NavData()
    {
        m_dicKeyAndCh[MyHometown] = "去我的家园";
        m_dicKeyAndCh[TaHometown] = "去TA的家园";
        m_dicKeyAndCh[ShangYeJie] = "去商业街";
        m_dicKeyAndCh[ShopBuy] = "购买商品";
        m_dicKeyAndCh[SearchGoods] = "搜索商品";
        foreach (var item in DataMgr.m_dicShopsProperties)
        {
            ShopsProperties shop = item.Value;
            m_dicKeyAndCh[shop.id.ToString()] = "去" + shop.name;
        }

        //我的家园
        string key = EnMyOhter.My.ToString() + EnCurScene.Hometown.ToString();
        m_dicNav[key] = new List<string>();
        m_dicNav[key].Add(ShangYeJie);

        //我的家
        key = EnMyOhter.My.ToString() + EnCurScene.Home.ToString();
        m_dicNav[key] = new List<string>();
        m_dicNav[key].Add(MyHometown);
        m_dicNav[key].Add(ShangYeJie);

        //商业街
        key = EnMyOhter.My.ToString() + EnCurScene.Business.ToString();
        m_dicNav[key] = new List<string>();
        m_dicNav[key].Add(MyHometown);
        //m_dicNav[key].Add(SearchGoods);
        foreach (var item in m_dicKeyAndCh)
        {
            long id = 0;
            if (long.TryParse(item.Key, out id) == true)
            {
                m_dicNav[key].Add(item.Key);
            }
        }

        //商店
        key = EnMyOhter.My.ToString() + EnCurScene.Shop.ToString();
        m_dicNav[key] = new List<string>();
        m_dicNav[key].Add(ShangYeJie);
        m_dicNav[key].Add(MyHometown);
        m_dicNav[key].Add(ShopBuy);

        //他的家园
        key = EnMyOhter.Other.ToString() + EnCurScene.Hometown.ToString();
        m_dicNav[key] = new List<string>();
        m_dicNav[key].Add(MyHometown);
        m_dicNav[key].Add(ShangYeJie);

        //他的家
        key = EnMyOhter.Other.ToString() + EnCurScene.Home.ToString();
        m_dicNav[key] = new List<string>();
        m_dicNav[key].Add(TaHometown);
        m_dicNav[key].Add(MyHometown);
        m_dicNav[key].Add(ShangYeJie);
    }
    public void InfoUpdate()
    {
        m_curKeyInNav = DataMgr.m_myOther.ToString() + DataMgr.m_curScene.ToString();
        Debug.Log("当前向导：" + m_curKeyInNav);
        if (m_dicNav.ContainsKey(DataMgr.m_myOther.ToString() + DataMgr.m_curScene.ToString()) == false)
        {
            Debug.Log("不包含当前状态导航");
            return;
        }
        m_listName = m_dicNav[m_curKeyInNav];
        if (m_isInit == false)
        {
            m_scroll.InitListView(m_listName.Count, OnGetItemByIndexCommonHouse);
            m_isInit = true;
        }
        m_max = m_listName.Count;

        for (int i = 0; i < m_scroll.transform.Find("Viewport/Content").childCount; i++)
        {
            m_scroll.transform.Find("Viewport/Content").GetChild(i).name = "";
        }
        m_scroll.SetListItemCount(m_max);
        m_scroll.RefreshAllShownItem();
        StartCoroutine(YieldToItem());
        //m_scroll.ResetListView();
    }

    IEnumerator YieldToItem()
    {
        yield return 1;
        m_scroll.MovePanelToItemIndex(0, 0);
    }

    LoopListViewItem2 OnGetItemByIndexCommonHouse(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= m_max)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("Item");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            ClickListener.Get(item.gameObject).onClick = OnBtnItem;
            //itemScript.Init();
        }
        item.name = m_dicNav[m_curKeyInNav][index];
        item.transform.Find("Text").GetComponent<Text>().text = m_dicKeyAndCh[m_dicNav[m_curKeyInNav][index]];
        return item;
    }

    void OnBtnItem(GameObject obj)
    {
        long shopId = 0;
        UIManager.Instance.PopSelf(false);
        if (long.TryParse(obj.name, out shopId) == true)
        {
            //去商业街的某个点
            ShopsProperties shop = DataMgr.m_dicShopsProperties[shopId];
            Vector3 pos = new Vector3(shop.x, shop.y, shop.z);
            EventManager.Instance.DispatchEvent(Common.EventStr.PlayerNavGotoPoint, new EventDataEx<Vector3>(pos));
        }
        else
        {
            switch (obj.name)
            {
                case MyHometown:
                    VirtualCityMgr.GotoHometown(EnMyOhter.My);
                    break;
                case TaHometown:
                    VirtualCityMgr.GotoHometown(EnMyOhter.Other, DataMgr.m_taInfo);
                    break;
                case ShangYeJie:
                    VirtualCityMgr.GotoShanYeJie();
                    //NewGuideMgr.Instance.StartOneNewGuide();
                    break;
                case ShopBuy:
                    UIManager.Instance.PushPanel(Vc.AbName.shoppingmunepanel, false, false, (param) =>
                    {
                        shoppingmunepanel chat = param.GetComponent<shoppingmunepanel>();
                        ShopsProperties info = DataMgr.m_dicShopsProperties[ShopMgr.m_instance.m_shopId];
                        chat.ReqGGLM(info.businessId);
                        //NewGuideMgr.Instance.StartOneNewGuide();
                    }
                    );
                    break;
                case SearchGoods:
                    UIManager.Instance.PushPanel(UIPanelName.searchgoodspanel);
                    break;
                default:
                    break;
            }
        }


    }
}
