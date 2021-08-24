using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefleshGoodsInfo : MonoBehaviour {
    // Use this for initialization
    List<Goods> ListGoods;
	void Start () {
	}
    public void Init(int childcount, int Index,List<Goods> m_ListGoods)
    {
        ListGoods = m_ListGoods;
        Transform m_Tran = transform.GetChild(childcount);
        //if (m_ListGoods[Index].moudleId != 0)
        //{
        //    m_Tran.Find("GoodImage").Find("3DImage").gameObject.SetActive(true);
        //}
        if (m_ListGoods[Index].hasMoudle == 1)
        {
            m_Tran.Find("GoodImage").Find("3DImage").gameObject.SetActive(true);
        }

        m_Tran.name = m_ListGoods[Index].id.ToString();
        m_Tran.Find("Price").GetComponent<Text>().text = m_ListGoods[Index].priceMin.ToString()+"元起";
        m_Tran.Find("GoodName").GetComponent<Text>().text = m_ListGoods[Index].name;
        //GoodsRawImage.transform.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + order.goodsKindUrl);
        m_Tran.Find("GoodImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_ListGoods[Index].coverPicture);
        m_Tran.Find("JianJie").Find("Text").GetComponent<Text>().text = m_ListGoods[Index].infoText;
        ClickListener.Get(m_Tran.Find("GoodImage").gameObject).onClick = clickGoodsImage;
    }
    public void clickGoodsImage(GameObject obj)
    {
        Goods m_Goods=new Goods(); 
        for (int i = 0; i < ListGoods.Count; i++)
        {
            if (obj.transform.parent.name == ListGoods[i].id.ToString())
            {
                m_Goods = ListGoods[i];
                UIManager.Instance.PushPanel(UIPanelName.goodsdetailspanel, false, false, paragrm => { paragrm.GetComponent<goodsdetailspanel>().Init(m_Goods); });
            }
        }
    }
}
