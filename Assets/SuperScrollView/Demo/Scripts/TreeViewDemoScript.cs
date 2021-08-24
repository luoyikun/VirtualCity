using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{

    public class TreeViewDemoScript : MonoBehaviour
    {
        public LoopListView2 mLoopListView;
        Button mScrollToButton;
        Button mExpandAllButton;
        Button mCollapseAllButton;
        InputField mScrollToInputItem;
        InputField mScrollToInputChild;
        Button mBackButton;
        TreeViewItemCountMgr mTreeItemCountMgr = new TreeViewItemCountMgr();
        // Use this for initialization
        void Start()
        {
            int count = TreeViewDataSourceMgr.Get.TreeViewItemCount;
            for (int i = 0; i < count; ++i)
            {
                int childCount = TreeViewDataSourceMgr.Get.GetItemDataByIndex(i).ChildCount;
                mTreeItemCountMgr.AddTreeItem(childCount, true);
            }


            mLoopListView.InitListView(mTreeItemCountMgr.GetTotalItemAndChildCount(), OnGetItemByIndex);

            mExpandAllButton = GameObject.Find("ButtonPanel/buttonGroup1/ExpandAllButton").GetComponent<Button>();
            mScrollToButton = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
            mCollapseAllButton = GameObject.Find("ButtonPanel/buttonGroup3/CollapseAllButton").GetComponent<Button>();
            mScrollToInputItem = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputFieldItem").GetComponent<InputField>();
            mScrollToInputChild = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputFieldChild").GetComponent<InputField>();
            mScrollToButton.onClick.AddListener(OnJumpBtnClicked);
            mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
            mBackButton.onClick.AddListener(OnBackBtnClicked);
            mExpandAllButton.onClick.AddListener(OnExpandAllBtnClicked);
            mCollapseAllButton.onClick.AddListener(OnCollapseAllBtnClicked);


        }

        void OnBackBtnClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }


        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }

            TreeViewItemCountData countData = mTreeItemCountMgr.QueryTreeItemByTotalIndex(index);
            if(countData == null)
            {
                return null;
            }
            int treeItemIndex = countData.mTreeItemIndex;
            TreeViewItemData treeViewItemData = TreeViewDataSourceMgr.Get.GetItemDataByIndex(treeItemIndex);
            if (index == countData.mBeginIndex)
            {
                LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab1");
                ListItem12 itemScript = item.GetComponent<ListItem12>();
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                    itemScript.Init();
                    itemScript.SetClickCallBack(this.OnExpandClicked);
                }
                item.UserIntData1 = treeItemIndex;
                item.UserIntData2 = 0;
                itemScript.mText.text = treeViewItemData.mName;
                itemScript.SetItemData(treeItemIndex, countData.mIsExpand);
                return item;
            }
            else
            {
                int childIndex = index - countData.mBeginIndex -1;
                ItemData itemData = treeViewItemData.GetChild(childIndex);
                if (itemData == null)
                {
                    return null;
                }
                LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab2");
                ListItem13 itemScript = item.GetComponent<ListItem13>();
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                    itemScript.Init();
                }
                item.UserIntData1 = treeItemIndex;
                item.UserIntData2 = childIndex;
                itemScript.SetItemData(itemData, treeItemIndex, childIndex);
                return item;
            }
            
        }
        public void OnExpandClicked(int index)
        {
            mTreeItemCountMgr.ToggleItemExpand(index);
            mLoopListView.SetListItemCount(mTreeItemCountMgr.GetTotalItemAndChildCount(),false);
            mLoopListView.RefreshAllShownItem();
        }
        void OnJumpBtnClicked()
        {
            int itemIndex = 0;
            int childIndex = 0;
            int finalIndex = 0;
            if (int.TryParse(mScrollToInputItem.text, out itemIndex) == false)
            {
                return;
            }
            if (int.TryParse(mScrollToInputChild.text, out childIndex) == false)
            {
                childIndex = 0;
            }
            if (childIndex < 0)
            {
                childIndex = 0;
            }
            TreeViewItemCountData itemCountData = mTreeItemCountMgr.GetTreeItem(itemIndex);
            if(itemCountData == null)
            {
                return;
            }
            int childCount = itemCountData.mChildCount;
            if (itemCountData.mIsExpand == false || childCount == 0 || childIndex == 0)
            {
                finalIndex = itemCountData.mBeginIndex;
            }
            else
            {
                if(childIndex > childCount)
                {
                    childIndex = childCount;
                }
                if (childIndex < 1)
                {
                    childIndex = 1;
                }
                finalIndex = itemCountData.mBeginIndex + childIndex;
            }
            mLoopListView.MovePanelToItemIndex(finalIndex, 0);
        }

        void OnExpandAllBtnClicked()
        {
            int count = mTreeItemCountMgr.TreeViewItemCount;
            for (int i = 0; i < count; ++i)
            {
                mTreeItemCountMgr.SetItemExpand(i, true);
            }
            mLoopListView.SetListItemCount(mTreeItemCountMgr.GetTotalItemAndChildCount(), false);
            mLoopListView.RefreshAllShownItem();
        }

        void OnCollapseAllBtnClicked()
        {
            int count = mTreeItemCountMgr.TreeViewItemCount;
            for (int i = 0; i < count; ++i)
            {
                mTreeItemCountMgr.SetItemExpand(i, false);
            }
            mLoopListView.SetListItemCount(mTreeItemCountMgr.GetTotalItemAndChildCount(), false);
            mLoopListView.RefreshAllShownItem();
        }

    }

}
