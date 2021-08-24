using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBottomTest : MonoBehaviour {
    public LoopListView2 mLoopListView;
    // Use this for initialization
    void Start () {
        mLoopListView.InitListView(100, OnGetItemByIndex);
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= 100)
        {
            return null;
        }

        //ItemData itemData = DataSourceMgr.Get.GetItemDataByIndex(index);
        //if (itemData == null)
        //{
        //    return null;
        //}
        //get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
        //And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
        LoopListViewItem2 item = listView.NewListViewItem("GameObject");
        //ListItem2 itemScript = item.GetComponent<ListItem2>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        //itemScript.SetItemData(itemData, index);
        item.transform.Find("Text").GetComponent<Text>().text = index.ToString();


        return item;
    }
}
