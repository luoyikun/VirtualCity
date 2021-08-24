using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSocket : SocketManager
{

    private static OtherSocket _instance;
    public static OtherSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new OtherSocket();

                OtherSocket._instance.m_name = "Other";
            }
            return _instance;
        }
    }
}
