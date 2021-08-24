using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsItem : MonoBehaviour {
    List<GoodsKind> Target_GoodsKindList=new List<GoodsKind>();
    
    // Use this for initialization
    public void Init(int childcount,int index,List<GoodsKind> m_GoodsKindList,shoppingcartpanel m_SPCT, Dictionary<long, ShoppingCartDic> m_Dic)
    {
        //SPCT = m_SPCT;
        //Target_GoodsKindList = m_GoodsKindList;
        transform.GetChild(childcount).name = m_GoodsKindList[index].id.ToString();//Name = m_Dic.Key
        transform.GetChild(childcount).GetComponent<itemGoods>().m_SPCT = m_SPCT;
       // transform.GetChild(childcount).Find("SelectBtn").GetChild(1).gameObject.SetActive(m_SPCT.ItemBoolList[index]);
        transform.GetChild(childcount).GetComponent<itemGoods>().GoodsImage.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_GoodsKindList[index].kindPicture);
        ClickListener.Get(transform.GetChild(childcount).GetComponent<itemGoods>().GoodsImage.gameObject).onClick = m_SPCT.clickGoodsImage;
        transform.GetChild(childcount).GetComponent<itemGoods>().GoodsName.GetComponent<Text>().text = m_GoodsKindList[index].name; 
        transform.GetChild(childcount).GetComponent<itemGoods>().GoodsPrice.GetComponent<Text>().text = m_GoodsKindList[index].value.ToString()+"元";
        ClickListener.Get(transform.GetChild(childcount).GetComponent<itemGoods>().reduBtn.gameObject).onClick = m_SPCT.clickNumberBtn;
        ClickListener.Get(transform.GetChild(childcount).GetComponent<itemGoods>().increaseBtn.gameObject).onClick = m_SPCT.clickNumberBtn;
        ClickListener.Get(transform.GetChild(childcount).GetComponent<itemGoods>().SelectBtn.gameObject).onClick = m_SPCT.clickSelectBtn;
        transform.GetChild(childcount).GetComponent<itemGoods>().SelectBtn.name = index.ToString();
        transform.GetChild(childcount).GetComponent<itemGoods>().IsSelect= (m_SPCT.ItemBoolList[index]);
        foreach (long Key in m_Dic.Keys)
        {
            if (long.Parse(transform.GetChild(childcount).name) == Key)
            {
                for (int i = 0; i < m_GoodsKindList.Count; i++)
                {
                    if (long.Parse(transform.GetChild(childcount).name) == m_GoodsKindList[i].id)
                    {
                        if (m_Dic[Key].number <= m_GoodsKindList[i].number)
                        {
                            transform.GetChild(childcount).Find("Numbers").Find("GoodsNumText").GetComponent<Text>().text = m_Dic[Key].number.ToString();
                        }
                        else if (m_Dic[Key].number > m_GoodsKindList[i].number)
                        {
                            m_Dic[Key].number = (int)m_GoodsKindList[i].number;
                            transform.GetChild(childcount).Find("Numbers").Find("GoodsNumText").GetComponent<Text>().text = m_GoodsKindList[i].number.ToString();
                        }
                    }
                }
            }
        }
        transform.GetChild(childcount).GetComponent<itemGoods>().Init();
    }
}
