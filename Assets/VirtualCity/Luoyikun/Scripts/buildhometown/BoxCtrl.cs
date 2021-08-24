using Framework.Event;
using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxCtrl : MonoBehaviour {
    public string m_id;
    public bool m_isHaveBuild = false;
    public GameObject m_kuai;
    Transform m_trans;
    public Land m_land;
    public Renderer m_renderer;
	// Use this for initialization
	void Start () {
        m_trans = this.transform;

    }

    private void OnEnable()
    {
        m_kuai.SetActive(false);
    }
    public void OnSelectChange(bool isSelect)
    {
        m_kuai.SetActive(isSelect);
    }

    public void OnSetCollider(bool isEnable)
    {
        if (m_trans == null)
        {
            m_trans = this.transform;
        }
        m_trans.GetComponent<Collider>().enabled = isEnable;
    }

    public void OnSetVisible(bool isVisible)
    {
        m_renderer.enabled = isVisible;
    }

    public void PopOtherUi()
    {
        

    }
    public void OnPick()
    {

        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            //Debug.Log("点击UI");
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("点击UI");
            return;
        }

        OnSelectChange(true);

        Debug.Log("点击了地块:" + m_id);
        buildhometown.m_selectBoxId = m_id;
        buildhometown.m_instance.SetBoxSelect(m_id);


        /*ExPlayGold play = new ExPlayGold();
        play.type = 0;
        play.source = buildhometown.m_dicBuildObj[buildhometown.m_instance.m_dicLand[m_id].buildId].transform.position;
        play.target = PublicFunc.PosOverlay2World(buildhometown.m_instance.m_cam, homepanel.m_instance.m_textGold.transform.position);
        play.count = 20;
        EventManager.Instance.DispatchEvent(Common.EventStr.PlayGetGoldEffect, new EventDataEx<ExPlayGold>(play));
        */

        //只有自己有建造模式，其他人只有展示模式
        if (buildhometown.m_instance.m_btMode == BtMode.Build)
        {
            switch (buildhometown.m_instance.m_dicLand[m_id].getBuildType())
            {
                case (int)EnLand.Space:

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
                        //NewGuideMgr.Instance.StartOneNewGuide();
                    }

                    break;
                case (int)EnLand.CommonBuild:
                    {
                        if (UIManager.Instance.IsTopPanel(Vc.AbName.hthistorypanel) == false)
                        {
                            UIManager.Instance.PopSelf();
                        }

                        // 压入新的ui
                        UIManager.Instance.PushPanel(UIPanelName.hometownborpanel, false, false, (obj) =>
                        {
                            hometownborpanel bor = obj.GetComponent<hometownborpanel>();
                            bor.SetCommonBuildInfo((long)buildhometown.m_instance.m_dicLand[m_id].Model, EnBoR.Remove);

                        });
                    }
                    break;
                case (int)EnLand.Home:
                    {
                        if (UIManager.Instance.IsTopPanel(Vc.AbName.hthistorypanel) == false)
                        {
                            UIManager.Instance.PopSelf();
                        }

                        // 压入新的ui
                        UIManager.Instance.PushPanel(UIPanelName.hometownborpanel, false, false, (obj) =>
                        {
                            hometownborpanel bor = obj.GetComponent<hometownborpanel>();
                            bor.SetHomeInfo((long)buildhometown.m_instance.m_dicLand[m_id].Model, EnBoR.Remove);

                        });
                    }
                    
                    break;
                case (int)EnLand.None:

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

                    UIManager.Instance.PushPanel(Vc.AbName.paypanel, false, true, (param) => {
                        paypanel pay = param.GetComponent<paypanel>();
                        pay.SetContent("提示", "确认开垦此地块", DataMgr.m_dicOpenAreaCast[areaNum].g, DataMgr.m_dicOpenAreaCast[areaNum].d);
                        pay.m_GoldPay = () => {
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
                        pay.m_DiamondPay = () => {
                            ReqOpenAreaMessage open = new ReqOpenAreaMessage();
                            open.code = m_id;
                            open.costDiamond = 1;
                            GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqOpenAreaMessage, open);
                        };
                    }, true);
                    break;
                default:
                    break;
            }
        }
        else if (buildhometown.m_instance.m_btMode == BtMode.Display)
        {
            EventManager.Instance.DispatchEvent(Common.EventStr.CloseCurHeadUi);
            //switch (DataMgr.m_dicLand[m_id].getBuildType())
            switch (buildhometown.m_instance.m_dicLand[m_id].getBuildType())
            {
                case (int)EnLand.CommonBuild:
                    EventManager.Instance.DispatchEvent(Common.EventStr.CreateBtCommonHeadUi, new EventDataEx<string>(buildhometown.m_instance.m_dicLand[m_id].buildId));
                    break;
                case (int)EnLand.Home:
                    EventManager.Instance.DispatchEvent(Common.EventStr.CreateBtHomeHeadUi, new EventDataEx<string>(buildhometown.m_instance.m_dicLand[m_id].buildId));
                    break;
            }
        }
       
    }
}
