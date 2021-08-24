using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AddWall : Editor {
    [MenuItem("HomeTool/AddAllOneBuild")]
    public static void AddAllOneBuild()
    {
        GameObject obj = Selection.activeGameObject;
        ChangeTagToNoRecvLight(obj);

        AddNavTag();

        //NavMeshSurface nav = obj.GetComponent<NavMeshSurface>();
        //nav.collectObjects = CollectObjects.Children;

        //GameObject wallPar = obj.transform.Find("wall").gameObject;
        //AddWallByObj(wallPar);

        //1l 可建造
        Transform floor1L = PublicFunc.GetTransform(obj.transform, "floor_1l");
        if (floor1L != null)
        {
            AddFloorByObj(floor1L.gameObject);
        }

        //2L可建造
        Transform floor2L = PublicFunc.GetTransform(obj.transform, "floor_2l");
        if (floor2L != null)
        {
            AddFloorByObj(floor2L.gameObject);
        }

        //3L可建造
        Transform floor3L = PublicFunc.GetTransform(obj.transform, "floor_3l");
        if (floor3L != null)
        {
            AddFloorByObj(floor3L.gameObject);
        }

        //室外可建造
        Transform floorOutDoor = PublicFunc.GetTransform(obj.transform, "floor_dz");
        if (floorOutDoor != null)
        {
            AddFloorOutDoor(floorOutDoor.gameObject);
        }

        // 屋顶挡住摄像头
        Transform floorWd = PublicFunc.GetTransform(obj.transform, "floor_wd");
        if (floorWd != null)
        {
            if (floorWd.GetComponent<MeshCollider>() == null)
            {
                floorWd.gameObject.AddComponent<MeshCollider>();
            }
        }

        Transform box_1l = PublicFunc.GetTransform(obj.transform, "box_1l");
        if (box_1l != null)
        {
            foreach (var trans in box_1l.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = HomeMgr.Wall;
                trans.gameObject.layer = HomeMgr.m_layerWall;
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }

        Transform box_2l = PublicFunc.GetTransform(obj.transform, "box_2l");
        if (box_2l != null)
        {
            foreach (var trans in box_2l.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = HomeMgr.Wall;
                trans.gameObject.layer = HomeMgr.m_layerWall;
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }

        Transform box_3l = PublicFunc.GetTransform(obj.transform, "box_3l");
        if (box_3l != null)
        {
            foreach (var trans in box_3l.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = HomeMgr.Wall;
                trans.gameObject.layer = HomeMgr.m_layerWall;
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }

        Transform box_dz = PublicFunc.GetTransform(obj.transform, "box_dz");
        if (box_dz != null)
        {
            foreach (var trans in box_dz.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = HomeMgr.Wall;
                trans.gameObject.layer = HomeMgr.m_layerWall;
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }
    }

    [MenuItem("HomeTool/AddMeshWall")]
    public static void DoAddMeshWall()
    {
        GameObject obj = Selection.activeGameObject;
        
        if (obj.gameObject.GetComponent<MeshCollider>() == null)
        {
            obj.gameObject.AddComponent<MeshCollider>();
        }
    }

    public static void ChangeTagToNoRecvLight(GameObject obj)
    {
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            PublicFunc.ChangeLayerForce(trans.gameObject, DataMgr.m_layerNoRecvLight);
        }
    }

    [MenuItem("HomeTool/AddWallTag")]
    public static void DoAddWall()
    {
        GameObject obj = Selection.activeGameObject;

        AddWallByObj(obj);
    }

    public static void AddWallByObj(GameObject obj)
    {
        PublicFunc.ChangeTagForce(obj, HomeMgr.Wall);

        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
        }
    }

    [MenuItem("HomeTool/AddNavMesh")]
    public static void AddNavMesh()
    {
        GameObject obj = Selection.activeGameObject;
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (trans.gameObject.GetComponent<NavMeshSurface>() == null)
            {
                trans.gameObject.AddComponent<NavMeshSurface>();
            }
        }
    }

    [MenuItem("HomeTool/AddFloorOutDoor")]
    public static void DoAddFloorOutDoor()
    {
        GameObject obj = Selection.activeGameObject;
        AddFloorOutDoor(obj);
    }


    static void AddFloorOutDoor(GameObject obj)
    {
        PublicFunc.ChangeTagForce(obj, HomeMgr.FloorOutDoor);
        PublicFunc.ChangeLayerForce(obj, HomeMgr.m_layerOutDoor);
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (trans.gameObject.GetComponent<MeshCollider>() == null)
            {
                trans.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
    [MenuItem("HomeTool/AddFloorTag")]
    public static void DoAddFloor()
    {
        GameObject obj = Selection.activeGameObject;
        AddFloorByObj(obj);
    }


    public static void AddFloorByObj(GameObject obj)
    {
        PublicFunc.ChangeTagForce(obj, HomeMgr.Floor);
        PublicFunc.ChangeLayerForce(obj, HomeMgr.m_layerInDoor);
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (trans.gameObject.GetComponent<MeshCollider>() == null)
            {
                trans.gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    [MenuItem("HomeTool/AddCommUnit")]
    public static void DoAddCommonUnit()
    {
        //GameObject obj = Selection.activeGameObject;
        //obj.tag = HomeMgr.HomeUnit;
        //AddUnit(obj);

        GameObject[] bufObj = Selection.gameObjects;
        for (int i = 0; i < bufObj.Length; i++)
        {
            if (bufObj[i].gameObject.GetComponent<Animator>() != null)
            {
                DestroyImmediate(bufObj[i].gameObject.GetComponent<Animator>(), true);
            }
            AddUnit(bufObj[i]);
        }
    }

    [MenuItem("HomeTool/AddUnitOnTable")]
    public static void DoAddUnitOnTable()
    {
        GameObject obj = Selection.activeGameObject;
        obj.tag = HomeMgr.HomeUnitOnTable;
        AddUnit(obj);
    }

    [MenuItem("HomeTool/AddUnitIsTable")]
    public static void DoAddUnitIsTable()
    {
        GameObject obj = Selection.activeGameObject;
        obj.tag = HomeMgr.HomeUnitIsTable;
        AddUnit(obj);
    }


    [MenuItem("HomeTool/AddUnitOutDoor")]
    public static void DoAddUnitOutDoor()
    {
        GameObject obj = Selection.activeGameObject;
        obj.tag = HomeMgr.HomeUnitOutDoor;
        AddUnit(obj);
    }

    [MenuItem("HomeTool/AddNavTag")]
    public static void AddNavTag()
    {
        GameObject obj = Selection.activeGameObject;
        foreach (var item in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (item.GetComponent<NavMeshSourceTag>() == null)
            {
                item.gameObject.AddComponent<NavMeshSourceTag>();
            }
        }
    }

    public static void AddNavTagByObj(GameObject obj)
    {
        foreach (var item in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (item.GetComponent<NavMeshSourceTag>() == null)
            {
                item.gameObject.AddComponent<NavMeshSourceTag>();
            }
        }
    }

    [MenuItem("HomeTool/AddHomeUnitInAPar")]
    public static void AddHomeUnitInAPar()
    {
        GameObject obj = Selection.activeGameObject;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            AddUnit(obj.transform.GetChild(i).gameObject);
        }
        Debug.Log("共修改家具:" + obj.transform.childCount);
    }

    static void AddUnit(GameObject obj)
    {
        

        AddNavTagByObj(obj);
        Rigidbody rigi;
        if (obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>();
        }
        rigi = obj.GetComponent<Rigidbody>();
        rigi.useGravity = false;
        rigi.isKinematic = true;

        List<BoxCollider> listBox = new List<BoxCollider>();
        foreach (var trans in obj.transform.GetComponentsInChildren<BoxCollider>())
        {
            listBox.Add(trans);
        }

        for (int i = listBox.Count - 1; i >= 0; i++)
        {
            DestroyImmediate(listBox[i], true);
        }

        if (obj.GetComponent<BoxCollider>() == null)
        {
            AddBoxCollider.DoAddBoxColliderByObj(obj);
        }
        //AddBoxCollider.DoAddBoxCollider();
        BoxCollider box = obj.GetComponent<BoxCollider>();
        box.isTrigger = true;
        //if (obj.GetComponent<HomeUnit>() == null)
        //{
        //    obj.AddComponent<HomeUnit>();
        //}
        //HomeUnit unit = obj.GetComponent<HomeUnit>();
        //unit.m_height = box.size.y;

        //if (obj.GetComponent<NavMeshObstacle>() == null)
        //{
        //    obj.AddComponent<NavMeshObstacle>();
        //}

        //NavMeshObstacle nav = obj.GetComponent<NavMeshObstacle>();
        //nav.center = box.center;
        //nav.size = box.size;



    }


}
