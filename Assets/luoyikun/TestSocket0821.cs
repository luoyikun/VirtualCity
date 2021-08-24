using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSocket0821 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Loom.Current.StartUp();
        //GameSocket.Instance.Connect("127.0.0.1", 5004);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameSocket.Instance.Connect("127.0.0.1", 50006);
        }
    }
}
