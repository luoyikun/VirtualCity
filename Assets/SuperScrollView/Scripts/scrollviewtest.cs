using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;

public class scrollviewtest : MonoBehaviour {
    public LoopListView2 scrollview;
	// Use this for initialization
	void Start () {
        scrollview.InitListView(9, OnGetItemByIndex);
	}
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= DataSourceMgr.Get.TotalItemCount)
        {
            return null;
        }

        ItemData itemData = DataSourceMgr.Get.GetItemDataByIndex(index);
        if (itemData == null)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab1");
        ListItem2 itemScript = item.GetComponent<ListItem2>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            itemScript.Init();
        }

        itemScript.SetItemData(itemData, index);
        return item;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
