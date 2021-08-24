using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace ThisisGame
{

    public class TestEmailUI : MonoBehaviour
    {

        public InfinityGridLayoutGroup infinityGridLayoutGroup;

        int amount = 20;

        // Use this for initialization
        void Start()
        {
            ////初始化数据列表;
            //infinityGridLayoutGroup = transform.Find("Panel_Scroll/Panel_Grid").GetComponent<InfinityGridLayoutGroup>();

            infinityGridLayoutGroup.updateChildrenCallback = UpdateChildrenCallback;


            infinityGridLayoutGroup.SetAmount(10);

            if (infinityGridLayoutGroup.m_isSelect == true)
            {
                for (int i = 0; i < infinityGridLayoutGroup.transform.childCount; i++)
                {
                    Transform trans = infinityGridLayoutGroup.transform.GetChild(i);
                    LoopItem item = trans.GetComponent<LoopItem>();
                    item.m_onSelect = OnSelectItem;
                    item.m_onUnselect = OnUnSelectItem;
                }
            }

            //infinityGridLayoutGroup.SetSelect(2);
            
        }

        private void OnSelectItem(GameObject obj)
        {
            obj.GetComponent<Image>().color = Color.green;
        }

        private void OnUnSelectItem(GameObject obj)
        {
            obj.GetComponent<Image>().color = Color.white;
        }

        void OnGUI()
        {
            if (GUILayout.Button("Add one item"))
            {
                infinityGridLayoutGroup.SetAmount(++amount);
            }
            if (GUILayout.Button("remove one item"))
            {
                infinityGridLayoutGroup.SetAmount(--amount);
            }
        }

        void UpdateChildrenCallback(int index, Transform trans)
        {
            //下面的为不可删除
            if (index >= infinityGridLayoutGroup.amount)
            {
                return;
            }
            if (infinityGridLayoutGroup.m_isSelect)
            {
                LoopItem item = trans.GetComponent<LoopItem>();
                item.m_idx = index;

                if (item.m_idx == infinityGridLayoutGroup.m_selectIdx)
                {
                    if (item.m_onSelect != null)
                    {
                        item.m_onSelect(trans.gameObject);
                    }
                }
                else
                {
                    if (item.m_onUnselect != null)
                    {
                        item.m_onUnselect(trans.gameObject);
                    }
                }
            }
            //上面的为不可删除

            
            Debug.Log("UpdateChildrenCallback: index=" + index + " name:" + trans.name);

            Text text = trans.Find("Text").GetComponent<Text>();
            text.text = index.ToString();

            Debug.Log(index);
        }


        public void CreateScroll()
        {
            infinityGridLayoutGroup.SetAmount(1);
        }
    }

}
