using Framework.Tools;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gold_effect : UGUIPanel
{
    public GameObject img;

    public Text speed_pos;

    private List<GameObject> img_arr;
    // Use this for initialization

    public static gold_effect instance;

    void Awake()
    {
        instance = this;
        img_arr = new List<GameObject>();
        GameObject obj;
        for (int i = 0; i < 10; i++)
        {
            obj = Instantiate(img);
            obj.transform.parent = transform;
            obj.GetComponent<Image>().enabled = false;
            img_arr.Add(obj);
        }
    }

    void Start () {
       
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

    public  void Dynamic_text(Text text_,int initial,int Final)
    {
        StartCoroutine(IE_Dynamic_text(text_, initial, Final));
    }
    IEnumerator IE_Dynamic_text(Text text_, float initial, float Final)
    {
        float i = (Final - initial) / 26;
        while (initial<Final)
        {
            initial+=i;
            text_.text = ((int)initial).ToString();
            yield return new WaitForSeconds(0.05f); ;
        }

        text_.text = ((int)Final).ToString();
    }

    public  void Speed(Vector2 pos, Vector2 To_pos, string numNumber_text)
    {
        speed_pos.transform.localPosition = new Vector3(To_pos.x, To_pos.y,0f);
        speed_pos.text = string.Format("+ "+numNumber_text);
        for (int i = 0; i < 10; i++)
        {
            img_arr[i].transform.localPosition = new Vector3(Random.Range(pos.x-100, pos.x+100), Random.Range(pos.y - 100, pos.y + 100), 0f);
            img_arr[i].GetComponent<Image>().enabled = true;
            StartCoroutine(speed(img_arr[i]));
        }
    }

    int idx = 0;
    IEnumerator speed(GameObject obj)
    {
        while (obj.transform.localPosition != speed_pos.transform.localPosition)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, speed_pos.transform.localPosition, 15f);
            yield return new WaitForSeconds(0.01f);
        }
        obj.GetComponent<Image>().enabled = false;
        idx++;
        if (idx==9){ speed_pos.text = null; idx = 0; }
    }
}
