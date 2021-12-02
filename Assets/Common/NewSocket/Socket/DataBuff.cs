using System.IO;
using System;
using UnityEngine;
using SGF.Network.Core;

//常量数据
public class Constants
{
    //消息：数据总长度(4byte) + 数据类型(2byte) + 数据(N byte)
    public static int HEAD_DATA_LEN = 4;
    public static int HEAD_TYPE_LEN = 2;
    public static int HEAD_LEN//6byte
    {
        get { return HEAD_DATA_LEN + HEAD_TYPE_LEN + HEAD_TYPE_LEN; }
    }
}

/// <summary>
/// 网络数据结构
/// </summary>
[System.Serializable]
public struct sSocketData
{
    public string key;
    public byte[] _data;
    public eProtocalCommand _protocallType;
    public int _buffLength;
    public int _dataLength;
}

/// <summary>
/// 网络数据缓存器，
/// </summary>
[System.Serializable]
public class DataBuffer
{//自动大小数据缓存器
    private int _minBuffLen;
    private byte[] _buff; //待处理字节流:网络接收到数据，往这里塞；多了，会扩容；每次处理完一条完整协议a，截取掉前面a所有的数据后，尾部的未处理直接流又组成新的_buff
    private int _curBuffPosition;
    private int _buffLength = 0; //提取出的
    private int _dataLength;
    private UInt16 _protocalType;
    public string m_key = "";
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="_minBuffLen">最小缓冲区大小</param>
    public DataBuffer(int _minBuffLen = 1024)
    {
        if (_minBuffLen <= 0)
        {
            this._minBuffLen = 1024;
        }
        else
        {
            this._minBuffLen = _minBuffLen;
        }
        _buff = new byte[this._minBuffLen];
    }

    /// <summary>
    /// 添加缓存数据
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_dataLen"></param>
    public void AddBuffer(byte[] _data, int _dataLen)
    {
        if (_dataLen > _buff.Length - _curBuffPosition)//接收的长度，要塞入_buff中，_buff剩余容量不够，扩容
        {
            byte[] _tmpBuff = new byte[_curBuffPosition + _dataLen];
            Array.Copy(_buff, 0, _tmpBuff, 0, _curBuffPosition);
            Array.Copy(_data, 0, _tmpBuff, _curBuffPosition, _dataLen);
            _buff = _tmpBuff; //生成新的扩容后_buff
            _tmpBuff = null;
        }
        else //剩余空间还够，直接塞入
        {
            Array.Copy(_data, 0, _buff, _curBuffPosition, _dataLen);
        }
        _curBuffPosition += _dataLen;//修改当前数据标记
    }

    /// <summary>
    /// 更新数据长度
    /// </summary>
    public void UpdateDataLength()
    {
        if (_dataLength == 0 && _curBuffPosition >= Constants.HEAD_LEN)
        {
            //从0号位提取4位包长字节流
            byte[] tmpDataLen = new byte[Constants.HEAD_DATA_LEN];
            Array.Copy(_buff, 0, tmpDataLen, 0, Constants.HEAD_DATA_LEN);
            //小端接收，要转换下，转换位包长int
            _buffLength = BitConverter.ToInt32(NetBuffer.ReverseOrder(tmpDataLen), 0)+4; //得到包长度

            //提取moudleID
            byte[] tmpProtocalType = new byte[Constants.HEAD_TYPE_LEN];
            Array.Copy(_buff, Constants.HEAD_DATA_LEN, tmpProtocalType, 0, Constants.HEAD_TYPE_LEN);
            ushort module = BitConverter.ToUInt16(NetBuffer.ReverseOrder(tmpProtocalType), 0);

            //提取cmdID
            byte[] tmpCmd = new byte[Constants.HEAD_TYPE_LEN];
            Array.Copy(_buff, Constants.HEAD_DATA_LEN + Constants.HEAD_TYPE_LEN, tmpCmd, 0, Constants.HEAD_TYPE_LEN);
            ushort cmd = BitConverter.ToUInt16(NetBuffer.ReverseOrder(tmpCmd), 0);

            m_key = module.ToString() + "," + cmd.ToString();

            //内容字节流为全长度 - （4+2+2）
            _dataLength = _buffLength - Constants.HEAD_LEN;
        }
    }

    /// <summary>
    /// 获取一条可用数据，返回值标记是否有数据
    /// </summary>
    /// <param name="_tmpSocketData"></param>
    /// <returns></returns>
    public bool GetData(out sSocketData _tmpSocketData)
    {
        _tmpSocketData = new sSocketData();

        //_buffLength如果没提取过为 0 ，提取一次，取全包长（4+2+2+内容字节流），使用后又重置为 0 
        if (_buffLength <= 0)
        {
            UpdateDataLength();
        }

        if (_buffLength > 0 && _curBuffPosition >= _buffLength)
        {
            _tmpSocketData._buffLength = _buffLength;
            _tmpSocketData._dataLength = _dataLength;
            _tmpSocketData._protocallType = (eProtocalCommand)_protocalType;
            _tmpSocketData.key = m_key;
            _tmpSocketData._data = new byte[_dataLength];
            Array.Copy(_buff, Constants.HEAD_LEN, _tmpSocketData._data, 0, _dataLength); //_buff 中从 （4+2+2）开始，复制给内容字节流
            _curBuffPosition -= _buffLength; //当前接收到一条网络数据流里还未处理完的字节流 长度 =  总长度（当前长度） - _buffLength（一条完整数据长度）
            byte[] _tmpBuff = new byte[_curBuffPosition < _minBuffLen ? _minBuffLen : _curBuffPosition];
            Array.Copy(_buff, _buffLength, _tmpBuff, 0, _curBuffPosition);
            _buff = _tmpBuff; //重新复制新的待处理字节流


            _buffLength = 0;
            _dataLength = 0;
            _protocalType = 0;
            return true;
        }
        return false;
    }
    
}