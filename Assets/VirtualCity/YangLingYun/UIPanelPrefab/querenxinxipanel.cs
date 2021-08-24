using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;
using System;

public class querenxinxipanel : UGUIPanel
{
    public GameObject SwitchAddressBtn;
    public GameObject SelectAliPayBtn;
    public GameObject SelectWeiXinPayBtn;
    public GameObject GoodsPar;
    public GameObject GoodsTmp;
    public GameObject PayBtn;
    public GameObject BackBtn;
    public GameObject MainPar;
    public GameObject AddAddressBtn;
    RspCreateOrderMessage m_RspCOM;
    public Text NameText;
    public Text TelephoneText;
    public Text DiZhiText;
    public Text TotalPriceText;
    public Text GouWuJinText;
    public Text CashText;
    public Text PayPriceText;
    public Text HasProxyText;
    int? payType;
    Address Target_Address = new Address();
    Dictionary<long, ShoppingCartDic> Target_ShoppingCartDic = new Dictionary<long, ShoppingCartDic>();
    bool IsHasAddress=false;
    public static querenxinxipanel QRXXP;
    private void Start()
    {
        ClickListener.Get(SwitchAddressBtn).onClick = clickSwitchAddressBtn;
        ClickListener.Get(SelectAliPayBtn).onClick = clickSelectAliPayBtn;
        ClickListener.Get(SelectWeiXinPayBtn).onClick = clickSelectWeiXinPayBtn;
        ClickListener.Get(PayBtn).onClick = clickPayBtn;
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(AddAddressBtn).onClick = clickSwitchAddressBtn;
    }
    public override void OnOpen()
    {
        QRXXP = this;
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCreateOrderMessage, OnNetRspCOM);
        //Init
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AliComponent.Instance.aliPayCallBack += AliPayCallback;
        }
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCreateOrderMessage, OnNetRspCOM);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AliComponent.Instance.aliPayCallBack -= AliPayCallback;
        }
    }

    private void AliPayCallback(string result)
    {
        if (result == "true")
        {
            Debug.Log("支付宝支付成功");
            PayResult("1");
        }
        else
        {
            Debug.Log("支付宝支付失败");
            PayResult("0");

        }

    }

    void OnNetRspCOM(byte[] buf)
    {
        RspCreateOrderMessage RspCOM = PBSerializer.NDeserialize<RspCreateOrderMessage>(buf);
        m_RspCOM = RspCOM;
        if (RspCOM.code != 0)
        {
            if (RspCOM.payInfo == null)
            {
                PayResult("1");//如果payinfo等于null，代表游戏内货币支付成功，则直接进入支付成功
                return;
            }
            if (payType == 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidFunc.AliPay(RspCOM.payInfo, gameObject.name, "PayResult");
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    AliComponent.Instance.AliPay(RspCOM.payInfo);
                }
                //else if (Application.platform == RuntimePlatform.WindowsEditor)
                //{
                //    PayResult("1");
                //}
            }
            else if (payType == 1)
            {
                Hint.LoadTips("微信支付尚未开放", Color.white);
            }
        }
        else if (RspCOM.code == 0)
        {
            Hint.LoadTips(RspCOM.tips, Color.white);
        }
    }
    public void PayResult(string str)
    {
        int state = int.Parse(str);
        if (state == 0)
        {
            Hint.LoadTips("订单支付失败", Color.white);
        }
        else if (state == 1)
        {
            Dictionary<long, ShoppingCartDic> DataMgrAccountCart = JsonConvert.DeserializeObject<Dictionary<long, ShoppingCartDic>>(DataMgr.m_account.goodsList);
            List<long> targetKey = new List<long>();
            foreach (long key in DataMgrAccountCart.Keys)
            {
                foreach (long Key in Target_ShoppingCartDic.Keys)
                {
                    if (DataMgrAccountCart[key].goodsId == Target_ShoppingCartDic[Key].goodsId)
                    {
                        targetKey.Add(key);
                    }

                    if (goodsdetailspanel.gdp != null)
                    {
                        for (int i = 0; i < goodsdetailspanel.gdp.m_goodsKindList.Count; i++)
                        {
                            if (goodsdetailspanel.gdp.m_goodsKindList[i].id == Target_ShoppingCartDic[Key].goodsId)
                            {
                                goodsdetailspanel.gdp.m_goodsKindList[i].number -= Target_ShoppingCartDic[Key].number;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < targetKey.Count; i++)
            {
                DataMgrAccountCart.Remove(targetKey[i]);
            }
            DataMgr.m_account.goodsList = JsonConvert.SerializeObject(DataMgrAccountCart);
            UIManager.Instance.PopSelf();
            if (UIManager.Instance.IsTopPanel("guigepanel"))
            {
                UIManager.Instance.PopSelf();
            }

            Hint.LoadTips("支付成功", Color.white);
            DataMgr.m_account.wallet.sMoneyNum = m_RspCOM.sMoney;
            DataMgr.m_account.wallet.moneyNum = m_RspCOM.money;
        }
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    public void Init(Dictionary<long, ShoppingCartDic> m_ShoppingCartDic, List<GoodsKind> m_ListGoodsKind)
    {
        Target_ShoppingCartDic = m_ShoppingCartDic;
        List<Address> m_ListAddress = new List<Address>();
        List<GoodsKind> m_SelectGoodsKindList = new List<GoodsKind>();
        m_ListAddress = JsonConvert.DeserializeObject<List<Address>>(DataMgr.m_account.addressInfo);
        double totalprice = 0;
        if (m_ListAddress.Count != 0)
        {
            InitAddress(m_ListAddress[0]);
            for (int i = 0; i < m_ListAddress.Count; i++)
            {
                if (m_ListAddress[i].IsMoRen == true)
                {
                    InitAddress(m_ListAddress[i]);
                     break;
                }
            }
        }
        else if (m_ListAddress.Count == 0)
        {
            MainPar.transform.Find("Address").gameObject.SetActive(false);
            MainPar.transform.Find("AddAddress").gameObject.SetActive(true);
        }
        foreach (long Key in m_ShoppingCartDic.Keys)
        {
            for (int i = 0; i < m_ListGoodsKind.Count; i++)
            {
                if (m_ListGoodsKind[i].id == Key)
                {
                    m_SelectGoodsKindList.Add(m_ListGoodsKind[i]);
                }
            }
        }
        if (GoodsPar.transform.childCount != 0)
        {
            for (int i = GoodsPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(GoodsPar.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < m_SelectGoodsKindList.Count; i++)
        {
            GameObject obj = PublicFunc.CreateTmp(GoodsTmp, GoodsPar.transform);
            obj.transform.Find("GoodsName").GetComponent<Text>().text = m_SelectGoodsKindList[i].name;
            obj.transform.Find("GoodsPrice").GetComponent<Text>().text = m_SelectGoodsKindList[i].value.ToString() + "元";
            obj.transform.Find("GoodsNum").GetComponent<Text>().text = "x" + m_ShoppingCartDic[(long)m_SelectGoodsKindList[i].id].number.ToString();
            totalprice += (double)m_SelectGoodsKindList[i].value * m_ShoppingCartDic[(long)m_SelectGoodsKindList[i].id].number;
            obj.transform.Find("Image").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_SelectGoodsKindList[i].kindPicture);
        }
        InitMoney(totalprice, (double)DataMgr.m_account.wallet.sMoneyNum, (double)DataMgr.m_account.wallet.moneyNum);
        clickSelectAliPayBtn(SelectAliPayBtn);
    }
    public void Update()
    {
        //MainPar.GetComponent<VerticalLayoutGroup>().enabled = false;
        //MainPar.GetComponent<VerticalLayoutGroup>().enabled = true;
        //MainPar.GetComponent<ContentSizeFitter>().enabled = false;
        //MainPar.GetComponent<ContentSizeFitter>().enabled = true;
        //GoodsPar.GetComponent<GridLayoutGroup>().enabled = false;
        //GoodsPar.GetComponent<GridLayoutGroup>().enabled = true;
        //GoodsPar.GetComponent<ContentSizeFitter>().enabled = false;
        //GoodsPar.GetComponent<ContentSizeFitter>().enabled = true;
    }
    private void LateUpdate()
    {
        MainPar.GetComponent<VerticalLayoutGroup>().enabled = false;
        MainPar.GetComponent<VerticalLayoutGroup>().enabled = true;
        MainPar.GetComponent<ContentSizeFitter>().enabled = false;
        MainPar.GetComponent<ContentSizeFitter>().enabled = true;
        GoodsPar.GetComponent<GridLayoutGroup>().enabled = false;
        GoodsPar.GetComponent<GridLayoutGroup>().enabled = true;
        GoodsPar.GetComponent<ContentSizeFitter>().enabled = false;
        GoodsPar.GetComponent<ContentSizeFitter>().enabled = true;
    }
    public void InitAddress(Address m_Address)
    {
        MainPar.transform.Find("Address").gameObject.SetActive(true);
        MainPar.transform.Find("AddAddress").gameObject.SetActive(false);
        Target_Address = m_Address;
        NameText.text = m_Address.m_RA.Name;
        TelephoneText.text = m_Address.m_RA.Mobile.ToString();
        DiZhiText.text = m_Address.m_RA.ProvinceName + "-"+m_Address.m_RA.CityName+"-"+m_Address.m_RA.ExpAreaName+"-" + m_Address.m_RA.Address;
        IsHasAddress = true;
    }
    void clickPayBtn(GameObject obj)
    {
        if (IsHasAddress == true)
        {
            List<Order> m_ListOrder = new List<Order>();
            foreach (long Key in Target_ShoppingCartDic.Keys)
            {
                Order m_Order = new Order();
                m_Order.goodsId = Target_ShoppingCartDic[Key].goodsId;
                m_Order.accountId = (long)DataMgr.m_account.id;
                m_Order.goodsKindId = Key;
                m_Order.goodsNum = Target_ShoppingCartDic[Key].number;
                m_Order.payType = (int)payType;
                m_Order.expressInfo = JsonConvert.SerializeObject(Target_Address);
                m_Order.customerTel = Target_Address.m_RA.Mobile.ToString();
                m_ListOrder.Add(m_Order);
            }
            if ((int)payType == 1)
            {
                Hint.LoadTips("微信支付暂未开放", Color.white);
                return;
            }
            ReqCreateOrderMessage m_ReqCOM = new ReqCreateOrderMessage();
            m_ReqCOM.order = m_ListOrder;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCreateOrderMessage, m_ReqCOM);
        }
        else if (IsHasAddress == false)
        {
            Hint.LoadTips("请先填写收货地址", Color.white);
        }
    }
    void InitMoney(double TotalMoneny, double GouWuJin, double Cash)
    {
        double GouWuJinDouble = 0;
        double CashDouble = 0;
        double ShiFuJinE = 0;
        TotalPriceText.text = TotalMoneny.ToString();

        if (GouWuJin >= TotalMoneny)
        {
            GouWuJinDouble = GouWuJin - (GouWuJin - TotalMoneny);
            GouWuJinText.text = "-" + GouWuJinDouble.ToString("0.00") + "元";
            //PayPriceText.text = ShiFuJinE.ToString();
            CashText.transform.parent.gameObject.SetActive(false);
        }
        else if (GouWuJin < TotalMoneny)
        {
            GouWuJinDouble = GouWuJin;

            GouWuJinText.text = "-" + GouWuJinDouble.ToString("0.00") + "元";

            if (Cash >= TotalMoneny - GouWuJinDouble)
            {
                CashText.gameObject.SetActive(true);
                CashDouble = Cash - (Cash-(TotalMoneny - GouWuJinDouble));
                CashText.text = "-" + CashDouble.ToString("0.00") + "元";
            }
            if (Cash < TotalMoneny - GouWuJinDouble)
            {
                CashDouble = Cash;
                CashText.text = "-" + CashDouble.ToString("0.00") + "元";
                //ShiFuJinE = TotalMoneny - GouWuJinString - CashString;
            }

        }
        if (DataMgr.m_account.hadProxy == 1)
        {
            ShiFuJinE = TotalMoneny - GouWuJinDouble - CashDouble;
            PayPriceText.text = ShiFuJinE.ToString("0.00") + "元";
            HasProxyText.gameObject.SetActive(false);
        }
        else if (DataMgr.m_account.hadProxy == 0)
        {
            ShiFuJinE = TotalMoneny;
            PayPriceText.text = ShiFuJinE.ToString("0.00") + "元";
            HasProxyText.gameObject.SetActive(true);
        }
    }
    void clickSwitchAddressBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.addresspanel, false, false, paragrm => { paragrm.GetComponent<addresspanel>().init(true); });
    }
    void clickSelectAliPayBtn(GameObject obj)
    {
        payType = 0;
        obj.transform.Find("Select").gameObject.SetActive(true);
        SelectWeiXinPayBtn.transform.Find("Select").gameObject.SetActive(false);
    }
    void clickSelectWeiXinPayBtn(GameObject obj)
    {
        payType = 1;
        obj.transform.Find("Select").gameObject.SetActive(true);
        SelectAliPayBtn.transform.Find("Select").gameObject.SetActive(false);
    }
}
