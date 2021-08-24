using Framework.UI;
using ProtoDefine;
using SuperScrollView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class hthistorypanel :UGUIPanel
{

    public LoopListView2 ScrollView;

    public Transform Display_list;

    public Transform direction;

    private bool io;

    public Sprite[] img_data;

    public GameObject Text_obj;

    /// <summary>
    /// 注意设置表的长度
    /// </summary>
    private int shuliang;

    Dictionary<DateTime, List<Recode>> m_records = new Dictionary<DateTime, List<Recode>>();

    DateTime[] keys;

    private void Awake()
    {
        io = false;
        ScrollView.InitListView(1, OnGetItemByIndex);
    }


    // Use this for initialization
    void Start () {
    }


    public override void OnOpen()
    {
       
    }
        // Update is called once per frame
        void Update () {
		
	}

    public void Display_But()
    {
     
        if (!io)
        {
            io = true;
            direction.Rotate(Vector3.forward * 180);
            get_Dictionary_data();
            StartCoroutine(kai());
        }
        else
        {
            io = false;
            direction.Rotate(Vector3.back * 180);
            StartCoroutine(gaun());
        }
    }

    public void Display_()
    {
        if (io)
        {
            io = false;
            direction.Rotate(Vector3.forward * 180);
            StartCoroutine(gaun());
        }
    }


    IEnumerator kai()
    {
        while (Display_list.localPosition.x < 830)
        {
            // Display_list.Translate(Vector3.right * 60);
            Display_list.localPosition = new Vector3(Display_list.localPosition.x+60,0,0);
            yield return 0;//new WaitForSeconds(0.0001f);
        }
        Display_list.localPosition = new Vector3(830, Display_list.localPosition.y,Display_list.localPosition.z);
    }

    IEnumerator gaun()
    {
        while (Display_list.localPosition.x >2)
        {
          //  Display_list.Translate(Vector3.left*60);
            Display_list.localPosition = new Vector3(Display_list.localPosition.x - 60, 0, 0);
            yield return 0;//new WaitForSeconds(0.0001f);
        }
        Display_list.localPosition = new Vector3(2, Display_list.localPosition.y, Display_list.localPosition.z);
        m_records.Clear();
       // fz_records.Clear();
    }

    int idx;
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {

        if (index < 0 || index >= shuliang)
        {
            return null;
        }

        LoopListViewItem2 item = listView.NewListViewItem("But_0");

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        List<Recode> info= m_records[keys[index]];

        string key = keys[index].ToShortDateString().ToString();
        string[] bufTime = key.Split('/');
        key = string.Format(bufTime[0] + "-" + bufTime[1]);

        item.transform.GetChild(1).GetComponent<Text>().text = key;
        Top_setting(info.Count,item,info);
        Debug.Log(index);
        return item;
    }

    void Top_setting(int cont, LoopListViewItem2 item, List<Recode> info)
    {
        for (int i = item.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(item.transform.GetChild(i).gameObject);
        }
        item.GetComponent<RectTransform>().sizeDelta = new Vector2(800, (50 + 75 * cont));
        item.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(50, (75 * cont));
        item.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(2.5f, (75 * cont));
        int pos = -25;
        for (int i = 0; i < cont; i++)
        {
            GameObject In = Instantiate(Text_obj);
            In.transform.parent = item.transform;
            In.transform.localPosition = new Vector3(0, -(pos += 75), 0);
            if (info[i].handleType==0)
            {
                In.transform.GetChild(1).GetComponent<Text>().text = string.Format(info[i].name + " 来加速了" + ",时间减少了" + info[i].getReduceTime().ToString()+"秒");
                In.transform.GetChild(0).GetComponent<Image>().sprite= img_data[0];
            }
            else
            {
                In.transform.GetChild(1).GetComponent<Text>().text = string.Format(info[i].name + " 来搞了一下卫生" + ",顺便带走了" + info[i].number.ToString() + "金币");
                In.transform.GetChild(0).GetComponent<Image>().sprite = img_data[1];
            }
            if (Screen.width == 1440)
            {
                /*
                In.transform.GetChild(0).GetComponent<RectTransform>().localPosition =
                    new Vector3
                    (In.transform.GetChild(0).GetComponent<RectTransform>().localPosition.x - 15,
                    In.transform.GetChild(0).GetComponent<RectTransform>().localPosition.y,
                    In.transform.GetChild(0).GetComponent<RectTransform>().localPosition.z);
                    */
            }
            In.transform.localScale = new Vector3(1, 1, 0);
        }
    }


    void Senchen(int num)
    {
        ScrollView.SetListItemCount(num);
        ScrollView.RefreshAllShownItem();
    }




    void get_Dictionary_data()
    {
        Debug.Log("有"+buildhometown.m_instance.m_dicDevlopment.Count.ToString()+"个房子的数据");

        List<Recode> L_stoneRecod = new List<Recode>();

        //刷新界面
        foreach (var item in buildhometown.m_instance.m_dicDevlopment)
        {
           // Debug.Log("房子数据的Key"+item.Key.ToString() + item.Value.ToString());
            Dictionary<string, Recode> stoneRecod = buildhometown.m_instance.m_dicDevlopment[item.Key].getStoneRecod();
            Dictionary<string, Recode> helpRecod = buildhometown.m_instance.m_dicDevlopment[item.Key].getHelpRecod();
            if (stoneRecod != null)
            {
                Debug.Log("stoneRecod的长度:" + stoneRecod.Count.ToString());
                foreach (var item_0 in stoneRecod)
                {

                    if (stoneRecod[item_0.Key]!=null)
                    {
                        L_stoneRecod.Add(stoneRecod[item_0.Key]);
                    }
                }
            }
            else { Debug.Log("stoneRecod没有数据");  }

            if (helpRecod != null)
            {
                Debug.Log("helpRecod的长度:" + helpRecod.Count.ToString());
                foreach (var item_0 in helpRecod)
                {
                    if (helpRecod[item_0.Key] != null)
                    {
                        L_stoneRecod.Add(helpRecod[item_0.Key]);
                    }
                }
            }
            else{ Debug.Log("helpRecod为没有数据"); }
        }

        Recode_sort(L_stoneRecod);
    }


    void Recode_sort(List<Recode> L_stoneRecod)
    {
        Debug.Log("stoneRecod的长度" + L_stoneRecod.Count.ToString());
            for (int i = 0; i < L_stoneRecod.Count; i++)
            {
               Recode info = L_stoneRecod[i];
            /*
               string key = SyncTime.Stamp2DataTime(SyncTime.Server2Stamp(L_stoneRecod[i].getTime())).ToShortDateString().ToString();
               string[] bufTime = key.Split('/');
               key = string.Format(bufTime[0]+"-"+ bufTime[1]); 
             */
            Debug.Log(L_stoneRecod[i].getTime());
            string[] bufTime = L_stoneRecod[i].getTime().Split('T');
            string timemun = string.Format(bufTime[0]+ "T10: 00:00.000");
            DateTime key = SyncTime.Stamp2DataTime(SyncTime.Server2Stamp(timemun));
            if (m_records.ContainsKey(key) == false)
                {
                    m_records[key] = new List<Recode>();
                    m_records[key].Add(info);
                }
                else
                {
                    m_records[key].Add(info);
                }
            }
        //在这个地方把m_records按时间排序
        var dicSort = from objDic in m_records orderby objDic.Key descending select objDic;
        m_records = new Dictionary<DateTime, List<Recode>>();

        foreach (KeyValuePair<DateTime, List<Recode>> kvp in dicSort)
        {
            m_records.Add(kvp.Key, kvp.Value);
        }


        keys = new DateTime[m_records.Count];
        int j = 0;
        foreach (var item_0 in m_records)
        {
            keys[j]=item_0.Key;
            j++;
        }

        shuliang = m_records.Count;
        Senchen(shuliang);
    }
  


    public static DateTime ConvertLongDateTime(long d)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = d;
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart.Add(toNow);
        return dtResult;
    }
}

public class My_Recode
{
    public Recode stoneRecod;
    public Recode sthelpRecod;
    public string time;
}
