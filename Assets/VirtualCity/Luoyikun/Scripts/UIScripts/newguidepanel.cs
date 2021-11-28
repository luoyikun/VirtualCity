using Framework.Event;
using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class newguidepanel : MonoSingleton<newguidepanel>, ICanvasRaycastFilter
{
    public bool m_isOpen = false;
    Transform m_par;
    public Text m_text;
    public GameObject m_btnKnow;
    //public EnGuideClick m_isTarCanClick = EnGuideClick.Click;
    public Image m_imgRect;
    public GameObject m_btnSkip;
    public RectTransform m_tipPar;
    public RectTransform m_arrows; // 箭头
    NewGuideItem m_newItem;
    public bool m_isSetOk = false;
    //public GameObject m_imgDontClick;
    public void Open(Image imgTarget,Image imgBg, NewGuideItem item)
    {
        m_isSetOk = false;
        m_btnSkip.SetActive(true);
        m_newItem = item;
        
        if (m_par == null)
        {
            m_par = UIManager.Instance.GetParent(UIManager.CanvasType.Screen);
        }

        if (m_canvas == null)
        {
            m_canvas = UIManager.Instance.GetParent(UIManager.CanvasType.Screen).GetComponent<Canvas>();
        }

        if (_material == null)
        {
            _material = GetComponent<Image>().material;
        }

        m_isOpen = true;
        transform.SetParent(m_par, false);
        transform.localPosition = Vector3.zero;
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        m_target = imgTarget;
        m_bgTarget = imgBg;

        Vector2 textSize = m_text.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("textSizeBefore:" + textSize.ToString());
        Vector2 sizeImg = imgTarget.transform.GetComponent<RectTransform>().sizeDelta;
        m_text.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeImg.x, textSize.y);
        Color color = m_text.color;
        color.a = 0;
        m_text.color = color;
        m_text.text = item.text;
        //m_text.text = "";

        //m_text.DOText(item.text, 3);
        SetRect(imgTarget);

        if (item.text != "")
        {
            StartCoroutine(YieldCreateTip(color, item.isTextShowDir));
        }
        else {
            m_tipPar.gameObject.SetActive(false);
        }
        SetNewTargetImage();
        SetArrows(item.isTextShowDir);
        switch (item.isCanClick)
        {
            case EnGuideClick.NoClickNoClose:
            case EnGuideClick.NoClickCloseSelf:
            case EnGuideClick.ClickNeedNext:
                m_btnKnow.SetActive(true);
                break;
            case EnGuideClick.Click:
                
                m_btnKnow.SetActive(false);
                break;
                
            default:
                break;
        }
           
    }

    void SetArrows(int isTextShowDown = -1)
    {
        m_arrows.gameObject.SetActive(true);
        Vector3 imgPos = Vector3.zero;
        Vector2 sizeImg = m_arrows.sizeDelta;
        
        RectTransform rectTar = m_target.GetComponent<RectTransform>();
        if (rectTar.anchorMin == new Vector2(0, 1) && rectTar.anchorMax == new Vector2(0, 1) && rectTar.pivot == new Vector2(0, 1))
        {
            imgPos = rectTar.position;
            imgPos.x += rectTar.sizeDelta.x / 2 * PublicFunc.GetWidthFactor();
            imgPos.y -= rectTar.sizeDelta.y / 2 * PublicFunc.GetHeightFactor();
        }
        else
        {
            imgPos = rectTar.position;
        }
        imgPos.y = imgPos.y + isTextShowDown * sizeImg.y * 0.5f * PublicFunc.GetHeightFactor() + isTextShowDown * rectTar.sizeDelta.y * 0.5f * PublicFunc.GetHeightFactor();
        m_arrows.position = imgPos;
        m_arrows.GetComponent<NewArrowsCtrl>().SetDir(isTextShowDown * -1);
    }
    void SetRect(Image imgTarget)
    {
        m_imgRect.gameObject.SetActive(true);
        RectTransform rectTar = imgTarget.GetComponent<RectTransform>();
        Vector3 pos = Vector3.zero;
        if (rectTar.anchorMin == new Vector2(0, 1) && rectTar.anchorMax == new Vector2(0, 1) && rectTar.pivot == new Vector2(0, 1))
        {
            pos = rectTar.position;
            pos.x += rectTar.sizeDelta.x / 2 * PublicFunc.GetWidthFactor();
            pos.y -= rectTar.sizeDelta.y /2 * PublicFunc.GetHeightFactor();
        }
        else {
            pos = rectTar.position;
        }
        m_imgRect.GetComponent<RectTransform>().position = pos;
        m_imgRect.GetComponent<RectTransform>().sizeDelta = rectTar.sizeDelta * 1.05f;
    }

    IEnumerator YieldCreateTip(Color color, int isTextShowDown = -1)
    {
        yield return 1;
        m_tipPar.gameObject.SetActive(true);
        color.a = 1;
        m_text.color = color;
        Vector2 sizeImg = m_tipPar.sizeDelta + new Vector2(100,110);
        Vector3 imgPos = Vector3.zero;// = m_target.GetComponent<RectTransform>().position;
       
        RectTransform rectTar = m_target.GetComponent<RectTransform>();
        if (rectTar.anchorMin == new Vector2(0, 1) && rectTar.anchorMax == new Vector2(0, 1) && rectTar.pivot == new Vector2(0, 1))
        {
            imgPos = rectTar.position;
            imgPos.x += rectTar.sizeDelta.x / 2 * PublicFunc.GetWidthFactor();
            imgPos.y -= rectTar.sizeDelta.y / 2 * PublicFunc.GetHeightFactor();
        }
        else
        {
            imgPos = rectTar.position;
        }
        imgPos.y = imgPos.y + isTextShowDown * sizeImg.y * 0.5f * PublicFunc.GetHeightFactor() + m_text.GetComponent<RectTransform>().sizeDelta.y * isTextShowDown * 0.5f * PublicFunc.GetHeightFactor() + 80 * isTextShowDown * PublicFunc.GetHeightFactor();// + isTextShowDown * 20;

        //对超过屏幕的判断
        if (imgPos.x + sizeImg.x /2 * PublicFunc.GetWidthFactor() > Screen.width )
        {
            imgPos.x -= imgPos.x + sizeImg.x/2 * PublicFunc.GetWidthFactor() - Screen.width ;
        }

        if (imgPos.x - sizeImg.x/2 * PublicFunc.GetWidthFactor() < 0)
        {
            imgPos.x += sizeImg.x /2 * PublicFunc.GetWidthFactor() - imgPos.x;
        }

        m_tipPar.position = imgPos;
    }
    IEnumerator YieldCreateText(Color color,int isTextShowDown = -1)
    {
        yield return 1;
        color.a = 1;
        m_text.color = color;
        Vector2 sizeImg = m_target.transform.GetComponent<RectTransform>().sizeDelta;
        Vector3 imgPos = m_target.GetComponent<RectTransform>().position;
        imgPos.y = imgPos.y + isTextShowDown * sizeImg.y * 0.5f + m_text.GetComponent<RectTransform>().sizeDelta.y * isTextShowDown * 0.5f;// + isTextShowDown * 20;
        m_text.GetComponent<RectTransform>().position = imgPos;
    }
    public void Close()
    {
        //if (m_isOpen == true)
        {
            m_isOpen = false;
            gameObject.SetActive(false);
        }
    }


    //void OnEnable()
    //{
    //    EventManager.Instance.AddEventListener(Common.EventStr.CreateNewGuideByUi, OnEvCreateNewGuideByUi);
    //}

    //void OnDisable()
    //{
    //    EventManager.Instance.RemoveEventListener(Common.EventStr.CreateNewGuideByUi, OnEvCreateNewGuideByUi);
    //}

    //void OnEvCreateNewGuideByUi(EventData data)
    //{
    //    var exdata = data as EventDataEx<Image>;
    //    m_target = exdata.GetData();

    //    SetNewTargetImage();
    //}

    /// <summary>
    /// 高亮显示的目标
    /// </summary>
    public Image m_target;

    public Image m_bgTarget;
    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 镂空区域中心
    /// </summary>
    private Vector4 _center;

    /// <summary>
    /// 最终的偏移值X
    /// </summary>
    private float _targetOffsetX = 0f;

    /// <summary>
    /// 最终的偏移值Y
    /// </summary>
    private float _targetOffsetY = 0f;

    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material _material;

    /// <summary>
    /// 当前的偏移值X
    /// </summary>
    private float _currentOffsetX = 0f;

    /// <summary>
    /// 当前的偏移值Y
    /// </summary>
    private float _currentOffsetY = 0f;

    /// <summary>
    /// 动画收缩时间
    /// </summary>
    private float _shrinkTime = 0.2f;


    Canvas m_canvas;
    /// <summary>
    /// 世界坐标向画布坐标转换
    /// </summary>
    /// <param name="m_canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns>返回画布上的二维坐标</returns>
    private Vector2 WorldToCanvasPos(Canvas m_canvas, Vector3 world)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform,
            world, m_canvas.GetComponent<Camera>(), out position);
        return position;
    }


    void SetNewTargetImage()
    {
        //获取画布
        //Canvas m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //Canvas m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //获取高亮区域四个顶点的世界坐标
        
        m_bgTarget.rectTransform.GetWorldCorners(_corners);
        //计算高亮显示区域咋画布中的范围
        _targetOffsetX = Vector2.Distance(WorldToCanvasPos(m_canvas, _corners[0]), WorldToCanvasPos(m_canvas, _corners[3])) / 2f;
        _targetOffsetY = Vector2.Distance(WorldToCanvasPos(m_canvas, _corners[0]), WorldToCanvasPos(m_canvas, _corners[1])) / 2f;
        //计算高亮显示区域的中心
        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(m_canvas, centerWorld);
        //设置遮罩材料中中心变量
        Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);
        _material = GetComponent<Image>().material;
        _material.SetVector("_Center", centerMat); //传入要镂空矩形的中心点
        //计算当前偏移的初始值
        RectTransform canvasRectTransform = (m_canvas.transform as RectTransform);
        if (canvasRectTransform != null)
        {
            //获取画布区域的四个顶点
            canvasRectTransform.GetWorldCorners(_corners);
            //它从左下开始，到左上， 然后到右上，最后到右下-->左下角开始逆时针旋转
            //求偏移初始值
            for (int i = 0; i < _corners.Length; i++)
            {
                if (i % 2 == 0)
                    _currentOffsetX = Mathf.Max(Vector3.Distance(WorldToCanvasPos(m_canvas, _corners[i]), center), _currentOffsetX);
                else
                    _currentOffsetY = Mathf.Max(Vector3.Distance(WorldToCanvasPos(m_canvas, _corners[i]), center), _currentOffsetY);
            }
        }
        //设置遮罩材质中当前偏移的变量
        _material.SetFloat("_SliderX", _currentOffsetX);//设置离中心点最大的x距离
        _material.SetFloat("_SliderY", _currentOffsetY);//设置离中心点最大的y距离
        m_isSetOk = true;
    }
    private void Start()
    {
        //m_canvas = UIManager.Instance.GetParent(UIManager.CanvasType.Screen).GetComponent<Canvas>();
        //_material = GetComponent<Image>().material;
        //SetNewTargetImage();
        ClickListener.Get(m_btnKnow).onClick = OnBtnKnow;
        ClickListener.Get(m_btnSkip).onClick = OnBtnSkip;
    }

    void OnBtnSkip(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, true);
        ispanel.SetContent("提示", "是否跳过全部的新手引导");
        ispanel.m_ok = () =>
        {
            ReqUpdateOtherDataMessage req = new ReqUpdateOtherDataMessage();
            req.fieldName = "newStep";
            req.value = "999";
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateOtherDataMessage, req);
            newguidepanel.Instance.Close();
            DataMgr.m_isNewGuide = false;
        };

            
    }


    private float _shrinkVelocityX = 0f;
    private float _shrinkVelocityY = 0f;

    void OnBtnKnow(GameObject obj)
    {
        if (m_newItem.isCanClick == EnGuideClick.ClickNeedNext)
        {
            if (m_target.transform.GetComponent<InputField>())
            {
                if (m_target.transform.GetComponent<InputField>().text.Length  <= 0)
                {
                    if (m_target.transform.Find("Text").GetComponent<Text>() != null)
                    {
                        if (m_target.transform.Find("Text").GetComponent<Text>().text.Length <= 0)
                        {
                            Debug.Log("已经输入的文字:" + m_target.transform.GetComponent<InputField>().text);
                            Hint.LoadTips("请填入正确的信息");
                            return;
                        }
                    }
                    
                }
            }
        }
        if (m_newItem.isCanClick == EnGuideClick.NoClickCloseSelf)
        {
            UIManager.Instance.PopSelf();
        }
        if (m_newItem.param != "")
        {
            EventManager.Instance.DispatchEvent(Common.EventStr.NewGuideParam, new EventDataEx<NewGuideItem>(m_newItem));
        }
        NewGuideMgr.Instance.StartOneNewGuide();
    }
    private void Update()
    {
        if (m_isSetOk == false)
        {
            return;
        }
        if (m_isSetOk == true && PublicFunc.FloatEqual(_currentOffsetX, _targetOffsetX,0.09f) && PublicFunc.FloatEqual(_currentOffsetY, _targetOffsetY, 0.09f))
        {
            //Debug.Log("新手引导到了");
            //m_imgDontClick.SetActive(false);
            uiloadpanel.Instance.CloseByNewGuide();
            return;
        }

        //从当前偏移值到目标偏移值差值显示收缩动画
        float valueX = Mathf.SmoothDamp(_currentOffsetX, _targetOffsetX, ref _shrinkVelocityX, _shrinkTime);
        float valueY = Mathf.SmoothDamp(_currentOffsetY, _targetOffsetY, ref _shrinkVelocityY, _shrinkTime);
        if (!Mathf.Approximately(valueX, _currentOffsetX))
        {
            _currentOffsetX = valueX;
            _material.SetFloat("_SliderX", _currentOffsetX);
        }

        if (!Mathf.Approximately(valueY, _currentOffsetY))
        {
            _currentOffsetY = valueY;
            _material.SetFloat("_SliderY", _currentOffsetY);
        }
    }

    public void PauseGuide()
    {
        m_target = null;
        m_text.text = "";
        _material.SetFloat("_SliderX", 0);
        _material.SetFloat("_SliderY", 0);

        m_btnSkip.SetActive(false);
        m_btnKnow.SetActive(false);
        m_imgRect.gameObject.SetActive(false);
        m_tipPar.gameObject.SetActive(false);
        m_arrows.gameObject.SetActive(false);
        //SetNewTargetImage();
    }
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (m_target == null || IsTarNoClick (m_newItem) || m_isSetOk == false)
            return true;

        return !RectTransformUtility.RectangleContainsScreenPoint(m_target.rectTransform, sp, eventCamera);
    }

    bool IsTarNoClick(NewGuideItem item)
    {
        if (item.isCanClick == EnGuideClick.NoClickCloseSelf || item.isCanClick == EnGuideClick.NoClickNoClose)
        {
            return true;
        }
        return false;
    }
}
