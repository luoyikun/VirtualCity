using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Net;

public class ChatSocket : SocketManager
{

    private static ChatSocket _instance;
    public  static ChatSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ChatSocket();

                ChatSocket._instance.m_name = "Chat";
            }
            return _instance;
        }
    }
}
