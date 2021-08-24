using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbTestScene : MonoBehaviour {
    public Transform m_tarns;
    public static AbTestScene Instance;
    public GameObject m_btn;
    GameObject m_login;
	// Use this for initialization
	void Start () {
        m_tarns = this.transform;
        Instance = this;
        AssetMgr.Instance.Init(Init);
        ClickListener.Get(m_btn).onClick = ClickClose;
    }


    void ClickClose(GameObject obj)
    {
        //Destroy(m_login);
        //UIManager.Instance.PopSelf(true);
        //UIManager.Instance.PushPanel(UIPanelName.loginpanel);
        //AssetMgr.Instance.CreateObj(UIPanelName.loginpanel, UIPanelName.loginpanel, m_tarns, Vector3.zero, Vector3.zero, Vector3.one, OnFinish);

        //AssetMgr.Instance.CreateObjOne(Vc.AbName.syjmodel, Vc.AbName.syjmodel, null, Vector3.zero, Vector3.zero, Vector3.one, OnFinish);
        AssetMgr.Instance.CreateObjOne(Vc.AbName.syjmodel, Vc.AbName.syjmodel, null, Vector3.zero, Vector3.zero, Vector3.one, OnFinish);
    }
    void Init()
    {
        //AssetMgr.Instance.CreateObj(UIPanelName.loginpanel, UIPanelName.loginpanel,m_tarns, Vector3.zero, Vector3.zero, Vector3.one, OnFinish);

        //UIManager.Instance.PushPanel(UIPanelName.loginpanel);
    }

    void OnFinish(GameObject obj)
    {
        m_login = obj;
    }
   
}
