using Framework.Pattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.UI
{
	 public class UIManager :Singleton<UIManager>
    {
        CanvasConfig m_canvasWorld;
        CanvasConfig m_canvasScreen;
        CanvasConfig m_canvasTop;
        public override void Init()
        {
            m_canvasWorld.CanvasPath = "UIManager/CanvasWorld";
            m_canvasScreen.CanvasPath = "UIManager/CanvasScreen";
            m_canvasTop.CanvasPath = "UIManager/CanvasTop";
        }

        public struct CanvasConfig
        {
            public string CanvasPath;
            private Transform canvasTransform;
            public Transform CanvasTransform
            {
                get
                {
                    if (canvasTransform == null)
                    {
                        canvasTransform = GameObject.Find(CanvasPath).transform;
                    }
                    return canvasTransform;
                }
            }
        }

        public enum CanvasType
        {
            World,
            Screen,
            Top
        }

        public Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();//借助BasePanel脚本保存所有实例化出来的面板物体（因为BasePanel脚本被所有面板预设物体的自己的脚本所继承，所以需要的时候可以根据BasePanel脚本来实例化每一个面板对象）
        private Stack<BasePanel> panelStack = new Stack<BasePanel>();//这是一个栈，用来保存实例化出来（显示出来）的面板
        private Dictionary<string, BasePanel> m_dicNotInStack = new Dictionary<string, BasePanel>();


        public void PushPanelDeleteSelf(string panelType, bool isAni = false, UnityAction<GameObject> onFinish = null,CanvasType uiStayCanvas = CanvasType.Screen)
        {
            uiloadpanel.Instance.Open();
            if (panelStack == null)//如果栈不存在，就实例化一个空栈
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Pop();//取出栈顶元素保存起来，但是不移除
                if (topPanel != null)
                {
                    topPanel.OnExit();//使该页面暂停，不可交互
                }
            }

            Transform par = null;
            if (uiStayCanvas == CanvasType.World)
            {
                par = m_canvasWorld.CanvasTransform;
            }
            else if (uiStayCanvas == CanvasType.Screen)
            {
                par = m_canvasScreen.CanvasTransform;
            }
            else if (uiStayCanvas == CanvasType.Top)
            {
                par = m_canvasTop.CanvasTransform;
            }


            if (loadingpanel.m_dicBufObj.ContainsKey(panelType))
            {
                GameObject obj = loadingpanel.m_dicBufObj[panelType];
                obj.transform.SetParent(par, false);
                AfterCreate(obj, panelType, isAni);
                if (onFinish != null) onFinish(obj);
            }
            else
            {
                AssetMgr.Instance.CreateObjOne(panelType, panelType, par, Vector3.zero, Vector3.zero, Vector3.one, (obj) =>
                {
                    AfterCreate(obj, panelType, isAni);
                    loadingpanel.m_dicBufObj[panelType] = obj;
                    if (onFinish != null) onFinish(obj);
                });
            }


            //AssetMgr.Instance.CreateObjOne(panelType, panelType, par, Vector3.zero,Vector3.zero,Vector3.one,(obj)=> { AfterCreate(obj,panelType,isAni); if (onFinish != null )onFinish(obj); });

        }

        void AfterCreate(GameObject obj, string panelType,bool isAni = false)
        {
            BasePanel panel = obj.GetComponent<BasePanel>();
            panel.m_type = panelType;
            panel.transform.SetAsLastSibling();
            panelStack.Push(panel);
            panel.OnEnter(isAni);//页面进入显示，可交互

            //字典加入
            panelDict[panelType] = panel;
            uiloadpanel.Instance.Close();
        }

        //页面入栈，不影响栈顶ui
        /// <summary>
        /// 
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="uiStayCanvas"></param>
        /// <param name="isHideLast">是否隐藏上一个面板</param>
        /// <returns></returns>
        public void PushPanel(string panelType,bool isHideLast = false,bool isAni = false, UnityAction<GameObject> onFinish = null, bool isAcceptMsg = false,CanvasType uiStayCanvas = CanvasType.Screen)
        {
            uiloadpanel.Instance.Open();
            if (panelStack == null)//如果栈不存在，就实例化一个空栈
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();//取出栈顶元素保存起来，但是不移除
                if (topPanel != null)
                {
                    topPanel.OnPause(isHideLast,isAcceptMsg);//使该页面暂停，不可交互
                }
            }

            Transform par = null;
            if (uiStayCanvas == CanvasType.World)
            {
                par = m_canvasWorld.CanvasTransform;
            }
            else if (uiStayCanvas == CanvasType.Screen)
            {
                par = m_canvasScreen.CanvasTransform;
            }
            else if (uiStayCanvas == CanvasType.Top)
            {
                par = m_canvasTop.CanvasTransform;
            }

            if (loadingpanel.m_dicBufObj.ContainsKey(panelType))
            {
                GameObject obj = loadingpanel.m_dicBufObj[panelType];
                obj.transform.SetParent(par, false);
                AfterCreate(obj, panelType, isAni);
                if (onFinish != null) onFinish(obj);
            }
            else
            {
                AssetMgr.Instance.CreateObjOne(panelType, panelType, par, Vector3.zero, Vector3.zero, Vector3.one, (obj) => {
                    AfterCreate(obj, panelType, isAni);
                    loadingpanel.m_dicBufObj[panelType] = obj;
                    if (onFinish != null) onFinish(obj);});
            }
        }


        public BasePanel PushPanelFromRes(string panelType, CanvasType uiStayCanvas = CanvasType.Screen, bool isHideLast = false, bool isAni = false,bool isLastAcceptMsg = true)
        {
            if (panelStack == null)//如果栈不存在，就实例化一个空栈
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();//取出栈顶元素保存起来，但是不移除
                if (topPanel != null)
                {
                    topPanel.OnPause(isHideLast, isLastAcceptMsg);//使该页面暂停，不可交互
                }
            }
            BasePanel panelTemp = GetPanelFromRes(panelType, uiStayCanvas);
            panelTemp.transform.SetAsLastSibling();
            panelStack.Push(panelTemp);
            panelTemp.OnEnter(isAni);//页面进入显示，可交互
            return panelTemp;
        }

        //判断我当前是否是顶部ui
        public bool IsTopPanel(BasePanel panel)
        {
            BasePanel topPanel = panelStack.Peek();
            if (panel == topPanel)
            {
                return true;
            }
            return false;
        }


        public bool IsTopPanel(string type)
        {
            string top = GetTopPanelType();
            if (top == type)
            {
                return true;
            }
            return false;
        }
        //当前顶部ui类型
        public string GetTopPanelType()
        {
            string ret = "";

            if (panelStack.Count >= 1)
            {
                BasePanel topPanel = panelStack.Peek();
                return topPanel.m_type;
            }
            return "";
        }

        //public bool IsExistInStack(string type)
        //{

        //}
        //是否曾经创建过该ui
        public bool IsExist(string panelType)
        {
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);
            if (panel == null)
            {
                return false;
            }
            return true;
        }


        //当前页面退出，并显示上一个界面
        public void PopPanelShowPeek()
        {
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0) return;
            //关闭栈顶页面的显示
            BasePanel topPanel1 = panelStack.Pop();
            topPanel1.OnExit();
            if (panelStack.Count <= 0) return;
            BasePanel topPanel2 = panelStack.Peek();
            topPanel2.OnResume();//使第二个栈里的页面显示出来，并且可交互
        }

        //当前页面退出
        public void PopSelf(bool isEnableLast = true)
        {
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0) return;
            //关闭栈顶页面的显示
            BasePanel topPanel1 = panelStack.Pop();
            topPanel1.OnExit();
            if (isEnableLast == true)
            {
                if (panelStack.Count <= 0) return;
                BasePanel topPanel2 = panelStack.Peek();
                topPanel2.OnOpen();//使第二个栈里的页面显示出来，并且可交互
            }
        }

        public void PopAll()
        {
            while (panelStack.Count > 0)
            {
                BasePanel topPanel1 = panelStack.Pop();
                topPanel1.OnExit();
            }
        }
        // 所有显示的页面隐藏，所有页面出栈
        public void PopAllOld()
        {
            if (panelDict != null)
            {
                foreach (var kvp in panelDict)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.OnExit();
                    }
                }
            }

           
            if (panelStack != null)
            {
                panelStack.Clear ();
            }
            
        }

        // 所有显示的页面摧毁，所有页面出栈
        public void DestroyAll()
        {
            if (panelDict != null)
            {
                foreach (var kvp in panelDict)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.DestroySelf();
                    }
                }

                panelDict.Clear();
            }

            if (panelStack != null)
            {
                panelStack.Clear();
            }
        }

        public Transform GetParent(CanvasType uiStayCanvas)
        {
            Transform tempParent = null;
            if (uiStayCanvas == CanvasType.World)
            {
                tempParent = m_canvasWorld.CanvasTransform;
            }
            else if (uiStayCanvas == CanvasType.Screen)
            {
                tempParent = m_canvasScreen.CanvasTransform;
            }
            else if (uiStayCanvas == CanvasType.Top)
            {
                tempParent = m_canvasTop.CanvasTransform;
            }
            return tempParent;
        }

        // 加载面板显示在屏幕，但不存在栈中
        public BasePanel LoadPanel(string panelType,CanvasType type)
        {
            BasePanel tempPanel = GetPanel(panelType, type);
            tempPanel.OnEnter();
            m_dicNotInStack[panelType] = tempPanel;
            return tempPanel;
        }


        public GameObject GetLoadObject(string path)
        {
            return (Resources.Load(path) as GameObject);
        }

        internal void PushPanel(object paypanel, bool v1, bool v2, Action<GameObject> p, bool v3)
        {
            throw new NotImplementedException();
        }

        private BasePanel GetPanelFromRes(string panelType, CanvasType uiStayCanvas)
        {
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);//不为空就根据类型得到Basepanel
            if (panel == null)//如果得到的panel为空，那就去panelPathDict字典里面根据路径path找到，然后加载，接着实例化
            {
                Transform par = null;
                if (uiStayCanvas == CanvasType.World)
                {
                    par = m_canvasWorld.CanvasTransform;
                }
                else if (uiStayCanvas == CanvasType.Screen)
                {
                    par = m_canvasScreen.CanvasTransform;
                }
                else if (uiStayCanvas == CanvasType.Top)
                {
                    par = m_canvasTop.CanvasTransform;
                }
                //GameObject obj = AssetMgr.Instance.CreateObjSync(panelType, panelType, Vector3.zero, Vector3.zero, Vector3.one, par);


                GameObject obj = PublicFunc.CreateObjFromRes(panelType, par);
                //obj.transform.localScale = Vector3.one;
  

                panel = obj.GetComponent<BasePanel>();
                panel.m_type = panelType;
                if (panelDict.ContainsKey(panelType))
                {
                    panelDict[panelType] = panel;
                }
                else
                {
                    panelDict.Add(panelType, panel);
                }

            }
            return panel;
        }
    
        //根据面板类型UIPanelType得到实例化的面板
        private BasePanel GetPanel(string panelType, CanvasType uiStayCanvas)
        {
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);//不为空就根据类型得到Basepanel
            if (panel == null)//如果得到的panel为空，那就去panelPathDict字典里面根据路径path找到，然后加载，接着实例化
            {
                Transform par = null;
                if (uiStayCanvas == CanvasType.World)
                {
                    par = m_canvasWorld.CanvasTransform;
                }
                else if (uiStayCanvas == CanvasType.Screen)
                {
                    par = m_canvasScreen.CanvasTransform;
                }
                else if (uiStayCanvas == CanvasType.Top)
                {
                    par = m_canvasTop.CanvasTransform;
                }
                //GameObject obj = AssetMgr.Instance.CreateObjSync(panelType, panelType, Vector3.zero, Vector3.zero, Vector3.one, par);
                GameObject obj = AssetMgr.Instance.CreateObjSync(panelType, panelType, par);
                //obj.transform.localScale = Vector3.one;
                
                panel = obj.GetComponent<BasePanel>();
                panel.m_type = panelType;
                //if (panelDict.ContainsKey(panelType))
                //{
                //    panelDict[panelType] = panel;
                //}
                //else
                //{
                //    panelDict.Add(panelType, panel);
                //}
                
            }
            return panel;
        }

        public BasePanel GetPanelFromCache(string panelType)
        {
            if (panelDict == null)
            {
                return null;
            }
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);//不为空就根据类型得到Basepanel
            return panel;
        }
}
}