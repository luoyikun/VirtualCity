using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Address
{
    public int ID;
    public RefundAddress m_RA;
    public int YouZhengBianMa;
    public bool IsMoRen;
}
public class addresspanel : UGUIPanel
{
    public GameObject backBtn;
    public GameObject addressPar;
    public GameObject addressTmp;
    public GameObject addaddressTmp;
    //GameObject ClickAddress;
    public static addresspanel ap;
    public string UserName;
    public string Account;
    public string Address;
    public string youzhengbianma;
    //public bool IsMoren = false;
    //public bool IsEdit = true;
    public bool IsPay;
    public List<Address> m_AddressList = new List<Address>();
    // Use this for initialization
    void Start()
    {
        ap = this;
        ClickListener.Get(backBtn).onClick = clickBackBtn;
    }
    public override void OnOpen()
    {
        init(IsPay);
    }
    public override void OnClose()
    {

    }
    public void init(bool m_IsPay)
    {
        Debug.Log("InitAddress!");
        IsPay = m_IsPay;
        GameObject obj;
        if (addressPar.transform.childCount != 0)
        {
            for (int i = addressPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(addressPar.transform.GetChild(i).gameObject);
            }
        }
        if (DataMgr.m_account.addressInfo != "[]")
        {
            m_AddressList = JsonConvert.DeserializeObject<List<Address>>(DataMgr.m_account.addressInfo);
            for (int i = 0; i < m_AddressList.Count; i++)
            {
                obj = PublicFunc.CreateTmp(addressTmp, addressPar.transform);
                obj.name = m_AddressList[i].ID.ToString();
                obj.transform.Find("UserNameText").GetComponent<Text>().text = m_AddressList[i].m_RA.Name;
                obj.transform.Find("AccountText").GetComponent<Text>().text = m_AddressList[i].m_RA.Mobile.ToString();
                obj.transform.Find("AddressText").GetComponent<Text>().text = m_AddressList[i].m_RA.ProvinceName + "-" + m_AddressList[i].m_RA.CityName + "-" + m_AddressList[i].m_RA.ExpAreaName + "-" + m_AddressList[i].m_RA.Address;
                obj.transform.Find("Moren").gameObject.SetActive(m_AddressList[i].IsMoRen);
                if (m_AddressList[i].IsMoRen == true)
                {
                    obj.transform.SetSiblingIndex(0);
                }
                ClickListener.Get(obj.transform.Find("Edit").gameObject).onClick = clickEditBtn;
                if (IsPay == true)
                {
                    ClickListener.Get(obj).onClick = clickSelectAddress;
                }
            }
        }
        else if (DataMgr.m_account.addressInfo == "[]")
        {
            Hint.LoadTips("暂无地址", Color.white);
        }
        obj = PublicFunc.CreateTmp(addaddressTmp, addressPar.transform);
        ClickListener.Get(obj).onClick = clickAddBtn;
    }
    void clickSelectAddress(GameObject obj)
    {
        Address Target_Address = new Address();
        for (int i = 0; i < m_AddressList.Count; i++)
        {
            if (int.Parse(obj.name) == m_AddressList[i].ID)
            {
                Target_Address = m_AddressList[i];
            }
        }
        UIManager.Instance.PopSelf();
        querenxinxipanel.QRXXP.InitAddress(Target_Address);
        //UIManager.Instance.PushPanel(UIPanelName.querenxinxipanel, false, false, paragrm => { paragrm.GetComponent<querenxinxipanel>().InitAddress(Target_Address); });
    }
    void clickEditBtn(GameObject obj)
    {
        int index = 0;
        for (int i = 0; i < addressPar.transform.childCount; i++)
        {
            if (addressPar.transform.GetChild(i).name == obj.transform.parent.name)
            {
                index = i;
                break;
            }
        }
        UIManager.Instance.PushPanel(UIPanelName.editaddresspanel, false, true, paragrm => { paragrm.GetComponent<editaddresspanel>().Init(m_AddressList[index], true); });
    }
    void clickAddBtn(GameObject obj)
    {
        Address m_Address = new Address();
        m_Address.m_RA = new RefundAddress();
        //UIManager.Instance.PushPanel(UIPanelName.editaddresspanel, UIManager.CanvasType.Screen, false);
        UIManager.Instance.PushPanel(UIPanelName.editaddresspanel, false, true, paragrm => { paragrm.GetComponent<editaddresspanel>().Init(m_Address, false); });
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
}
