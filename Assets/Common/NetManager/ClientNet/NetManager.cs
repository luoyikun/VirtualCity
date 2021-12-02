using UnityEngine;
using Util;
using System.Collections.Generic;
using System;
using System.IO;
using SGF.Network.Core;
using System.Text;
using Framework.Pattern;
using SGF.Codec;
using UnityEngine.Events;
using LitJson;
public enum EnSocket
{
    Hall,
    Game,
    Chat
}
namespace Net
{
    public class NetManager : MonoSingleton<NetManager>
    {
        public delegate void OnNetVoid();
        public OnNetVoid m_onUpdate;
        public OnNetVoid m_onGameContentOk;
        public OnNetVoid m_onChatContentOk;
        public OnNetVoid m_onHallContentOk;
        
        protected override void Init()
        {
            base.Init();
   
            socketClient.OnRegister();
            gameSocket.OnRegister();
            chatSocket.OnRegister();
        }

      
        private SocketClient _socketClient;  //大厅的连接
        private SocketClient m_gameSocket; //游戏内连接
        private SocketClient m_chatSocket; //聊天连接

        public HeartBeatHandler m_heart = new HeartBeatHandler();
        SocketClient socketClient
        {
            get
            {
                if (_socketClient == null)
                {
                    _socketClient = new SocketClient();
                    _socketClient.m_name = "HallSocket";
                }
                return _socketClient;
            }
        }

        SocketClient gameSocket
        {
            get
            {
                if (m_gameSocket == null)
                {
                    m_gameSocket = new SocketClient();
                    m_gameSocket.m_name = "GameSocket";
                }
                return m_gameSocket;
            }
        }

        SocketClient chatSocket
        {
            get
            {
                if (m_chatSocket == null)
                {
                    m_chatSocket = new SocketClient();
                    m_chatSocket.m_name = "ChatSocket";
                }
                return m_chatSocket;
            }
        }

        /// <summary>
        /// 发送链接请求
        /// </summary>
        public void SendConnect(string ip,int port,EnSocket type = EnSocket.Game)
        {
            switch (type)
            {
                case EnSocket.Hall:
                    socketClient.SendConnect(ip, port);
                    break;
                case EnSocket.Game:
                    gameSocket.SendConnect(ip, port);
                    break;
                case EnSocket.Chat:
                    chatSocket.SendConnect(ip, port);
                    break;
                default:
                    break;
            }
            
        }

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void OnRemove(EnSocket type = EnSocket.Game)
        {
            switch (type)
            {
                case EnSocket.Hall:
                    socketClient.OnRemove();
                    break;
                case EnSocket.Game:
                    gameSocket.OnRemove();
                    break;
                case EnSocket.Chat:
                    chatSocket.OnRemove();
                    break;
                default:
                    break;
            }
            
        }

        /// <summary>
        /// 发送SOCKET消息
        /// </summary>
        public void SendMessage(ByteBuffer buffer)
        {
            socketClient.SendMessage(buffer);
        }

        public void SendMsgProto<T>(string msgId, T content,EnSocket type = EnSocket.Game)
        {
            
            NetMessage msg = new NetMessage();
            string[] bufMsgId = msgId.Split(',');
            msg.head.moduleId = short.Parse(bufMsgId[0]);
            msg.head.cmd = short.Parse(bufMsgId[1]);
            msg.content = PBSerializer.NSerialize(content);
            msg.head.packetLength = 2 + 2 + msg.content.Length;
            SendMsg(msg, type);
            string json = JsonMapper.ToJson(content);
            Debug.Log("To server：" + json);

        }
        public void SendMsgOri(string msgId, byte[] msgObj)
        {
            NetMessage msg = new NetMessage();
            string[] bufMsgId = msgId.Split(',');
            msg.head.moduleId = short.Parse(bufMsgId[0]);
            msg.head.cmd = short.Parse(bufMsgId[1]);
            msg.content = msgObj;
            msg.head.packetLength = 2 + 2 + msgObj.Length;
            SendMsg(msg);
        }
        byte[] m_sendBuf = new byte[4096];
        public void SendMsg(NetMessage netMsg, EnSocket type = EnSocket.Game)
        {
            //byte[] tmp = null;
            int len = netMsg.Serialize(out m_sendBuf);
            //byte[] buf1 = new byte[len];
            //Array.Copy(tmp, buf1, len);

            switch (type)
            {
                case EnSocket.Hall:
                    socketClient.SendMsg(m_sendBuf, len);
                    break;
                case EnSocket.Game:
                    gameSocket.SendMsg(m_sendBuf, len);
                    break;
                case EnSocket.Chat:
                    chatSocket.SendMsg(m_sendBuf, len);
                    break;
                default:
                    break;
            }
        }
       


        /// <summary>
        /// 连接 
        /// </summary>
        public void OnConnect(string name)
        {
            Debug.Log(name + "======连接========");
            if (name == "GameSocket")
            {
                if (m_onGameContentOk != null)
                {
                    m_onGameContentOk();
                }
            }

            if (name == "HallSocket")
            {
                if (m_onHallContentOk != null)
                {
                    m_onHallContentOk();
                }
            }

            if (name == "ChatSocket")
            {
                if (m_onChatContentOk != null)
                {
                    m_onChatContentOk();
                }
            }
        }


        public void StartHeart()
        {
            m_heart = new HeartBeatHandler();
            m_heart.m_reqHeartBeatMessage.accountId = (long)DataMgr.m_account.id;
            m_heart.Start();
            //m_heart.m_act = ReContectGameServer;
        }


        /// <summary>
        /// 断开连接
        /// </summary>
        public void OnDisConnect(string name)
        {
            Debug.Log(name+ "======断开连接========");
            if (name == "GameSocket")
            {
                //m_heart.Stop();
            }
        }

        public void DispatchProto(string protoId, byte[] buff)
        {
            sEvents.Enqueue(new KeyValuePair<string, byte[]>(protoId, buff));
        }

        static Queue<KeyValuePair<string, byte[]>> sEvents = new Queue<KeyValuePair<string, byte[]>>();
        /// <summary>
        /// 交给Command，这里不想关心发给谁。
        /// </summary>
        void Update()
        {
            if (sEvents.Count > 0)
            {
                while (sEvents.Count > 0)
                {
                    KeyValuePair<string, byte[]> _event = sEvents.Dequeue();
                    NetEventManager.Instance.DispatchEvent(_event.Key, _event.Value);
                }
            }
            if (m_onUpdate != null)
            {
                m_onUpdate();
            }
        }

        
    }

}
