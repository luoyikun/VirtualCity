using System.Collections;
using System.Collections.Generic;
using Framework.Event;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using UnityEngine;
using UnityEngine.UI;

public class searchGoodItem : MonoBehaviour
{
    public Goods TargetGood;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clickGoToBtn(GameObject obj)
    {
        long shopId = 0;
        Debug.Log("Goto");
        foreach (var item in DataMgr.m_dicShopsProperties)
        {
            shopId++;
            if (item.Value.businessId == TargetGood.businessId)
            {
                ShopsProperties shop = DataMgr.m_dicShopsProperties[shopId];
                Vector3 pos = new Vector3(shop.x, shop.y, shop.z);
                EventManager.Instance.DispatchEvent(Common.EventStr.PlayerNavGotoPoint, new EventDataEx<Vector3>(pos));
                UIManager.Instance.PopSelf();
                return;
            }
        }
        
        
    }

    public void Init(Goods m_good)
    {
        TargetGood = m_good;
        gameObject.name = m_good.id.ToString();
        transform.Find("GoodImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_good.coverPicture);
        transform.Find("GoodName").GetComponent<Text>().text = m_good.name;
        transform.Find("Price").GetComponent<Text>().text = m_good.priceMin + "元<size=30>起</size>";
        ClickListener.Get(transform.Find("GoToBtn").gameObject).onClick = clickGoToBtn;
    }
}
