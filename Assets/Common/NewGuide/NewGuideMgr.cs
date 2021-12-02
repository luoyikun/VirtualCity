using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//新手引导UI箭头出现的方向
public enum EnGuideDir
{
    up = 0,
    down = 1,
    left = 2,
    right = 3  
}

//出现引导如何跳转下一步
public enum EnGuideClick
{
    NoClickCloseSelf = 0, //点击空白处关闭当前ui面板
    Click = 1, //点击要引导的按个按钮
    NoClickNoClose = 2, //点击空白处，只关闭引导mask，不关闭UI面板
    ClickNeedNext = 3, // 可以点击但是要通过点击 "下一步"按钮  驱动 ，针对输入框
}

[System.Serializable]
public class NewGuideItem
{
    public string panelName; //面板的名字
    public string imgPath; //目标的路径
    public string text;//提示的字
    public EnGuideClick isCanClick = EnGuideClick.Click; // 目标按钮可点击 1:可点  0：不可点，并关闭自己   2：不可点，不关闭自己
    public int isTextShowDir = -1; // 文本显示按钮的位置 -1 为下， 1为上   ,  2   
    public int belongCanvas = 0; // 属于哪个ui canvas下   0：screen  1：top
    public string bgPath; //  背景路径,新手引导的收缩至此，即这个区域是可点击区域，其他区域半透明黑色，屏蔽点击
    public int isAutoNext = 1; // 是否自动开始下步引导   0：不自动  1：自动
    public string param = ""; // 传入参数
}
public class NewGuideMgr : MonoSingleton<NewGuideMgr>
{
    public static int m_curIdx = -1;
    public static List<NewGuideItem> m_listGuide = new List<NewGuideItem>();
    public Transform m_canvasScreen;
    public Transform m_canvasTop;
    Transform m_lastTarget;
    private void Start()
    {
        
    }

    protected override void Init()
    {
        if (m_canvasScreen == null)
        {
            m_canvasScreen = UIManager.Instance.GetParent(UIManager.CanvasType.Screen);
        }

        if (m_canvasTop == null)
        {
            m_canvasTop = UIManager.Instance.GetParent(UIManager.CanvasType.Top);
        }
    }

    public static bool SetGuideIdx(int idx)
    {
        Debug.Log("服务器上一次引导步数:" + idx);
        if (idx >= m_listGuide.Count ||idx ==-1) //-1代表老用户不可参与新手引导
        {
            newguidepanel.Instance.Close();
            DataMgr.m_isNewGuide = false;
            return false;
        }
        DataMgr.m_isNewGuide = true;

        if (idx >= 1 && idx <= 18)
        {
            idx = 18;
        }
        else if (idx >= 20 && idx <= 27)
        {
            idx = 28;
        }
        else if (idx >= 28){
            idx = 28;
        }
     
        m_curIdx = idx;
        Debug.Log("开始引导步数:" + m_curIdx);
        if (m_curIdx >= 0)
        {
            return true;
        }
        
        return false;
    }

    public static void DataInit(string text)
    {
        //m_listGuide = JsonConvert.DeserializeObject<List<NewGuideItem>>(text);
        List<string[]> listData = CSVMgr.GetData(text);
        for (int i = 0; i < listData.Count; i++)
        {
            NewGuideItem item = new NewGuideItem();
            item.panelName = listData[i][0];
            item.imgPath = listData[i][1];
            item.text = listData[i][2];
            item.isCanClick = (EnGuideClick)(int.Parse(listData[i][3]));
            item.isTextShowDir = int.Parse(listData[i][4]);
            item.belongCanvas = int.Parse(listData[i][5]);
            item.bgPath = listData[i][6];
            item.isAutoNext = int.Parse(listData[i][7]);
            m_listGuide.Add(item);
        }
        
    }
    public void StartOneNewGuide(int delay = 0)
    {
        if (m_lastTarget != null)
        {
            if (m_lastTarget.GetComponent<DontDrag>() != null)
            {
                Destroy(m_lastTarget.GetComponent<DontDrag>());
            }
        }

        if (DataMgr.m_isNewGuide == false)
        {
            return;
        }

        //当前是最后一步，最后要发送一次
        if (m_curIdx >= m_listGuide.Count)
        {
            newguidepanel.Instance.Close();
            DataMgr.m_isNewGuide = false;
            //JsonMgr.SaveJsonString("OK", AppConst.LocalPath + "/NewGuide.txt");
            ReqUpdateOtherDataMessage req = new ReqUpdateOtherDataMessage();
            req.fieldName = "newStep";
            req.value = m_curIdx.ToString();
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateOtherDataMessage, req);
            return;
        }
        //if (delay == 0)
        //{
        //    DoNextNewGuide();
        //}
        //else {
        //    StartCoroutine(YieldDoNextNewGuide(delay));
        //}

        uiloadpanel.Instance.OpenByNewGuide();
        StartCoroutine(YieldDoNextNewGuide(delay));
    }

    /// <summary>
    /// 查找当前界面 是否 是当前的新手引导的第n步，如果找到了，执行引导遮罩
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator YieldDoNextNewGuide(int delay)
    {
        yield return delay;
        //DoNextNewGuide();

        NewGuideItem item = m_listGuide[m_curIdx];
        Transform transCanvas = null;
        if (item.belongCanvas == 0)
        {
            transCanvas = m_canvasScreen;
            Debug.Log("transCanvas:" + transCanvas.name);
        }
        else if (item.belongCanvas == 1)
        {
            transCanvas = m_canvasTop;
            Debug.Log("transCanvas:" + transCanvas.name);
        }
        Transform trans = null;

        while (trans == null)
        {
            //Debug.Log("接着找");
            yield return new WaitForSeconds(0.2f);
            try
            {
                
                trans = transCanvas.Find(item.panelName + "/" + item.imgPath);
                if (trans != null)
                {
                    Debug.Log("新手引导找到了：" + item.panelName + "/" + item.imgPath);
                }
            }
            catch
            {
                Debug.Log("新手引导找不到：" + item.panelName + "/" + item.imgPath);
            }
        }

        UGUIPanel panel = transCanvas.Find(item.panelName).GetComponent<UGUIPanel>();

        while (panel.m_isOpen == false)
        {
            yield return null;
        }
        trans.gameObject.AddComponent<DontDrag>(); //如果引导在滚动层上，加屏蔽滚动

        //目标本身可点，击且点击后能驱动到下一步引导，m_curIdx+1，并接着引导
        if (item.isCanClick == EnGuideClick.Click && item.isAutoNext == 1)
        {
            while (trans.gameObject.GetComponent<ClickListener>() == null)
            {
                yield return null;
            }
            trans.gameObject.GetComponent<ClickListener>().onNewGuideClick = (obj) =>
            {
                StartOneNewGuide(); 
                trans.gameObject.GetComponent<ClickListener>().onNewGuideClick = null;
            };
        }

        //目标本身可点击，点击后不能驱动下一步，新手引导暂停
        if (item.isCanClick == EnGuideClick.Click && item.isAutoNext == 0)
        {
            while (trans.gameObject.GetComponent<ClickListener>() == null)
            {
                yield return null;
            }
            trans.gameObject.GetComponent<ClickListener>().onNewGuideClick = (obj) =>
            {
                newguidepanel.Instance.PauseGuide();
                trans.gameObject.GetComponent<ClickListener>().onNewGuideClick = null;
            };
        }

        m_lastTarget = trans;
        Image img = trans.GetComponent<Image>();
        Transform transBg = transCanvas.Find(item.panelName + "/" + item.bgPath);
        Image imgBg = transBg.GetComponent<Image>();
        newguidepanel.Instance.Open(img,imgBg,item);
        
        ReqUpdateOtherDataMessage req = new ReqUpdateOtherDataMessage();
        req.fieldName = "newStep";
        req.value = m_curIdx.ToString();
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateOtherDataMessage, req);
        m_curIdx++;

    }
   
}
