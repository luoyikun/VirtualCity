using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;

public class rewardmoneytreepanel : UGUIPanel {

    public void Init(int state)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(state).gameObject.SetActive(true);
        ClickListener.Get(transform.GetChild(state).gameObject).onClick = clickBack;
    }

    void clickBack(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
}
