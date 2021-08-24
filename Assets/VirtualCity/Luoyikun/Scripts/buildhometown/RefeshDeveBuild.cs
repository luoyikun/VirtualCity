using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefeshDeveBuild : MonoBehaviour {
    public Devlopments m_deveInfo;
    public GameObject m_btnRefesh;
	// Use this for initialization
	void Start () {
        ClickListener.Get(m_btnRefesh).onClick = OnBtnRefesh;

    }

    public void SetInfo(Devlopments info)
    {
        m_deveInfo = info;
    }

    void OnBtnRefesh(GameObject obj)
    {
        DevlopmentProperties info = DataMgr.m_dicDevlopmentProperties[(long)m_deveInfo.modelId];
        UIManager.Instance.PushPanel(Vc.AbName.paypanel, false, true, (param) =>
        {
            paypanel pay = param.GetComponent<paypanel>();
            pay.SetContent("提示", "确认翻新此建筑", (int)(info.gold* DataMgr.m_reBuildCast), (int)(info.diamond*DataMgr.m_reBuildCast));
            pay.m_GoldPay = () =>
            {
                ReqRebuildMessage req = new ReqRebuildMessage();
                req.buildId = m_deveInfo.id;
                req.costDiamond = 0;
                PublicFunc.ToGameServer(MsgIdDefine.ReqRebuildMessage, req);
            };
            pay.m_DiamondPay = () =>
            {
                ReqRebuildMessage req = new ReqRebuildMessage();
                req.buildId = m_deveInfo.id;
                req.costDiamond = 1;
                PublicFunc.ToGameServer(MsgIdDefine.ReqRebuildMessage, req);
            };
        }, true);
       
    }
}
