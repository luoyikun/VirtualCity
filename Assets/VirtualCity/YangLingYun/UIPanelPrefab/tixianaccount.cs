using System.Collections;
using System.Collections.Generic;
using ProtoDefine;
using UnityEngine;
using UnityEngine.UI;

public class tixianaccount : MonoBehaviour
{
    public GameObject ZFBAccountPar;
    public GameObject WXAccountPar;
    public GameObject AccountTmp;
    public GameObject AddAccountTmp;
    // Use this for initialization
    void Start()
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    GameObject obj = PublicFunc.CreateTmp(AccountTmp, ZFBAccountPar.transform);
        //    ClickListener.Get(obj).onClick = clickAccountBtn;
        //    ClickListener.Get(obj.transform.GetChild(2).gameObject).onClick = clickAccountEditBtn;
        //    if (i == 9)
        //    {
        //        obj = PublicFunc.CreateTmp(AddAccountTmp, ZFBAccountPar.transform);
        //        ClickListener.Get(obj).onClick = clickAddAcountBtn;
        //    }
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject obj = PublicFunc.CreateTmp(AccountTmp, WXAccountPar.transform);
        //    ClickListener.Get(obj).onClick = clickAccountBtn;
        //    ClickListener.Get(obj.transform.GetChild(2).gameObject).onClick = clickAccountEditBtn;
        //    if (i == 4)
        //    {
        //        obj = PublicFunc.CreateTmp(AddAccountTmp, WXAccountPar.transform);
        //        ClickListener.Get(obj).onClick = clickAddAcountBtn;
        //    }
        //}
    }
    private void OnEnable()
    {
        Init(tixianpanel.tp.Target_ListPayAccount);
    }
    void Init(List<PayAccount> m_ListPayAccount)
    {
        if (ZFBAccountPar.transform.childCount != 0)
        {
            for (int i = ZFBAccountPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(ZFBAccountPar.transform.GetChild(i).gameObject);
            }
        }
        if (WXAccountPar.transform.childCount != 0)
        {
            for (int i = ZFBAccountPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(WXAccountPar.transform.GetChild(i).gameObject);
            }
        }
        //if(m_ListPayAccount!=null&&m_ListPayAccount)
        if (m_ListPayAccount == null)
        {
            GameObject obj;
            obj = PublicFunc.CreateTmp(AddAccountTmp, WXAccountPar.transform);
            ClickListener.Get(obj).onClick = clickAddAcountBtn;
            //obj = PublicFunc.CreateTmp(AddAccountTmp, ZFBAccountPar.transform);
            //ClickListener.Get(obj).onClick = clickAddAcountBtn;
            return;
        }
        for (int i = 0; i <= m_ListPayAccount.Count; i++)
        {
            GameObject obj;
            if (i == m_ListPayAccount.Count)
            {
                if (i != 0)
                {
                    break;
                }
                obj = PublicFunc.CreateTmp(AddAccountTmp, ZFBAccountPar.transform);
                obj.name = "AddCountBtn";
                ClickListener.Get(obj).onClick = clickAddAcountBtn;
                break;
            }
            if (m_ListPayAccount[i].payType == 0)
            {
                obj = PublicFunc.CreateTmp(AccountTmp, ZFBAccountPar.transform);
            }
            else
            {
                obj = PublicFunc.CreateTmp(AccountTmp, WXAccountPar.transform);
            }
            obj.transform.Find("AccountCountText").GetComponent<Text>().text = "账号" + (i + 1);
            obj.transform.Find("NameText").GetComponent<Text>().text = m_ListPayAccount[i].realName;
            obj.transform.Find("TelephoneText").GetComponent<Text>().text = m_ListPayAccount[i].account;
            ClickListener.Get(obj).onClick = clickAccountBtn;
            ClickListener.Get(obj.transform.Find("BianJiIcon").gameObject).onClick = clickAccountEditBtn;
        }
    }
    void clickAccountBtn(GameObject obj)
    {
        tixianpanel.tp.clickAccountBtn(obj);
    }
    void clickAccountEditBtn(GameObject obj)
    {
        tixianpanel.tp.clickAccountEditBtn(obj);
    }
    void clickAddAcountBtn(GameObject obj)
    {
        tixianpanel.tp.clickAddAcountBtn(obj);
    }
}
