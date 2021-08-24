using Framework.UI;
using Newtonsoft.Json.Linq;
using SuperScrollView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class regionpanel : UGUIPanel
{
    public LoopListView2 ScrollView;
    public GameObject BackBtn;
    public GameObject BG;
    public GameObject QueRenBtn;
    static int shuliang = 0;
    //生成数量

    public GameObject[] button_arr;
    //省市区按钮

    public Text[] diqu;
    //省市区字体

    public Transform Select_pictures;
    public Transform Select_pictures_;
    //选择图片

    int province_idx;
    Rootobject province_arr;
    //省

    int city_idx;
    Datum city_arr;
    //市

    int area_idx;
    City area_arr;
    //区
    public static string[] data;
    static string Init_str;
    bool IsInitScroll = false;
    void Start()
    {
        ClickListener.Get(BackBtn).onClick = back;
        ClickListener.Get(BG).onClick = back;
        ClickListener.Get(QueRenBtn).onClick = clickQueRen;
        data = new string[3];
        Read_data();
        ScrollView.InitListView(1, OnGetItemByIndex);
        shuliang = province_arr.data.Length;
        Senchen(shuliang);

        for (int i = 0; i < 3; i++)
        {
            ClickListener.Get(button_arr[i]).onClick = but_Click;
        }
    }


    public void Init(string province, string city, string area)
    {
        for (int i = 0; i < province_arr.data.Length; i++)
        {
            if (province_arr.data[i].name== province)
            {
                city_arr = province_arr.data[i];
                province_idx = i;
            }
        }
        for (int j = 0; j < city_arr.city.Length; j++)
        {
            if (city_arr.city[j].name == city)
            {
                area_arr = city_arr.city[j];
                city_idx = j;
            }
        }
        for (int k = 0; k < area_arr.area.Length; k++)
        {
            if (area_arr.area[k]== area)
            {
                area_idx = k;
            }
        }
        diqu[0].text = province;
        diqu[1].text = city;
        diqu[2].text = area;
        but_Click(button_arr[2]);

    }
    void Update() {

    }
    public override void OnOpen()
    {

    }

    public override void OnClose()
    {

    }

    void Senchen(int num)
    {
        ScrollView.SetListItemCount(num);
        ScrollView.RefreshAllShownItem();
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= shuliang)
        {
            return null;
        }

        LoopListViewItem2 item = listView.NewListViewItem("shenchen");

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        switch (Select_pictures.GetChild(0).GetComponent<Text>().text)
        {
            case "省":
                if (index < province_arr.data.Length)
                {
                    item.transform.GetChild(0).GetComponent<Text>().text = province_arr.data[index].name;
                    item.transform.GetChild(1).GetComponent<Text>().text = index.ToString();
                    Shield(item.gameObject, province_idx);
                    city_idx = 0;
                    area_idx = 0;
                    diqu[1].text = ""; diqu[2].text = "";
                }
                break;
            case "市":
                if (index < city_arr.city.Length)
                {
                    item.transform.GetChild(0).GetComponent<Text>().text = city_arr.city[index].name;
                    item.transform.GetChild(1).GetComponent<Text>().text = index.ToString();
                    Shield(item.gameObject, city_idx);
                    area_idx = 0;
                    diqu[2].text = "";
                }
                break;
            case "区":
                if (index < area_arr.area.Length)
                {
                    item.transform.GetChild(0).GetComponent<Text>().text = area_arr.area[index];
                    item.transform.GetChild(1).GetComponent<Text>().text = index.ToString();
                    Shield(item.gameObject, area_idx);
                }
                break;
        }
        ClickListener.Get(item.transform.gameObject).onClick = Selection_Click;
     
        return item;
    }

    void Shield(GameObject obj, int idx)
    {

        if (int.Parse(obj.transform.GetChild(1).GetComponent<Text>().text) == idx)
        {
            Selection_Click(obj);
        }
        if (int.Parse(Select_pictures_.parent.GetComponent<Text>().text) != idx)
        {
            Select_pictures_.GetComponent<Image>().enabled = false;
        }
    }



    void but_Click(GameObject obj)
    {
        Select_pictures.GetChild(0).GetComponent<Text>().text = obj.transform.GetChild(0).GetComponent<Text>().text;
        Select_pictures.position = obj.transform.position;
        switch (obj.transform.GetChild(0).GetComponent<Text>().text)
        {
            case "省":
                shuliang = province_arr.data.Length;
                Senchen(shuliang);
                break;
            case "市":
                Debug.Log(province_idx);
                shuliang = province_arr.data[province_idx].city.Length;
                city_arr = province_arr.data[province_idx];
                Senchen(shuliang);
                break;
            case "区":
                Debug.Log(city_idx);
                shuliang = province_arr.data[province_idx].city[city_idx].area.Length;
                area_arr = province_arr.data[province_idx].city[city_idx];
                Senchen(shuliang);
                break;
        }
    }


    void Selection_Click(GameObject obj)
    {
        Select_pictures_.GetComponent<Image>().enabled = true;
        Select_pictures_.transform.parent = obj.transform.GetChild(1);
        Select_pictures_.transform.localPosition = Vector3.zero;
        switch (Select_pictures.GetChild(0).GetComponent<Text>().text)
        {
            case "省":
                province_idx = int.Parse(obj.transform.GetChild(1).GetComponent<Text>().text);
                diqu[0].text = obj.transform.GetChild(0).GetComponent<Text>().text;
                break;
            case "市":
                city_idx = int.Parse(obj.transform.GetChild(1).GetComponent<Text>().text);
                diqu[1].text = obj.transform.GetChild(0).GetComponent<Text>().text;
                break;
            case "区":
                area_idx = int.Parse(obj.transform.GetChild(1).GetComponent<Text>().text);
                diqu[2].text = obj.transform.GetChild(0).GetComponent<Text>().text;
                break;
        }
    }



    void Read_data()
    {
        province_arr = VcData.m_rootobject;
    }
    public void clickQueRen(GameObject obj)
    {
        for (int i = 0; i < diqu.Length; i++)
        {
            if (diqu[i].text == "")
            {
                Hint.LoadTips("请选择完整地址", Color.white);
                return;
            }
            data[i] = diqu[i].text;
        }
        UIManager.Instance.PopSelf();
        editaddresspanel.EAP.Target_Address.m_RA.ProvinceName = diqu[0].text;
        editaddresspanel.EAP.Target_Address.m_RA.CityName = diqu[1].text;
        editaddresspanel.EAP.Target_Address.m_RA.ExpAreaName = diqu[2].text;
        editaddresspanel.EAP.InitAddress(editaddresspanel.EAP.Target_Address.m_RA);
    }
    public void back(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }

}


public class Rootobject
{
    public Datum[] data { get; set; }
}

public class Datum
{
    public string name { get; set; }
    public City[] city { get; set; }
}

public class City
{
    public string name { get; set; }
    public string[] area { get; set; }
}