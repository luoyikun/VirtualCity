using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSocket : SocketManager
{

    private static GateSocket _instance;
    public static GateSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GateSocket();
                GateSocket._instance.m_name = "Gate";

            }
            return _instance;
        }
    }
}
