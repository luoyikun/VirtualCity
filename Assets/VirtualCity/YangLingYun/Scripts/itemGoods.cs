using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemGoods : MonoBehaviour {
    public GameObject SelectBtn;
    public RawImage GoodsImage;
    public Text GoodsName;
    public Text GoodsPrice;
    public GameObject reduBtn;
    public GameObject increaseBtn;
    public Text GoodsNumText;
    public GameObject SelectImage;
    public GameObject Mask;
    public Text GoodsNumbers;
    public bool IsSelect=false;
    public shoppingcartpanel m_SPCT;
    //bool IsZengJia = false;
    bool IsDuiYingShuZi=false;
    // Use this for initialization
    public void Init()
    {
        //if (IsDuiYingShuZi == false)
        //{
        //    foreach (long Key in m_SPCT.DataMgrAccountCart.Keys)
        //    {
        //        if (long.Parse(gameObject.name) == Key)
        //        {
        //            GoodsNumbers.text = m_SPCT.DataMgrAccountCart[Key].number.ToString();
        //            IsDuiYingShuZi = true;
        //        }
        //    }
        //}
        m_SPCT.TotalPrice = 0;
        for (int i = 0; i < m_SPCT.m_ListKind.Count; i++)
        {
            if (m_SPCT.m_ListKind[i].id == long.Parse(gameObject.name))
            {
                if (m_SPCT.m_ListKind[i].number == 0)
                {
                    Mask.SetActive(true);
                }
                else if (m_SPCT.m_ListKind[i].number > 0)
                {
                    Mask.SetActive(false);
                    //GoodsNumbers.text = m_SPCT.m_ListKind[i].numer.ToString();
                }
            }
        }
        if (IsSelect == false)
        {
            SelectImage.SetActive(false);
            //SelectImage.SetActive(false);
            if (m_SPCT.m_ListSelectGoodsId.Contains(long.Parse(gameObject.name)))
            {
                m_SPCT.m_ListSelectGoodsId.Remove(long.Parse(gameObject.name));
                double Price = double.Parse(GoodsPrice.text.Remove(GoodsPrice.text.Length - 1)) * int.Parse(GoodsNumbers.text);
                long? IndexID=null;
                foreach (long ID in m_SPCT.ItemPriceDic.Keys)
                {
                    if (long.Parse(SelectBtn.transform.parent.name) == ID)
                    {
                        IndexID = ID;
                    }
                }
                if (IndexID != null)
                {
                    m_SPCT.ItemPriceDic.Remove((long)IndexID);
                }
                foreach (long Key in m_SPCT.ItemPriceDic.Keys)
                {
                    m_SPCT.TotalPrice += m_SPCT.ItemPriceDic[Key];
                }
                //m_SPCT.TotalPrice -= Price;
                m_SPCT.TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + m_SPCT.TotalPrice.ToString() + "元";
            }
            //IsZengJia = false;
            //if (m_SPCT.m_ListSelectGoodsId.Contains(long.Parse(gameObject.name)))
            //{
            //    m_SPCT.m_ListSelectGoodsId.Remove(long.Parse(gameObject.name));
            //    int Price = int.Parse(GoodsPrice.text.Remove(GoodsPrice.text.Length - 1));
            //    m_SPCT.TotalPrice -= Price;
            //    m_SPCT.TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + m_SPCT.TotalPrice.ToString() + "元";
            //    SelectImage.SetActive(false);
            //}
            //int Price = int.Parse(GoodsPrice.text.Remove(GoodsPrice.text.Length - 1));
            //m_SPCT.TotalPrice -= Price;
            //m_SPCT.TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + m_SPCT.TotalPrice.ToString() + "元";
            //m_SPCT.m_ListSelectGoodsId.Remove(long.Parse(gameObject.name));
        }
        else if (IsSelect == true)
        {
            SelectImage.SetActive(true);
            if (!m_SPCT.m_ListSelectGoodsId.Contains(long.Parse(gameObject.name)))
            {
                m_SPCT.m_ListSelectGoodsId.Add(long.Parse(gameObject.name));
            }
            else if (m_SPCT.m_ListSelectGoodsId.Contains(long.Parse(gameObject.name)))
            {

            }
            double Price = double.Parse(GoodsPrice.text.Remove(GoodsPrice.text.Length - 1)) * int.Parse(GoodsNumbers.text);
            if (!m_SPCT.ItemPriceDic.ContainsKey(long.Parse(SelectBtn.transform.parent.name)))
            {
                m_SPCT.ItemPriceDic.Add(long.Parse(SelectBtn.transform.parent.name), Price);

            }
            else if (m_SPCT.ItemPriceDic.ContainsKey(long.Parse(SelectBtn.transform.parent.name)))
            {
                m_SPCT.ItemPriceDic[long.Parse(SelectBtn.transform.parent.name)] = Price;
            }
            foreach (long Key in m_SPCT.ItemPriceDic.Keys)
            {
                m_SPCT.TotalPrice += m_SPCT.ItemPriceDic[Key];
            }
            // m_SPCT.TotalPrice += Price;
            m_SPCT.TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + m_SPCT.TotalPrice.ToString() + "元";
            //if (IsZengJia == false)
            //{
            //   // m_SPCT.m_ListSelectGoodsId.add(long.Parse(gameObject.name));
            //    int Price = int.Parse(GoodsPrice.text.Remove(GoodsPrice.text.Length - 1));
            //    m_SPCT.TotalPrice += Price;
            //    m_SPCT.TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + m_SPCT.TotalPrice.ToString() + "元";
            //    m_SPCT.m_ListSelectGoodsId.Add(long.Parse(gameObject.name));
            //    IsZengJia = true;
            //}
            //if (m_SPCT.m_ListSelectGoodsId.Contains(long.Parse(gameObject.name)))
            //{

            //}
            //else
            //{
            //    int Price = int.Parse(GoodsPrice.text.Remove(GoodsPrice.text.Length - 1));
            //    m_SPCT.TotalPrice += Price;
            //    m_SPCT.TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + m_SPCT.TotalPrice.ToString() + "元";
            //    m_SPCT.m_ListSelectGoodsId.Add(long.Parse(gameObject.name));
            //}
        }
    }
}
