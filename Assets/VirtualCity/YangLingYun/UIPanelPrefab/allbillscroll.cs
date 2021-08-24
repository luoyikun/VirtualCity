using System;
using ProtoDefine;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;


    public class allbillscroll : MonoBehaviour
    {
    public int State;
    public List<RspQueryBillMessage> Old_RQBM = new List<RspQueryBillMessage>();
    public LoopListView2 ScrollView;
    bool IsScrollViewInit = false;
    int TotalCount = 0;
    public static allbillscroll ABS;
    public string m_LastDate=null;
    //public int Years;
    //public int Months;

        private DateTime billsTime;
    // Use this for initialization
    private void OnEnable()
    {
        ABS = this;
        ScrollView = transform.Find("ScrollView").GetComponent<LoopListView2>();
        m_LastDate = DateTime.Now.ToString("G");
        billsTime = DateTime.Now;
        billflowpanel.BFP.m_LastDate = m_LastDate;
        billflowpanel.BFP.SendReqQBM(State);
        Old_RQBM.Clear();
    }
    public void Init(RspQueryBillMessage m_RspQBM)
    {
        Old_RQBM.Add(m_RspQBM);
        if (m_RspQBM.bills != null)
        {
           billsTime = Convert.ToDateTime(m_RspQBM.bills[m_RspQBM.bills.Count-1].createtime);
           // Years=billsTime.Year
            //Years = int.Parse(m_RspQBM.bills[m_RspQBM.bills.Count - 1].createtime.Substring(0, 4));
            //Months = int.Parse(m_RspQBM.bills[m_RspQBM.bills.Count - 1].createtime.Substring(5, 2));
            ////Debug.Log(Years + "and" + Months);
            //if (Months.ToString().Length == 1)
            //{
            //    m_LastDate = Years + "0" + Months;
            //}
            //else if (Months.ToString().Length == 2)
            //{
            //    m_LastDate = Years.ToString() + Months.ToString();
            //}
            TotalCount = Old_RQBM.Count;
        }
        else if (m_RspQBM.bills == null)
        {
            TotalCount = 0;
        }
        //if(TotalCount)
        if (IsScrollViewInit == false)
        {
            ScrollView.InitListView(TotalCount + 1, OnGetItemByIndex);
            IsScrollViewInit = true;
        }
        else if (IsScrollViewInit == true)
        {
            ScrollView.SetListItemCount(TotalCount + 1);
            ScrollView.RefreshAllShownItem();
        }
        billflowpanel.BFP.m_LastDate= m_LastDate;
        //m_LastDate = billflowpanel.BFP.m_LastDate;
    }
    public void MonthsJianShao()
    {
        DateTime CharacterCreateTime =Convert.ToDateTime(DataMgr.m_account.createtime);
        
        billsTime=billsTime.AddMonths(-1);
        TimeSpan m_Span=billsTime-CharacterCreateTime;
        if (m_Span.Days<-30)
        {
            billsTime = billsTime.AddMonths(1);
            Hint.LoadTips("已没有更多账单");
            return;
        }
        
        m_LastDate = billsTime.Year +""+ billsTime.Month;
        billflowpanel.BFP.m_LastDate = m_LastDate;
        billflowpanel.BFP.SendReqQBM(State);
        // m_LastDate = billflowpanel.BFP.m_LastDate;
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= TotalCount+1)
        {
            return null;
        }
        LoopListViewItem2 item = null;
        if (index == TotalCount)
        {
            item = listView.NewListViewItem("LoadMoreBtn");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            ClickListener.Get(item.gameObject).onClick = clickLoadMoreBtn;
            return item;
        }
        item = listView.NewListViewItem("MonthBill");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        item.transform.GetComponent<MonthBillMgr>().Ini(Old_RQBM[index].bills.Count, Old_RQBM[index].bills);
        return item;
    }
    void clickLoadMoreBtn(GameObject obj)
    {
        MonthsJianShao();
    }
    }