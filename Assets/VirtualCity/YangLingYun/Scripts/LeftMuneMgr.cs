using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftMuneMgr : MonoBehaviour
{
    public GameObject LeftMenuPar;
    public GameObject MainPar;
    GameObject MainTmp;
    public bool Rank = false;
    public bool IsDingDan = false;
    public bool IsBillFlow = false;
    public int state = -1;
    public static LeftMuneMgr LFM;
    public GameObject TargetGameObject;
    int lastidx = -2;
    private void Awake()
    {
    }

    void OnEnable()
    {
        LFM = this;
        LeftMenuPar = this.gameObject;
    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < LeftMenuPar.transform.childCount; i++)
        {
            ClickListener.Get(LeftMenuPar.transform.GetChild(i).gameObject).onClick = clickLeftMenuBtn;
            if (IsDingDan == false) 
            {
                LeftMenuPar.transform.GetChild(i).name = i.ToString();
            }
            // LeftMenuPar.transform.GetChild(i).name = i.ToString();
        }
        if (IsDingDan == false)
        {
            clickLeftMenuBtn(LeftMenuPar.transform.GetChild(0).gameObject);
        }
    }
    public void clickLeftMenuBtn(GameObject obj)
    {
        //if (lastidx != int.Parse(obj.name))
        //{
        TargetGameObject = obj;
        if (IsDingDan == true)
        {
            state = int.Parse(obj.name);
            if (state == 0)
            {
                state--;
            }
            dingdanpanel.ddp.clickLeftMenu(state);
        }
        else if (Rank == true)
        {
            rankpanel.rp.clickLefMenuBtn(obj);
        }
        else if (IsBillFlow == true)
        {
            state = int.Parse(obj.name);
            if (state == 0)
            {
                state--;
            }
            billflowpanel.BFP.SendReqQBM(state);
        }
        else if (IsDingDan == false)
        {
            if (MainTmp != null)
            {
                DestroyImmediate(MainTmp);
            }
            MainTmp = PublicFunc.CreateTmp(MainPar.transform.GetChild(int.Parse(obj.name)).gameObject, MainPar.transform);
        }
        for (int i = 0; i < LeftMenuPar.transform.childCount; i++)
        {
            LeftMenuPar.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            LeftMenuPar.transform.GetChild(i).GetChild(2).GetComponent<Text>().color = new Vector4(0.0313f, 0.4274f, 0.8156f, 1);
        }
        LeftMenuPar.transform.Find(obj.name).GetChild(1).gameObject.SetActive(true);
        LeftMenuPar.transform.Find(obj.name).GetChild(2).GetComponent<Text>().color = new Vector4(1, 1, 1, 1);
        lastidx = int.Parse(obj.name);
        //}
    }
}
