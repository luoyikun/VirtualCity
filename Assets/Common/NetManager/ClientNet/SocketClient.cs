using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using Net;
using SGF.Network.Core;
using SGF.Codec;

public enum DisType
{
    Exception,
    Disconnect,
}

public class SocketClient
{
    private TcpClient client = null;
    private NetworkStream outStream = null;
    private MemoryStream memStream;
    private BinaryReader reader;

    private const int MAX_READ = 81920;
    private byte[] byteBuffer = new byte[MAX_READ];

    public string m_name;

    public bool m_isOK = false;
    public string m_ip;
    public int m_port;
    // Use this for initialization
    public SocketClient()
    {
    }

    /// <summary>
    /// 注册代理
    /// </summary>
    public void OnRegister()
    {
        memStream = new MemoryStream();
        reader = new BinaryReader(memStream);
    }

    /// <summary>
    /// 移除代理
    /// </summary>
    public void OnRemove()
    {
        this.Close();
        reader.Close();
        memStream.Close();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    void ConnectServer(string host, int port)
    {
        client = null;
        client = new TcpClient();
        client.SendTimeout = 1000;
        client.ReceiveTimeout = 1000;
        client.NoDelay = true;
        m_ip = host;
        m_port = port;
        try
        {

           client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);

        }
        catch (Exception e)
        {
            Close();
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 连接上服务器
    /// </summary>
    void OnConnect(IAsyncResult asr)
    {

        m_isOK = true;
        outStream = client.GetStream();

        client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
 
        NetManager.Instance.OnConnect(m_name);
    }

    /// <summary>
    /// 写数据
    /// </summary>
    void WriteMessage(byte[] message)
    {
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(message);
            writer.Flush();
            if (client != null && client.Connected)
            {
                byte[] payload = ms.ToArray();
                outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
            }
            else
            {
                Debug.LogError(m_name + ":client.connected----->>false");
            }
        }
    }

    /// <summary>
    /// 读取消息
    /// </summary>
    void OnRead(IAsyncResult asr)
    {
        int bytesRead = 0;
        try
        {
            lock (client.GetStream())
            {         //读取字节流到缓冲区
                bytesRead = client.GetStream().EndRead(asr);
            }
            if (bytesRead < 1)
            {                //包尺寸有问题，断线处理
                Debug.Log("包尺寸有问题，断线处理");
                //OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                return;
            }
            OnReceive(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
            lock (client.GetStream())
            {         //分析完，再次监听服务器发过来的新消息
                Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组


              client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);

                //client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
            }
        }
        catch (Exception ex)
        {
            //PrintBytes();
            OnDisconnected(DisType.Exception, ex.Message);
        }
    }

    /// <summary>
    /// 丢失链接
    /// </summary>
    void OnDisconnected(DisType dis, string msg)
    {
        m_isOK = false;
        Debug.Log("OnDisconnected" + msg);
        Close();   //关掉客户端链接
        NetManager.Instance.OnDisConnect(m_name);
    }

    /// <summary>
    /// 打印字节
    /// </summary>
    /// <param name="bytes"></param>
    void PrintBytes()
    {
        string returnStr = string.Empty;
        for (int i = 0; i < byteBuffer.Length; i++)
        {
            returnStr += byteBuffer[i].ToString("X2");
        }
        Debug.LogError(returnStr);
    }

    /// <summary>
    /// 向链接写入数据流
    /// </summary>
    void OnWrite(IAsyncResult r)
    {
        try
        {
            outStream.EndWrite(r);
        }
        catch (Exception ex)
        {
            Debug.LogError("OnWrite--->>>" + ex.Message);
        }
    }

    /// <summary>
    /// 接收到消息
    /// </summary>
    void OnReceive(byte[] bytes, int length)
    {
        /*memStream.Seek(0, SeekOrigin.End);
        memStream.Write(bytes, 0, length);
        //Reset to beginning
        memStream.Seek(0, SeekOrigin.Begin);
        while (RemainingBytes() > 2)
        {
            ushort messageLen = reader.ReadUInt16();
            if (RemainingBytes() >= messageLen)
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(ms);
                writer.Write(reader.ReadBytes(messageLen));
                ms.Seek(0, SeekOrigin.Begin);
                OnReceivedMessage(ms);
            }
            else
            {
                memStream.Position = memStream.Position - 2;
                break;
            }
        }
        byte[] leftover = reader.ReadBytes((int)RemainingBytes());
        memStream.SetLength(0);    
        memStream.Write(leftover, 0, leftover.Length);*/

        OnReceivedMessage(bytes, length);
    }

    /// <summary>
    /// 剩余的字节
    /// </summary>
    private long RemainingBytes()
    {
        return memStream.Length - memStream.Position;
    }

    /// <summary>
    /// 接收到消息
    /// </summary>
    /// <param name="ms"></param>
    void OnReceivedMessage(MemoryStream ms)
    {
        BinaryReader r = new BinaryReader(ms);
        byte[] message = r.ReadBytes((int)(ms.Length - ms.Position));

        NetMessage msg1 = new NetMessage();
        msg1.Deserialize(message, message.Length);

        LoginRspxx pro = PBSerializer.NDeserialize<LoginRspxx>(msg1.content);

        Debug.Log("get:" + pro.tips);
        /*ByteBuffer buffer = new ByteBuffer(message);
        int mainId = buffer.ReadShort();
        int pbDataLen = message.Length - 2;
        byte[] pbData = buffer.ReadBytes(pbDataLen);
        NetManager.Instance.DispatchProto(mainId, pbData);*/
        //NetManager.Instance.DispatchProto(mainId, pbData);
    }

    void OnReceivedMessage(byte[] ms,int len)
    {
        NetMessage msg1 = new NetMessage();
        msg1.Deserialize(ms, len);
        string sByte = "";
        for (int i = 0; i < msg1.content.Length; i++)
        {
            sByte += msg1.content[i].ToString();
            sByte += ",";
        }
        
        string key = msg1.head.moduleId + "," + msg1.head.cmd;
        NetManager.Instance.DispatchProto(key, msg1.content);


    }
    /// <summary>
    /// 会话发送
    /// </summary>
    void SessionSend(byte[] bytes)
    {
        WriteMessage(bytes);
    }

    /// <summary>
    /// 关闭链接
    /// </summary>
    public void Close()
    {
        if (client != null)
        {
            if (client.Connected) client.Close();
            client = null;
        }
    }

    /// <summary>
    /// 发送连接请求
    /// </summary>
    public void SendConnect(string ip,int port)
    {
        ConnectServer(ip, port);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMessage(ByteBuffer buffer)
    {
        SessionSend(buffer.ToBytes());
        buffer.Close();
    }

    public void SendMsg(byte[] bytes)
    {
        SessionSend(bytes);
    }
}
