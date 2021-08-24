using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using ProtoDefine;
using SGF.Codec;

public enum EnJjscType
{
    Gold = 0,
    Diamond=1
}
public class jiajushangchengpanel : UGUIPanel {

    public LoopListView2 ScrollView;
    public LoopListView2 ScrollView_0;

    public Image xstp;
    public Text wpsl;
    public Text jsjg;
    public Text cnname;
    public GameObject jia;
    public GameObject jian;
    public GameObject back;
    public GameObject ljqw;
    public GameObject ok;
    public GameObject xsmx;
    public GameObject zsjm;
    public GameObject img_zt;
    public GameObject img_ct;
    public GameObject is_obj_existence;
    public GameObject ti_Text;
    public GameObject ti_but;
    public Image goid_0;
    public Image goid_1;
    public Text goid_2;
    static int idx = 0;
    //运算下标

    static int shuliang = 0;
    //生成数量

    static int shuliang_0 = 0;
    //类别选择生成数量

    static int gmsl_num = 1;
    //购买数量逻辑

    static int jsjg_num = 0;
    //结算价格逻辑

    static long id;
    //物品ID

    bool but_img_io=false;


    int my_type;
    //保存当前是金币还是钻石类型

    int goid_num;
    //保存当前金币或者钻石的数量

     static string modleData;
    //当前选择的模型数据
    static double minscale;
    static double maxscale;
    //模型缩放比例

    Dictionary<int, List<PartProperties>> m_dicPageItem = new Dictionary<int, List<PartProperties>>();

    /// <summary>
    /// 商城实例
    /// </summary>
    public static jiajushangchengpanel instance;


    int m_selectPage = -1;

    int mItemCountPerRow = 2;
    void DataInit()
    {
        m_selectPage = 0;
    }

    private void Awake()
    {
        instance = this;
        DataInitOnlyOnce();
        shuliang_0 = DataMgr.m_dicHomeUnitOutDoorPage.Count + DataMgr.m_dicHomeUnitInDoorPage.Count;//m_dicPageItem.Count; 
        ScrollView.InitListView(1, OnGetItemByIndex);
        ScrollView_0.InitListView(1, OnGetItemByIndex_0);
    }
    public static bool kaiguai = false;

    void Start()
    {
        ClickListener.Get(back).onClick = back_;
        ClickListener.Get(ljqw).onClick = ljqw_;
        ClickListener.Get(ok).onClick = ok_;

        ClickListener.Get(xsmx).onClick = Open_model;
        ClickListener.Get(is_obj_existence).onClick = Open_model;
      /*  obj =>
      {
          if (!kaiguai) {
              UIManager.Instance.PushPanel(UIPanelName.showmodelpanel, false, false,(param)=> {
                  rete_mx showModel = param.GetComponent<rete_mx>();
                  showModel.CreateModel(modleData);
              });
              kaiguai = true;
          }
      };
      */
    }

    void Update()
    {

    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspBuyHousePartMessage, OnNetRspBuyHousePartMessage);
        DataInit();
        Senchen_0(shuliang_0);
        //shuliang_0传入的值会在开始的时候赋值 室内信息的长度加室外信息的长度
      //  AssetMgr.Instance.CreateSpr(m_dicPageItem[0][0].iconName, "homeuniticon", (spr) => { xstp.sprite = spr; });
    }


    /// <summary>
    /// 把全部信息遍历分类
    /// </summary>
    void DataInitOnlyOnce()
    {
        for (int i = 0; i < DataMgr.partProperties.Count; i++)
        {
            PartProperties info = DataMgr.partProperties[i];
            int key = info.inOutType * DataMgr.m_dicHomeUnitInDoorPage.Count + info.type;
            //如果取到的物品是室外物品Key的值会是室内物品长度再加物品类型（这样值就是8 9 或者10）
                if (m_dicPageItem.ContainsKey(key) == false)
                {
                    m_dicPageItem[key] = new List<PartProperties>();
                    m_dicPageItem[key].Add(info);
                    //不存在就生成一个
                }
                else
                {
                    m_dicPageItem[key].Add(info);
                }
                //最后m_dicPageItem前段就是室内物品后段就是室外物品
        }
    }


    /// <summary>
    /// true 为钻石
    /// false 为金币
    /// </summary>
    /// <param name="type"></param>
    public void set_type(int type)
    {
        my_type = type;
        if (type == (int)EnJjscType.Diamond)
        {
            AssetMgr.Instance.CreateSpr("DiamodColor", "commonicon", (spr) => { goid_0.sprite = spr; goid_1.sprite = spr; });
            goid_2.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.diamondNum);
            ti_Text.SetActive(false);
            ti_but.SetActive(false);
        }
        if (type == (int)EnJjscType.Gold)
        {
            AssetMgr.Instance.CreateSpr("GoldGreen", "commonicon", (spr) => { goid_0.sprite = spr; goid_1.sprite = spr; });
            goid_2.text = PublicFunc.GetAmountInt((int) DataMgr.m_account.wallet.goldNum);
            ti_Text.SetActive(true);
            ti_but.SetActive(true);
        }

        m_dicPageItem = new Dictionary<int, List<PartProperties>>();
        for (int i = 0; i < DataMgr.partProperties.Count; i++)
        {
            PartProperties info = DataMgr.partProperties[i];
            int key = info.inOutType * DataMgr.m_dicHomeUnitInDoorPage.Count + info.type;
            //如果取到的物品是室外物品Key的值会是室内物品长度再加物品类型（这样值就是8 9 或者10）
            if (type== (int)EnJjscType.Diamond)
            {
                if (info.diamond==0) { continue; }
            }
            if(type== (int)EnJjscType.Gold)
            {
                if (info.gold==0) { continue; }
            }
            if (m_dicPageItem.ContainsKey(key) == false)
            {
                m_dicPageItem[key] = new List<PartProperties>();
                m_dicPageItem[key].Add(info);
                //不存在就生成一个
            }
            else
            {
                m_dicPageItem[key].Add(info);
            }
            //最后m_dicPageItem前段就是室内物品后段就是室外物品
        }
        // shuaxin(m_dicPageItem[0].Count);
        shuaxin(0);
    }

    /// <summary>
    /// 打开模型界面
    /// </summary>
    /// <param name="obj"></param>
    void Open_model(GameObject obj)
    {
        if (!kaiguai)
        {
            UIManager.Instance.PushPanel(UIPanelName.showmodelpanel, false, false, (param) => {
                rete_mx showModel = param.GetComponent<rete_mx>();
                showModel.CreateModel(modleData,(float)maxscale,(float)minscale);
            },true);
            kaiguai = true;
        }
    }

    /// <summary>
    /// 修改金币或钻石物体
    /// </summary>
    void goid_set(int type)
    {
            if (type == (int)EnJjscType.Diamond)
            {
                goid_2.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.diamondNum);
                goid_num = (int)DataMgr.m_account.wallet.diamondNum;
            }
            if (type == (int)EnJjscType.Gold)
            {
                goid_2.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.goldNum);
                goid_num =(int)DataMgr.m_account.wallet.goldNum;
            }
    }

    /// <summary>
    /// 按钮点击事件绑定
    /// </summary>
    /// <param name="obj"></param>
    public void clickshangpin(GameObject obj)
    {
        //组件改成获取一次统一使用
        Transform img = obj.transform.GetChild(0);
        xstp.sprite = img.GetComponent<Image>().sprite;

        Transform name = obj.transform.GetChild(2);
        cnname.text = name.GetComponent<Text>().text;

        Transform qian = obj.transform.GetChild(3);
        jsjg.text = qian.GetComponent<Text>().text;

        jsjg_num = int.Parse(qian.GetComponent<Text>().text);
        gmsl_num = 1;
        wpsl.text = "1";
        modleData = obj.GetComponent<JjscItem>().modleData;
        minscale = obj.GetComponent<JjscItem>().m_info.minScale;
        maxscale = obj.GetComponent<JjscItem>().m_info.maxScale;
        id =(long)obj.GetComponent<JjscItem>().m_info.id;
        if (but_img_io)
        {
            img_ct.SetActive(true);
            but_img_io = false;
        }

        Debug.Log("我的模型信息"+ modleData);

        img_ct.transform.parent = obj.transform;
        img_ct.transform.position = obj.transform.position;


        goid_set(my_type);
      //  goid_2.text = PublicFunc.GetAmountInt((goid_num-int.Parse(jsjg.text)));
    }

    /// <summary>
    /// (按下按钮切换类型)生成表的时候调用
    /// </summary>
    /// <param name="num"></param>
    void Senchen(int num)
    {
        ScrollView.SetListItemCount(num);
        ScrollView.RefreshAllShownItem();
    }

    void Senchen_0(int num)
    {
        ScrollView_0.SetListItemCount(num);
        ScrollView_0.RefreshAllShownItem();
    }

    bool kaisi=true;
    /// <summary>
    /// 把生成的子物体名字（string）赋值给LoopListViewItem2
    /// </summary>
    /// <param name="listView"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= shuliang)
        {
            return null;
        }

        LoopListViewItem2 item = listView.NewListViewItem("shenchen");

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        int ItemChildIndex;
        for (int i = 0; i < mItemCountPerRow; i++)
        {
            ItemChildIndex = index * mItemCountPerRow + i;
            if (ItemChildIndex >= m_dicPageItem[m_selectPage].Count)
            {
                item.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            else
            {
                item.transform.GetChild(i).gameObject.SetActive(true);
            }
            item.transform.GetChild(i).GetComponent<JjscItem>().UpdateInfo(m_dicPageItem[m_selectPage][ItemChildIndex]);
            ClickListener.Get(item.transform.GetChild(i).gameObject).onClick = clickshangpin;
            if (kaisi&& index==1)
            {
                clickshangpin(item.transform.GetChild(i).gameObject);
                
                 AssetMgr.Instance.CreateSpr(item.transform.GetChild(i).GetComponent<JjscItem>().m_info.iconName,
                                             "homeuniticon", (spr) => { xstp.sprite = spr; });
                kaisi = false;
            }
          //  if (ItemChildIndex == 0) { clickshangpin(item.transform.GetChild(i).gameObject); }
        }
        return item;
    }

    LoopListViewItem2 OnGetItemByIndex_0(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= shuliang_0)
        {
            return null;
        }

        LoopListViewItem2 item = listView.NewListViewItem("But_0");

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }
        //生成的时候根据下标来逐个图片赋值（室内前半段）
        if (index < DataMgr.m_dicHomeUnitInDoorPage.Count)
        {
            item.transform.GetChild(0).GetComponent<Text>().text = DataMgr.m_dicHomeUnitInDoorPage[index.ToString()];
            item.transform.GetChild(1).GetComponent<Text>().text = index.ToString();
            ClickListener.Get(item.transform.gameObject).onClick = Side_selection;
            if (index == 0)
            {
               // tpxz(item.gameObject);
                Side_selection(item.gameObject);
                Debug.Log(item.transform.GetChild(0).GetComponent<Text>().text);
               // shuaxin(0);
            }
        }
        else if (index < m_dicPageItem.Count + DataMgr.m_dicHomeUnitOutDoorPage.Count) //室外后半段 （全部范围是室内加室外）
        {
            item.transform.GetChild(0).GetComponent<Text>().text = DataMgr.m_dicHomeUnitOutDoorPage[(index - DataMgr.m_dicHomeUnitInDoorPage.Count).ToString()];
            item.transform.GetChild(1).GetComponent<Text>().text = index.ToString();
            ClickListener.Get(item.transform.gameObject).onClick = Side_selection;
        }
        // Debug.Log(index);
        return item;
    }

    /// <summary>
    /// 关闭时调用
    /// </summary>
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspBuyHousePartMessage, OnNetRspBuyHousePartMessage);
    }


    /// <summary>
    /// 购买函数
    /// </summary>
    /// <param name="buf"></param>
    void OnNetRspBuyHousePartMessage(byte[] buf)
    {
        RspBuyHousePartMessage rsp = PBSerializer.NDeserialize<RspBuyHousePartMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else {
            Hint.LoadTips("购买成功", Color.white);
            //VirtualCityMgr.UpdateWallet(rsp.gold, rsp.diament, 0, 0);

            goid_set(my_type);
        }
    }


    // Use this for initialization

    // Update is called once per frame


    void back_(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    void ljqw_(GameObject obj)
    {
        VirtualCityMgr.GotoShanYeJie();
    }
    void ok_(GameObject obj)
    {
        PartProperties info = DataMgr.m_dicPartProperties[id];
        
        UIManager.Instance.PushPanel(Vc.AbName.paypanel, false, true, (param) =>
        {
            paypanel pay = param.GetComponent<paypanel>();
            pay.SetContent("提示", "确定购买" + info.cnName, info.gold* gmsl_num, info.diamond* gmsl_num);
            pay.m_GoldPay = () =>
            {
                ReqBuyHousePartMessage m_ReqBHPM = new ReqBuyHousePartMessage();
                m_ReqBHPM.housePartsId = id;
                m_ReqBHPM.number = gmsl_num;
                m_ReqBHPM.costDiamond = 0;
                Debug.Log("完成购买我花了:" + (jsjg_num * gmsl_num).ToString());
                Debug.Log("id:" + id + "数量:" + gmsl_num.ToString());
                GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqBuyHousePartMessage, m_ReqBHPM);
          
            };
            pay.m_DiamondPay = () =>
            {
                ReqBuyHousePartMessage m_ReqBHPM = new ReqBuyHousePartMessage();
                m_ReqBHPM.housePartsId = id;
                m_ReqBHPM.number = gmsl_num;
                m_ReqBHPM.costDiamond = 1;
                Debug.Log("完成购买我花了:" + (jsjg_num * gmsl_num).ToString());
                Debug.Log("id:" + id + "数量:" + gmsl_num.ToString());
                GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqBuyHousePartMessage, m_ReqBHPM);
              
            };
        }, true);

    }

    public void wpjs(int num)
    {
        if (gmsl_num + num >= 1)
        {
            gmsl_num += num;
            wpsl.text = gmsl_num.ToString();
            jsjg.text = (jsjg_num * gmsl_num).ToString();


            if (num == 1)
            {
             //   goid_2.text = PublicFunc.GetAmountInt( goid_num - jsjg_num * gmsl_num);
            }
            else
            {
             //   goid_2.text = PublicFunc.GetAmountInt(goid_num + jsjg_num * gmsl_num);
            }
        }
    }


    void Side_selection(GameObject obj)
    {
        if (!but_img_io)
        {
            img_ct.SetActive(false);
            but_img_io = true;
        }
        tpxz(obj);
        shuaxin(int.Parse(obj.transform.GetChild(1).GetComponent<Text>().text));
    }



    void tpxz(GameObject obg)
    {
        img_zt.transform.parent = obg.transform;
        img_zt.transform.position = obg.transform.GetChild(1).position;
        img_zt.transform.GetChild(0).GetComponent<Text>().text = obg.transform.GetChild(0).GetComponent<Text>().text;
    }

    void shuaxin(int num)
    {
        m_selectPage = num;
        idx = 0;
        try
        {
            shuliang = m_dicPageItem[m_selectPage].Count / mItemCountPerRow;
            if (m_dicPageItem[m_selectPage].Count % mItemCountPerRow > 0)
            {
                shuliang++;
            }
        }
        catch 
        {
            Debug.Log("这个分类没有数据");
            Senchen(0);
            return;
        }
        Senchen(shuliang);
    }
}