using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationMarker : MonoBehaviour {
    public GameObject m_btnClick;
    public EnLand m_landState = EnLand.None;
    public string m_id;
    // Use this for initialization
    void Start () {
        ClickListener.Get(m_btnClick).onClick = OnBtnClick;

    }

    public void SetInfo(string id, EnLand type)
    {
        m_id = id;
        m_landState = type;
        switch (m_landState)
        {
            case EnLand.None:
                transform.Find("Image/Text").GetComponent<Text>().text = "开垦";
                break;
            case EnLand.Space:
                transform.Find("Image/Text").GetComponent<Text>().text = "建造";
                break;
           
            default:
                break;
        }
    }
    void OnBtnClick(GameObject obj)
    {
        switch (m_landState)
        {
            case EnLand.None:
               if (UIManager.Instance.IsTopPanel(Vc.AbName.hthistorypanel) == false)
                    {
                    UIManager.Instance.PopSelf();
                }

                string needCost = "";
                //if (DataMgr.m_dicOpenAreaCast[m_id.ToString()].g != 0)
                //{
                //    needCost += DataMgr.m_dicOpenAreaCast[m_id.ToString()].g.ToString() + "金币 ";
                //}

                //if (DataMgr.m_dicOpenAreaCast[m_id.ToString()].d != 0)
                //{
                //    needCost += DataMgr.m_dicOpenAreaCast[m_id.ToString()].d.ToString() + "钻石";
                //}

                string areaNum = (buildhometown.m_instance.GetHaveOpenAreaNum() + 1).ToString();
                if (DataMgr.m_dicOpenAreaCast[areaNum].g != 0)
                {
                    needCost += DataMgr.m_dicOpenAreaCast[areaNum].g.ToString() + "金币 ";
                }

                if (DataMgr.m_dicOpenAreaCast[areaNum].d != 0)
                {
                    needCost += DataMgr.m_dicOpenAreaCast[areaNum].d.ToString() + "钻石";
                }

                UIManager.Instance.PushPanel(Vc.AbName.paypanel, false, true, (param) =>
                {
                    paypanel pay = param.GetComponent<paypanel>();
                    pay.SetContent("提示", "确认开垦此地块", DataMgr.m_dicOpenAreaCast[areaNum].g, DataMgr.m_dicOpenAreaCast[areaNum].d);
                    pay.m_GoldPay = () =>
                    {
                        ReqOpenAreaMessage open = new ReqOpenAreaMessage();
                        open.code = m_id;
                        open.costDiamond = 0;
                        GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqOpenAreaMessage, open);

                        if (DataMgr.m_isNewGuide == true)
                        {
                            hometwonbuildheadpanel.m_instance.CloseLocationMarker(DataMgr.m_newBoxId);
                            hometwonbuildheadpanel.m_instance.CreateLocationMarker(DataMgr.m_newBoxId, EnLand.Space);
                            //NewGuideMgr.Instance.StartOneNewGuide();
                        }
                    };
                    pay.m_DiamondPay = () =>
                    {
                        ReqOpenAreaMessage open = new ReqOpenAreaMessage();
                        open.code = m_id;
                        open.costDiamond = 1;
                        GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqOpenAreaMessage, open);
                    };
                }, true);
                break;
            case EnLand.Space:

                if (UIManager.Instance.IsTopPanel(UIPanelName.hometownborpanel) == true)
                {
                    UIManager.Instance.PopSelf();
                }
                if (UIManager.Instance.IsTopPanel(UIPanelName.homeselectbuildpanel) == false)
                {
                    UIManager.Instance.PushPanel(UIPanelName.homeselectbuildpanel);
                }

                if (DataMgr.m_isNewGuide == true)
                {
                    hometwonbuildheadpanel.m_instance.CloseLocationMarker(DataMgr.m_newBoxId);
                }

                break;
            case EnLand.Home:
                break;
            case EnLand.CommonBuild:
                break;
            default:
                break;
        }
    }
   
}
