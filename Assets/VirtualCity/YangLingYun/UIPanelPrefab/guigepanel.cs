using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using ProtoDefine;
using Newtonsoft.Json;
using SuperScrollView;

public class guigepanel : UGUIPanel {
    public GameObject BackBtn;
    //public GameObject QueDingBtn;
    public GameObject FenLeiTmp;
    public GameObject FenLeiPar;
    public GameObject JianShaoBtn;
    public GameObject ZengJiaBtn;
    public GameObject PayBtn;
    public GameObject AddBtn;
    public Text GoodName;
    public Text GoodPrice;
    public Text ShengYuNumberText;
    public RawImage GoodImage;
    public Text GoodsNum;
    public int GoodNumber;
    public int numbers;
    GoodsKind m_GoodsKind = new GoodsKind();
    public List<GoodsKind> goodsKindList=new List<GoodsKind>();
    List<string> m_IP = new List<string>();
    int Index = 0;
    // Use this for initialization
    void Start () {
       // Init();
    }
    public void Init(List<GoodsKind> m_goodsKindList)
    {
        if (FenLeiPar.transform.childCount != 0)
        {
            for (int i = FenLeiPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(FenLeiPar.transform.GetChild(i).gameObject);
            }
        }
        goodsKindList = m_goodsKindList;
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
       // ClickListener.Get(QueDingBtn).onClick = clickQueDingBtn;
        ClickListener.Get(JianShaoBtn).onClick = clickZengJianBtn;
        ClickListener.Get(ZengJiaBtn).onClick = clickZengJianBtn;
        ClickListener.Get(PayBtn).onClick = goodsdetailspanel.gdp.clickPlaceOrder;
        ClickListener.Get(AddBtn).onClick = goodsdetailspanel.gdp.clickAddShoppingCart;
        numbers = goodsdetailspanel.gdp.GoodsNumbers;
        GoodsNum.text = numbers.ToString();
        // GoodName.text = m_goodsKindList[0].name;
        // GoodPrice.text = m_goodsKindList[0].value.ToString()+"元";
        //// m_IP = JsonConvert.DeserializeObject<List<string>>(m_goods.infoPicture);
        // GoodImage.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_goodsKindList[0].kindPicture);
        //InitScrollView();
        for (int i = 0; i < m_goodsKindList.Count; i++)
        {
            GameObject obj = PublicFunc.CreateTmp(FenLeiTmp, FenLeiPar.transform);
            obj.name = m_goodsKindList[i].id.ToString();
            obj.transform.Find("Text").GetComponent<Text>().text = m_goodsKindList[i].name;
            if (m_goodsKindList[i].number == 0)
            {
                obj.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (m_goodsKindList[i].number != 0)
            {
                ClickListener.Get(obj).onClick = clickFenLeiTmp;
            }
        }
        for (int i = 0; i < FenLeiPar.transform.childCount; i++)
        {
            if (FenLeiPar.transform.GetChild(i).GetChild(1).gameObject.activeSelf == false)
            {
                clickFenLeiTmp(FenLeiPar.transform.GetChild(i).gameObject);
                break;
            }
        }
    }
    public void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
    }
    //public void clickQueDingBtn(GameObject obj)
    //{
    //    UIManager.Instance.PopSelf(true);
    //}
    public void clickFenLeiTmp(GameObject obj)
    {
        for (int i = 0; i < FenLeiPar.transform.childCount; i++)
        {
            FenLeiPar.transform.GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("FFFFFF");
            FenLeiPar.transform.GetChild(i).Find("Text").GetComponent<Text>().color = PublicFunc.StringToColor("0A7AE8");
        }
        obj.GetComponent<Image>().color = PublicFunc.StringToColor("47B320");
        obj.transform.Find("Text").GetComponent<Text>().color = PublicFunc.StringToColor("FFFFFF");
        for (int i = 0; i < goodsKindList.Count; i++)
        {
            if (obj.name == goodsKindList[i].id.ToString())
            {
                GoodName.text = goodsKindList[i].name;
                GoodPrice.text = goodsKindList[i].value.ToString()+"元";
                GoodImage.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + goodsKindList[i].kindPicture);
                m_GoodsKind = goodsKindList[i];
                ShengYuNumberText.text ="剩余"+ goodsKindList[i].number+"件商品";
                for (int j = 0; j < FenLeiPar.transform.childCount; j++)
                {
                    if (FenLeiPar.transform.GetChild(j).name == obj.name)
                    {
                        Index = j;
                    }
                }
            }
        }
        goodsdetailspanel.gdp.InitGuiGe(m_GoodsKind, numbers, Index);
    }
    public void clickZengJianBtn(GameObject obj)
    {
        if (obj.name == "JianShao")
        {
            if (numbers > 1)
            {
                numbers--;
            }
            else
            {
                Hint.LoadTips("不能再减少啦", Color.white);
            }
        }
        else if (obj.name == "ZengJia")
        {
            if (numbers < m_GoodsKind.number)
            {
                numbers++;
            }
            else
            {
                Hint.LoadTips("已经到商品最大数量了哦", Color.white);
            }
        }
        GoodsNum.text = numbers.ToString();
        goodsdetailspanel.gdp.InitGuiGe(m_GoodsKind, numbers, Index);
    }
}
