using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class turntablepanel : UGUIPanel
{
    public GameObject startbut;
    public GameObject background;
    public GameObject currentobj;
    public Transform turntablerot;
    Coroutine IErecord;
    Coroutine IEturn;



    public Transform[] Iconpos;
    public Transform icon_Effect_pos;
    public GameObject backbut;
    public GameObject Luck_draw_but;
    int initial_idx=0;
    bool star;
    // Use this for initialization
    void Start () {
        ClickListener.Get(startbut).onClick = startClick;
        ClickListener.Get(background.transform.GetChild(0).gameObject).onClick = CollectClick;




        ClickListener.Get(backbut).onClick = backClick;
        ClickListener.Get(Luck_draw_but).onClick = start_Click;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnOpen()
    {

    }

    public override void OnClose()
    {

    }
    void startClick(GameObject obj)
    {
        if (IErecord != null)
        {
            StopCoroutine(IErecord);
        }
        IErecord = StartCoroutine(objrotate());
    }


    void CollectClick(GameObject obj)
    {
        background.SetActive(false);
    }

    IEnumerator objrotate()
    {
        while (true)
        {
            turntablerot.Rotate(Vector3.back, 14f);
            yield return null;
        }
    }
   
    public void ac()
    {
        StopCoroutine(IErecord);
        turntablerot.localRotation = new Quaternion(0, 0, 0, 0);
        turntablerot.Rotate(Vector3.back, 45f);
        background.SetActive(true);
    }

    void initial()
    {
        for (int i = 0; i < Iconpos.Length; i++)
        {
            Iconpos[i].GetComponent<Image>();
        }
    }

    public void move()
    {
        if (IEturn != null)
        {
            StopCoroutine(IEturn);
        }
        IEturn =StartCoroutine(Iconturn());
    }

    IEnumerator Iconturn()
    {
        int i = 0;
        while (i< Iconpos.Length)
        {
            currentobj.transform.position = Iconpos[i].position;
            yield return new WaitForSeconds(0.2f);
            i++;
        }
    }








    void start_Click(GameObject obj)
    {
        if (!star)
        {
            star = true;
            StartCoroutine(ie_icon_move(Random.Range(0, 14)));
        }
    }

    IEnumerator ie_icon_move(int index)
    {
        Debug.Log("抽中的下标是"+index.ToString());
        int idx= initial_idx;
        int second = 0;
        int ac = (Iconpos.Length-idx)+ index;
        if (ac > Iconpos.Length)
            ac -= Iconpos.Length;

        ac += Iconpos.Length * 3 + 1;
        float speed = 0.08f;
        while (second<  ac)
        {
            icon_Effect_pos.position = Iconpos[idx].position;

            if (ac-12 < second)
                speed += 0.04f;

            yield return new WaitForSeconds(speed);
            second++;
            if (idx < Iconpos.Length-1)
                idx++;
            else
                idx = 0;
        }
        initial_idx = idx;
        star = false;
    }

    void backClick(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
    }

}