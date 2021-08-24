using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using DG.Tweening;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;

public class editaddresspanel : UGUIPanel
{
    public GameObject backBtn;
    public GameObject DownPar;
    public GameObject Fond;
    public GameObject MorenBtn;
    public GameObject InputPar;
    public Text NameText;
    public Text TelephoneText;
    public GameObject DiQuBtn;
    public Text DiZhiText;
    public Text YouZhengBianMaText;
    public Address Target_Address = new Address();
    string m_ListAddressString;
    bool IsMoRen = false;
    public bool IsEdit = false;
    Text[] Target_AddressTextArray;
    public GameObject AddressPar;
    public static editaddresspanel EAP;
    void Start()
    {
        EAP = this;
        ClickListener.Get(backBtn).onClick = clickBackBtn;
        ClickListener.Get(DownPar.transform.GetChild(1).gameObject).onClick = clickSaveBtn;
        ClickListener.Get(DownPar.transform.GetChild(0).GetChild(0).gameObject).onClick = clickSaveBtn;
        ClickListener.Get(DownPar.transform.GetChild(0).GetChild(1).gameObject).onClick = clickDeleteBtn;
        ClickListener.Get(MorenBtn).onClick = clickMorenBtn;
        ClickListener.Get(DiQuBtn).onClick = clickDiQuBtn;
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateInfo);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateInfo);
    }
    void OnEvNetUpdateInfo(byte[] buf)
    {
        RspUpdateUserInfoMessage RspUUIM = PBSerializer.NDeserialize<RspUpdateUserInfoMessage>(buf);
        if (RspUUIM.code != 0)
        {
            //Hint.LoadTips("修改收货地址成功", Color.white);
            DataMgr.m_account.addressInfo = m_ListAddressString;
            UIManager.Instance.PopSelf();
            Debug.Log("PopEdit!");

        }
        else if (RspUUIM.code == 0)
        {
            Hint.LoadTips(RspUUIM.tips, Color.white);
        }
    }
    public void Init(Address m_Address, bool m_IsEdit)
    {
        IsMoRen = false;
        Target_Address = m_Address;
        if (Target_Address.IsMoRen == true)
        {
            clickMorenBtn(MorenBtn);
        }
        else if (Target_Address.IsMoRen == false)
        {
            //IsMoren = false;
        }
        IsMoRen = Target_Address.IsMoRen;
        IsEdit = m_IsEdit;
        if (m_IsEdit == true)
        {
            Fond.transform.GetChild(0).gameObject.SetActive(true);
            Fond.transform.GetChild(1).gameObject.SetActive(false);
            DownPar.transform.GetChild(0).gameObject.SetActive(true);
            DownPar.transform.GetChild(1).gameObject.SetActive(false);
            NameText.transform.parent.GetComponent<InputField>().text = Target_Address.m_RA.Name;
            //NameText.text = "";
            TelephoneText.transform.parent.GetComponent<InputField>().text = Target_Address.m_RA.Mobile.ToString();
            //TelephoneText.text = "";
            InitAddress(m_Address.m_RA);
            //DiQuBtn.transform.parent.GetComponent<InputField>().text = Target_Address.m_RA;
            // DiQuText.text = "";
            DiZhiText.transform.parent.GetComponent<InputField>().text = Target_Address.m_RA.Address;
            // DiZhiText.text = "";
            YouZhengBianMaText.transform.parent.GetComponent<InputField>().text = Target_Address.YouZhengBianMa.ToString();
            // YouZhengBianMaText.text = "";
            //NameText.text = Target_Address.Name;
            //TelephoneText.text = Target_Address.Telephone.ToString();
            //DiQuText.text = Target_Address.DiQu;
            //DiZhiText.text = Target_Address.DiZhi;
            //YouZhengBianMaText.text=Target_Address.YouZhengBianMa
        }
        else if (m_IsEdit == false)
        {
            NameText.transform.parent.GetComponent<InputField>().text = "";
            TelephoneText.transform.parent.GetComponent<InputField>().text = "";
            //DiQuText.transform.parent.GetComponent<InputField>().text = "";
            DiZhiText.transform.parent.GetComponent<InputField>().text = "";
            YouZhengBianMaText.transform.parent.GetComponent<InputField>().text = "";
            Fond.transform.GetChild(0).gameObject.SetActive(false);
            Fond.transform.GetChild(1).gameObject.SetActive(true);
            DownPar.transform.GetChild(1).gameObject.SetActive(true);
            DownPar.transform.GetChild(0).gameObject.SetActive(false);
            AddressPar.transform.Find("ProvinceName").Find("Content").GetComponent<Text>().text = "";
            AddressPar.transform.Find("ExpAreaName").Find("Content").GetComponent<Text>().text = "";
            AddressPar.transform.Find("CityName").Find("Content").GetComponent<Text>().text = "";
        }
        SwtichMorenState(IsMoRen);
        //clickMorenBtn(MorenBtn);
    }
    public void InitAddress(RefundAddress m_RefundAddress)
    {
        Target_Address.m_RA = m_RefundAddress;
        AddressPar.transform.Find("ProvinceName").Find("Content").GetComponent<Text>().text = m_RefundAddress.ProvinceName;
        AddressPar.transform.Find("ExpAreaName").Find("Content").GetComponent<Text>().text = m_RefundAddress.ExpAreaName;
        AddressPar.transform.Find("CityName").Find("Content").GetComponent<Text>().text = m_RefundAddress.CityName;
    }
    void clickDiQuBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.regionpanel, false, false);
    }
    void clickMorenBtn(GameObject obj)
    {
        if (IsMoRen == true)
        {
            SwtichMorenState(false);
            IsMoRen = false;
        }
        else if (IsMoRen == false)
        {
            SwtichMorenState(true);
            IsMoRen = true;
        }
    }
    void SwtichMorenState(bool m_IsMoren)
    {
        if (m_IsMoren == false)
        {
            Tween move = DOTween.To(() => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D, r => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D = r, new Vector3(-135, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.y, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.z), 0.5f);
            // Tween move = DOTween.To(() => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D, r => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D = r, new Vector3(13, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.y, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.z), 0.5f);

        }
        else if (m_IsMoren == true)
        {
            Tween move = DOTween.To(() => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D, r => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D = r, new Vector3(13, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.y, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.z), 0.5f);
            // Tween move = DOTween.To(() => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D, r => MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D = r, new Vector3(-135, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.y, MorenBtn.transform.GetComponent<RectTransform>().anchoredPosition3D.z), 0.5f);
        }
    }
    void clickDeleteBtn(GameObject obj)
    {
        List<Address> m_ListAddress = new List<Address>();
        Address m_Address = new Address();
        m_ListAddress = JsonConvert.DeserializeObject<List<Address>>(DataMgr.m_account.addressInfo);
        for (int i = 0; i < m_ListAddress.Count; i++)
        {
            if (Target_Address.ID == m_ListAddress[i].ID)
            {
                m_ListAddress.RemoveAt(i);
                break;
            }
        }
        m_ListAddressString = JsonConvert.SerializeObject(m_ListAddress);
        ReqUpdateUserInfoMessage ReqUUIM = new ReqUpdateUserInfoMessage();
        //ReqUUIM.accountId = DataMgr.m_account.id;
        //ReqUUIM.info = new List<UserInfoMap>();
        UserInfoMap m_UserInfoMap = new UserInfoMap();
        m_UserInfoMap.infoType = 606;
        m_UserInfoMap.info = m_ListAddressString;
        ReqUUIM.accountId = DataMgr.m_account.id;
        ReqUUIM.info = new List<UserInfoMap>();
        ReqUUIM.info.Add(m_UserInfoMap);
        //ReqUUIM.info.Add(m_UserInfoMap);
        ///eqUUIM.
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateUserInfoMessage, ReqUUIM);
    }
    void clickSaveBtn(GameObject obj)
    {
        List<Address> m_ListAddress = new List<Address>();
        //Address m_Address = new Address();
        //m_Address.m_RA = new RefundAddress();
        if (DataMgr.m_account.addressInfo != "" && DataMgr.m_account.addressInfo != null)
        {
            m_ListAddress = JsonConvert.DeserializeObject<List<Address>>(DataMgr.m_account.addressInfo);
        }
        bool IsHasAddress = true;
        for (int i = 0; i < AddressPar.transform.childCount; i++)
        {
            if (AddressPar.transform.GetChild(i).Find("Content").GetComponent<Text>().text == "")
            {
                IsHasAddress = false;
                Hint.LoadTips("请确定输入信息都正确", Color.white);
                return;
            }
        }
        if (NameText.text != "" && TelephoneText.text != "" && IsHasAddress == true && DiZhiText.text != "" && YouZhengBianMaText.text != "")
        {
            if (IsMoRen == true)
            {
                for (int i = 0; i < m_ListAddress.Count; i++)
                {
                    m_ListAddress[i].IsMoRen = false;
                }
            }
            if (IsEdit == true)
            {
                for (int i = 0; i < m_ListAddress.Count; i++)
                {
                    if (m_ListAddress[i].ID == Target_Address.ID)
                    {
                        m_ListAddress[i].m_RA.Name = NameText.text;
                        m_ListAddress[i].m_RA.Mobile = long.Parse(TelephoneText.text);
                        m_ListAddress[i].m_RA.ProvinceName = AddressPar.transform.Find("ProvinceName").Find("Content").GetComponent<Text>().text;
                        m_ListAddress[i].m_RA.ExpAreaName = AddressPar.transform.Find("ExpAreaName").Find("Content").GetComponent<Text>().text;
                        m_ListAddress[i].m_RA.CityName = AddressPar.transform.Find("CityName").Find("Content").GetComponent<Text>().text;
                        m_ListAddress[i].m_RA.Address = DiZhiText.text;
                        m_ListAddress[i].YouZhengBianMa = int.Parse(YouZhengBianMaText.text);
                        m_ListAddress[i].IsMoRen = IsMoRen;
                        break;
                    }
                }
            }
            else if (IsEdit == false)
            {
                Address m_Address = new Address();
                m_Address.m_RA = new RefundAddress();
                m_Address.ID = m_ListAddress.Count + 1;
                m_Address.m_RA.Name = NameText.text;
                m_Address.m_RA.Mobile = long.Parse(TelephoneText.text);
                m_Address.m_RA.ProvinceName = AddressPar.transform.Find("ProvinceName").Find("Content").GetComponent<Text>().text;
                m_Address.m_RA.ExpAreaName = AddressPar.transform.Find("ExpAreaName").Find("Content").GetComponent<Text>().text;
                m_Address.m_RA.CityName = AddressPar.transform.Find("CityName").Find("Content").GetComponent<Text>().text;
                m_Address.m_RA.Address = DiZhiText.text;
                m_Address.YouZhengBianMa = int.Parse(YouZhengBianMaText.text);
                m_Address.IsMoRen = IsMoRen;
                m_ListAddress.Add(m_Address);
            }
            m_ListAddressString = JsonConvert.SerializeObject(m_ListAddress);
            ReqUpdateUserInfoMessage ReqUUIM = new ReqUpdateUserInfoMessage();
            ReqUUIM.accountId = DataMgr.m_account.id;
            ReqUUIM.info = new List<UserInfoMap>();
            UserInfoMap m_UserInfoMap = new UserInfoMap();
            m_UserInfoMap.infoType = 606;
            m_UserInfoMap.info = m_ListAddressString;
            ReqUUIM.info.Add(m_UserInfoMap);
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateUserInfoMessage, ReqUUIM);
        }
        else
        {
            Hint.LoadTips("请确定输入信息都正确", Color.white);
        }
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
