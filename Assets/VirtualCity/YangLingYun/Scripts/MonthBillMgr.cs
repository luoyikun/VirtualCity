using System;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthBillMgr : MonoBehaviour
{
    public GameObject Bills;
    public GameObject Bill;
    public Text TimeText;
    public Text TotalText;
    // Use this for initialization
    void Start()
    {

    }
    public void Ini(int Count, List<AccountBill> m_AccountBill)
    {
        if (Bills.transform.childCount != 0)
        {
            for (int i = Bills.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(Bills.transform.GetChild(i).gameObject);
            }
        }
        transform.Find("DownLine").gameObject.SetActive(true);
        TimeText.transform.parent.gameObject.SetActive(true);
        TimeText.text = m_AccountBill[0].createtime.Remove(10);
        double? ZhiChu = 0;
        double? ShouRu = 0;
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = PublicFunc.CreateTmp(Bill, Bills.transform);
            DateTime MyTime = Convert.ToDateTime(m_AccountBill[i].createtime);

            //obj.transform.Find("TimeText").GetComponent<Text>().text = m_AccountBill[i].createtime.Substring(5, 5);
            obj.transform.Find("TimeText").GetComponent<Text>().text = MyTime.Month + "-" + MyTime.Day;
            double Money = 0;
            double SMoney = 0;
            if (m_AccountBill[i].money != 0)//如果i账单的支出现金不等于0
            {
                Money = Money + m_AccountBill[i].money;
                if (m_AccountBill[i].sMoney != 0)
                {
                    SMoney = SMoney + m_AccountBill[i].sMoney;
                }
                else if (m_AccountBill[i].sMoney == 0)
                {

                }
            }
            else if (m_AccountBill[i].money == 0)
            {
                if (m_AccountBill[i].sMoney != 0)
                {
                    SMoney = SMoney + m_AccountBill[i].sMoney;
                }
                else if (m_AccountBill[i].sMoney == 0)
                {

                }
            }
            switch (m_AccountBill[i].budgetType)
            {
                case "0":
                    obj.transform.Find("MoneyText").GetComponent<Text>().text = "+" + (m_AccountBill[i].sMoney + m_AccountBill[i].money+m_AccountBill[i].oMoney);
                    ShouRu += m_AccountBill[i].sMoney + m_AccountBill[i].money;
                    break;
                case "1":
                    obj.transform.Find("MoneyText").GetComponent<Text>().text = "-" + (m_AccountBill[i].sMoney + m_AccountBill[i].money + m_AccountBill[i].oMoney);
                    ZhiChu += m_AccountBill[i].sMoney + m_AccountBill[i].money;
                    break;
                case "2":
                    obj.transform.Find("MoneyText").GetComponent<Text>().text =m_AccountBill[i].oMoney.ToString();
                    break;
            }

            string HasMoneyString = "现金" + Money + "元,";
            string HasSmoneyString= "购物金" + SMoney + "元,";
            string HasOmoneyString = "游戏外支付" + m_AccountBill[i].oMoney + "元";

            string MoneyString = "其中"+HasMoneyString+HasSmoneyString+HasOmoneyString;

            if (Money == 0)
            {
                MoneyString=MoneyString.Replace(HasMoneyString, "");
            }

            if (SMoney == 0)
            {
                MoneyString=MoneyString.Replace(HasSmoneyString, "");
            }

            if (m_AccountBill[i].oMoney == 0)
            {
                MoneyString=MoneyString.Replace(HasOmoneyString, "");
            }

            string EndString = MoneyString.Substring(MoneyString.Length-1, 1);

            if (EndString == ",")
            {
                MoneyString = MoneyString.Substring(0, MoneyString.Length - 2);
            }
            if (MoneyString.Length == 2)
            {
                MoneyString=MoneyString.Substring(0);
            }

            obj.transform.Find("XiaoFeiText").GetComponent<Text>().text = MoneyString;

            switch (m_AccountBill[i].billType)
            {
                case "0":
                    obj.transform.Find("InfoText").GetComponent<Text>().text = "购买VIP";
                    break;
                case "1":
                    obj.transform.Find("InfoText").GetComponent<Text>().text = "购买商品";
                    break;
                case "2":
                    obj.transform.Find("InfoText").GetComponent<Text>().text = "系统奖励";
                    break;
                case "3":
                    obj.transform.Find("InfoText").GetComponent<Text>().text = "获取奖励";
                    break;
            }
        }
        TotalText.text = "支出" + ZhiChu + "元,收入" + ShouRu + "元";

        double TranHeight = 0;
        for (int i = 0; i < Bills.transform.childCount; i++)
        {
            TranHeight += Bills.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }
        Bills.transform.GetComponent<RectTransform>().sizeDelta=new Vector2(Bills.transform.GetComponent<RectTransform>().sizeDelta.x,(float)TranHeight);

        TranHeight = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            TranHeight += transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }
        transform.GetComponent<RectTransform>().sizeDelta=new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x,(float)TranHeight);
    }
}
