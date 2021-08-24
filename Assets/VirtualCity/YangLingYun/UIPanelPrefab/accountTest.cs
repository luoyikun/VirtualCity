using System.Collections;
using System.Collections.Generic;
using ProtoDefine;
using UnityEngine;
using UnityEngine.UI;

public class accountTest : MonoBehaviour
{
    public GameObject ZFBAccountPar;
    public GameObject WXAccountPar;
    public GameObject AccountTmp;
    public GameObject AddAccountTmp;
    public static accountTest AT;
    // Use this for initialization
    void Start()
    {

    }
    private void OnEnable()
    {
        InitAccountList(accountsecuritypanel.asp.m_LPA);
        AT = this;
    }
    private void OnDisable()
    {

    }
    public void InitAccountList(List<PayAccount> m_ListPayAccount)
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
            for (int i = WXAccountPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(WXAccountPar.transform.GetChild(i).gameObject);
            }
        }

        //if (ZFBAccountPar.transform.Find("AddAcountBtn") == null)
        //{
        //    GameObject Obj = PublicFunc.CreateTmp(AddAccountTmp, ZFBAccountPar.transform);
        //    Obj.name = "AddAccountBtn";
        //    ClickListener.Get(Obj).onClick = clickAddAcountBtn;
        //    return;
        //}
        if (m_ListPayAccount != null)
        {
            for (int i = 0; i <= m_ListPayAccount.Count; i++)
            {
                if (i == m_ListPayAccount.Count)
                {
                    if (i != 0)
                    {
                        break;
                    }

                    GameObject Obj = PublicFunc.CreateTmp(AddAccountTmp, ZFBAccountPar.transform);
                    ClickListener.Get(Obj).onClick = clickAddAcountBtn;
                    break;
                }
                GameObject obj = PublicFunc.CreateTmp(AccountTmp, ZFBAccountPar.transform);
                if (m_ListPayAccount[i].payType == 0)
                {
                    obj.transform.parent = ZFBAccountPar.transform;
                }
                else
                {
                    obj.transform.parent = WXAccountPar.transform;
                }
                obj.name = i.ToString();
                obj.transform.Find("AccountCountText").GetComponent<Text>().text = "账号" + (i + 1);
                obj.transform.Find("NameText").GetComponent<Text>().text = m_ListPayAccount[i].realName;
                obj.transform.Find("TelephoneText").GetComponent<Text>().text = m_ListPayAccount[i].account;
                //ClickListener.Get(obj).onClick = clickAccountBtn;
                //if (i == m_ListPayAccount.alipay.Count)
                //{
                //    GameObject Obj = PublicFunc.CreateTmp(AddAccountTmp, ZFBAccountPar.transform);
                //    ClickListener.Get(Obj).onClick = clickAddAcountBtn;
                //}
            }
        }
    }
    void clickAccountBtn(GameObject obj)
    {
        accountsecuritypanel.asp.clickAccountBtn(obj);
    }
    void clickAddAcountBtn(GameObject obj)
    {
        accountsecuritypanel.asp.clickAddAcountBtn(obj);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
