using BE;
using Framework.Event;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUnit : MonoBehaviour {

    public float m_height; //自身高度

    public Dictionary<int,GameObject> m_dicTouchObj = new Dictionary<int, GameObject>();

    public bool m_isCanPlace = true;
    GameObject m_myObj;
    Transform m_trans;
    public float m_offsetHeight = 0; // ontable 物体放到 桌子上
    ModelGlint m_glint;

    public PutStatus m_putStatus = new PutStatus();
   
    public Dictionary<int,HomeUnit> m_dicChild = new Dictionary<int, HomeUnit>();// 桌子包含的子物体 key:gameobjectId  
    public HomeUnit m_par ;
    public int m_layer = -1;
    // Use this for initialization
    void Start () {
        m_myObj = gameObject;
        m_trans = transform;

        //AddNavTag();

        m_glint = m_myObj.AddComponent<ModelGlint>();

        BoxCollider box = m_myObj.GetComponent<BoxCollider>();
        m_height = box.size.y;
        //m_glint.StartGlinting();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.HomeUnitRecycle, OnEvHomeUnitRecycle);
   
    }
    private void OnDisable()
    {
        //不可以在隐藏时删除，只能在回收时删除
        //DestroyChildWhenRecyle();
        EventManager.Instance.RemoveEventListener(Common.EventStr.HomeUnitRecycle, OnEvHomeUnitRecycle);
    }

    void OnEvHomeUnitRecycle(EventData data)
    {
        var exdata = data as EventDataEx<List<int>>;
        List<int> listRec = exdata.GetData();
        for (int i = 0; i < listRec.Count; i++)
        {
            m_dicTouchObj.Remove(listRec[i]);
        }
    }
    public void DestroyChildWhenRecyle()
    {
        if (m_myObj != null)
        {
            if (m_myObj.tag == HomeMgr.HomeUnitIsTable)
            {
                foreach (var item in m_dicChild)
                {
                    HomeUnit home = item.Value;
                    HomeMgr.m_instance.RemoveOneDicUnitInLayer(home.m_layer, home.gameObject.GetInstanceID());
                    Debug.Log("删除了子部件");
                    DestroyImmediate(item.Value.gameObject);
                }
                m_dicChild.Clear();
            }
        }

        //transform.rotation = 
    }
    public  void AddNavTag()
    {
        foreach (var item in m_trans.GetComponentsInChildren<Renderer>())
        {
            if (item.GetComponent<NavMeshSourceTag>() == null)
            {
                item.gameObject.AddComponent<NavMeshSourceTag>();
            }
        }
    }
    // Update is called once per frame
    void Update () {
        if (HomeMgr.m_instance.m_selectUnit != null && HomeMgr.m_instance.m_selectUnit == m_myObj.transform)
        {
            if (IsCanPlace() == true)
            {
                m_putStatus.hasPut =  (int)ClickPlace();
                m_layer = (int)ClickPlace();
                m_putStatus.x = m_trans.position.x;

                if (m_par != null)
                {
                    m_putStatus.y = m_par.transform.position.y;
                }
                else {
                    m_putStatus.y = m_trans.position.y;
                }
                
                m_putStatus.z = m_trans.position.z;

                m_putStatus.dirX = m_trans.rotation.x;
                m_putStatus.dirY = m_trans.rotation.y;
                m_putStatus.dirZ = m_trans.rotation.z;
                m_putStatus.dirW = m_trans.rotation.w;

                //PublicFunc.LightHigh(m_myObj.transform,Color.green);

                m_glint.SetColor(Color.green);
                

                if (homeunitplacepanel.m_instance != null)
                {
                    homeunitplacepanel.m_instance.SetOkBtn(true);
                }

                HomeMgr.m_instance.m_isCanPlace = true;
            }
            else {
                m_glint.SetColor(Color.red);

                //PublicFunc.LightHigh(m_myObj.transform, Color.red);
                if (homeunitplacepanel.m_instance != null)
                {
                    homeunitplacepanel.m_instance.SetOkBtn(false);
                }
                HomeMgr.m_instance.m_isCanPlace = false;
            }
        }
        else {
            m_glint.StopGlinting();
            //PublicFunc.LightHighOff(m_myObj.transform);
        }
    }

    public void AddChild(HomeUnit child)
    {
        if (m_dicChild.ContainsKey(child.gameObject.GetInstanceID()) == false)
        {
            m_dicChild[child.gameObject.GetInstanceID()] = child;
        }
    }

    public void RemoveChild(HomeUnit child)
    {
        if (m_dicChild.ContainsKey(child.gameObject.GetInstanceID()))
        {
            m_dicChild.Remove(child.gameObject.GetInstanceID());
        }
    }

    public Dictionary<int, HomeUnit> GetOnTableChild()
    {
        List<int> listDelete = new List<int>();
        foreach (var item in m_dicChild)
        {
            if (item.Value == null)
            {
                listDelete.Add(item.Key);
            }
        }

        for (int i = 0; i < listDelete.Count; i++)
        {
            m_dicChild.Remove(listDelete[i]);
        }

        return m_dicChild;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_dicTouchObj.ContainsKey(other.gameObject.GetInstanceID()) == false)
        {
            m_dicTouchObj[other.gameObject.GetInstanceID()] = other.gameObject;

            if (m_myObj.tag == HomeMgr.HomeUnitOnTable && other.gameObject.tag == HomeMgr.HomeUnitIsTable && m_par == null && 
                ((HomeMgr.m_instance.m_selectUnit == m_myObj.transform && HomeMgr.m_instance.m_buildMode == EnBuildMode.Build) || HomeMgr.m_instance.m_buildMode == EnBuildMode.Display)
                )
            {
                HomeUnit otherUnit = other.gameObject.GetComponent<HomeUnit>();
                m_offsetHeight = otherUnit.m_height;
                Vector3 oldPos = m_trans.position;
                oldPos.y += m_offsetHeight;
                oldPos.y -= 0.01f;
                m_trans.position = oldPos;
                m_trans.SetParent(other.gameObject.transform, true);

                m_par = otherUnit;
                otherUnit.AddChild(this);

                m_putStatus.hasPut = (int)ClickPlace();
                m_putStatus.x = m_trans.position.x;
                m_putStatus.y = other.gameObject.transform.position.y;
                m_putStatus.z = m_trans.position.z;

                m_putStatus.dirX = m_trans.rotation.x;
                m_putStatus.dirY = m_trans.rotation.y;
                m_putStatus.dirZ = m_trans.rotation.z;
                m_putStatus.dirW = m_trans.rotation.w;
            }

        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (m_dicTouchObj.ContainsKey(other.gameObject.GetInstanceID()))
        {
            m_dicTouchObj.Remove(other.gameObject.GetInstanceID());
            HomeUnit otherUnit = other.gameObject.GetComponent<HomeUnit>();
            if (m_myObj.tag == HomeMgr.HomeUnitOnTable && other.gameObject.tag == HomeMgr.HomeUnitIsTable && m_par != null && m_par == otherUnit)
            {
                m_offsetHeight = 0;
                m_myObj.transform.SetParent(HomeMgr.m_instance.m_unitPar, true);

                
                m_par = null;
                otherUnit.RemoveChild(this);
            }
        }


    }

    public EnFloor ClickPlace()
    {
        if (m_myObj.tag == HomeMgr.HomeUnitOnTable && m_par != null)
        {
            return (EnFloor)m_par.m_putStatus.hasPut;
        }


        foreach (var item in m_dicTouchObj)
        {
            if (item.Value.tag == HomeMgr.Floor)
            {
                if (item.Value.name == EnFloor.floor_1l.ToString())
                {
                    
                    return EnFloor.floor_1l;
                }
                else if (item.Value.name == EnFloor.floor_2l.ToString())
                {
                    
                    return EnFloor.floor_2l;
                }
                else if (item.Value.name == EnFloor.floor_3l.ToString())
                {
                    
                    return EnFloor.floor_3l;
                }
            }

            else if (item.Value.tag == HomeMgr.FloorOutDoor)
            {
                return EnFloor.floor_dz;
            }
        }

        return EnFloor.floor_1l;
    }
    public void SetPlace(Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x, pos.y + m_offsetHeight, pos.z);
        m_myObj.transform.position = newPos;
    }

    public bool IsCanPlace()
    {
        if (HomeMgr.m_instance.m_selectUnit != null && HomeMgr.m_instance.m_selectUnit == m_myObj.transform)
        {
            List<int> listDelete = new List<int>();
            string str = "";
            foreach (var item in m_dicTouchObj)
            {
                if (item.Value == null)
                {
                    listDelete.Add(item.Key);
                }
                if (item.Value != null)
                    str += item.Value.name + ",";
            }

            for (int i = 0; i < listDelete.Count; i++)
            {
                m_dicTouchObj.Remove(listDelete[i]);
            }

            Debug.Log("当前unit接触的碰撞：" + str);
        }
        if (gameObject.tag == HomeMgr.HomeUnit)
        {
            bool isTouchFloor = false;
          
            foreach (var item in m_dicTouchObj)
            {
                if (item.Value.tag == "Player" || item.Value.tag == HomeMgr.FloorOutDoor || item.Value.tag == HomeMgr.HomeUnitOutDoor || item.Value.tag == HomeMgr.HomeUnit || item.Value.tag == HomeMgr.Wall || item.Value.tag == HomeMgr.HomeUnitIsTable || item.Value.tag == HomeMgr.HomeUnitOnTable)
                {
                    return false;
                }
                else if (item.Value.tag == HomeMgr.Floor)
                {
                    isTouchFloor = true;
                }
            }

            if (isTouchFloor == true)
            {
                return true;
            }
        }

        else if (gameObject.tag == HomeMgr.HomeUnitOnTable)
        {
            bool isTouchFloorOrUnit = false;
            foreach (var item in m_dicTouchObj)
            {
                if (item.Value.tag == "Player" || item.Value.tag == HomeMgr.FloorOutDoor || item.Value.tag == HomeMgr.HomeUnitOutDoor || item.Value.tag == HomeMgr.HomeUnitOnTable || item.Value.tag == HomeMgr.Wall || item.Value.tag == HomeMgr.HomeUnit)
                {
                    return false;
                }
                else if (item.Value.tag == HomeMgr.Floor || item.Value.tag == HomeMgr.HomeUnitIsTable)
                {
                    isTouchFloorOrUnit = true;
                }
            }

            if (isTouchFloorOrUnit)
            {
                return true;
            }
        }

        else if (gameObject.tag == HomeMgr.HomeUnitIsTable)
        {
            bool isTouchFloorOrUnit = false;
            foreach (var item in m_dicTouchObj)
            {
                if (item.Value.tag == "Player" || item.Value.tag == HomeMgr.FloorOutDoor || item.Value.tag == HomeMgr.HomeUnitOutDoor || item.Value.tag == HomeMgr.HomeUnitIsTable || item.Value.tag == HomeMgr.Wall || item.Value.tag == HomeMgr.HomeUnit)
                {
                    return false;
                }
                else if (item.Value.tag == HomeMgr.Floor || item.Value.tag == HomeMgr.HomeUnitOnTable)
                {
                    isTouchFloorOrUnit = true;
                    if (item.Value.tag == HomeMgr.HomeUnitOnTable && m_dicChild.ContainsKey(item.Key) == false)
                    {
                        return false;
                    }
                    
                }
            }

            if (isTouchFloorOrUnit)
            {
                return true;
            }
        }

        else if (gameObject.tag == HomeMgr.HomeUnitOutDoor)
        {
            bool isTouchOk = false;
            foreach (var item in m_dicTouchObj)
            {
                if (item.Value.tag == "Player" || item.Value.tag == HomeMgr.Floor || item.Value.tag == HomeMgr.HomeUnitOutDoor || item.Value.tag == HomeMgr.HomeUnitIsTable || item.Value.tag == HomeMgr.Wall || item.Value.tag == HomeMgr.HomeUnit)
                {
                    return false;
                }
                else if (item.Value.tag == HomeMgr.FloorOutDoor)
                {
                    isTouchOk = true;
                }
            }

            if (isTouchOk)
            {
                return true;
            }
        }
        return false;
    }


    public void ChildSetPutStatus()
    {
        if (m_par != null)
        {
            m_putStatus.hasPut = (int)ClickPlace();
            m_putStatus.x = m_trans.position.x;
            m_putStatus.y = m_par.transform.position.y;  
            m_putStatus.z = m_trans.position.z;

            m_putStatus.dirX = m_trans.rotation.x;
            m_putStatus.dirY = m_trans.rotation.y;
            m_putStatus.dirZ = m_trans.rotation.z;
            m_putStatus.dirW = m_trans.rotation.w;
        }
    }

    public void InitByDataWhenCreate()
    {
        m_layer = m_putStatus.hasPut;
        HomeMgr.m_instance.m_dicUnitInLayer[m_putStatus.hasPut][gameObject.GetInstanceID()] = gameObject;
    }

    //private void OnDisable()
    //{
    //    m_glint.StopGlinting();
    //}
    //private void OnDestroy()
    //{
    //    if (HomeMgr.m_instance.m_dicUnit.ContainsKey(m_putStatus.id))
    //    {
    //        HomeMgr.m_instance.m_dicUnit.Remove(m_putStatus.id);
    //    }
    //}
}
