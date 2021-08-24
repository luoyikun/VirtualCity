using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;

public class HallSocket : SocketManager
{

    private static HallSocket _instance;
    public  static HallSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HallSocket();
                HallSocket._instance.m_name = "Hall";

            }
            return _instance;
        }
    }
}
