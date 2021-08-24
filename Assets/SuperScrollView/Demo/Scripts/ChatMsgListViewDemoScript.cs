using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{

    public class ChatMsgListViewDemoScript : MonoBehaviour
    {
        public LoopListView2 mLoopListView;
        Button mScrollToButton;
        InputField mScrollToInput;
        Button mBackButton;

        // Use this for initialization
        void Start()
        {
            mLoopListView.InitListView(ChatMsgDataSourceMgr.Get.TotalItemCount, OnGetItemByIndex);
            mScrollToButton = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
            mScrollToInput = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
            mScrollToButton.onClick.AddListener(OnJumpBtnClicked);
            mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
            mBackButton.onClick.AddListener(OnBackBtnClicked);
        }

        void OnBackBtnClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }


        void OnJumpBtnClicked()
        {
            int itemIndex = 0;
            if (int.TryParse(mScrollToInput.text, out itemIndex) == false)
            {
                return;
            }
            if (itemIndex < 0)
            {
                return;
            }
            mLoopListView.MovePanelToItemIndex(itemIndex, 0);
        }

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0 || index >= ChatMsgDataSourceMgr.Get.TotalItemCount)
            {
                return null;
            }

            ChatMsg itemData = ChatMsgDataSourceMgr.Get.GetChatMsgByIndex(index);
            if (itemData == null)
            {
                return null;
            }
            LoopListViewItem2 item = null;
            if (itemData.mPersonId == 0)
            {
                item = listView.NewListViewItem("ItemPrefab1");
            }
            else
            {
                item = listView.NewListViewItem("ItemPrefab2");
            }
            ListItem4 itemScript = item.GetComponent<ListItem4>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                itemScript.Init();
            }
            itemScript.SetItemData(itemData,index);
            return item;
        }

    }

}
