using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSocket : SocketManager
{

    private static RoomSocket _instance;
    public static RoomSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomSocket();
                RoomSocket._instance.m_name = "Room";

            }
            return _instance;
        }
    }
}
