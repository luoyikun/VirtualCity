using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chatLeftBtn : MonoBehaviour
{
    public GameObject LeftMenuPar;
    public static chatLeftBtn clb;
    public GameObject clickObjN;
    private void Awake()
    {
        clb = this;
        //Init();
    }
    // Use this for initialization
    void Start()
    {
    }
    public void Init()
    {
        LeftMenuPar = this.transform.GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < LeftMenuPar.transform.childCount; i++)
        {
            ClickListener.Get(LeftMenuPar.transform.GetChild(i).gameObject).onClick = clickLeftMenuBtn;
        }
    }
    public void clickLeftMenuBtn(GameObject obj)
    {
        //chatpanel.clickObjName = obj.name;
        //if (obj.GetComponent<ChatType>() != null)
        //{
        //    if (obj.GetComponent<ChatType>().m_tag == ChatType.TypeTag.SearchObj)
        //    {
        //        chatpanel.cp.clickChatLeftBtn(obj);
        //    }
        //}
        GameObject MainPar = chatpanel.cp.MainPar;
        if (clickObjN != obj)
        {
            for (int i = 0; i < LeftMenuPar.transform.childCount; i++)
            {
                LeftMenuPar.transform.GetChild(i).Find("GreenBG").gameObject.SetActive(false);
                LeftMenuPar.transform.GetChild(i).Find("Name").GetComponent<Text>().color = new Vector4(0.0313f, 0.4274f, 0.8156f, 1);
            }
            for (int i = 0; i < MainPar.transform.childCount; i++)
            {
                MainPar.transform.GetChild(i).gameObject.SetActive(false);
            }

            obj.transform.Find("GreenBG").gameObject.SetActive(true);

            obj.transform.Find("Name").GetComponent<Text>().color = new Vector4(1, 1, 1, 1);

            obj.transform.Find("RedPoint").gameObject.SetActive(false);

            chatpanel.cp.clickChatLeftBtn(obj);
            clickObjN = obj;
            //chatpanel.cp.ReqGetSociality();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
