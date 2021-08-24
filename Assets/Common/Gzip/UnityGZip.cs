using System;
using System.Text;
using System.IO;
//using ICSharpCode.SharpZipLib;
//using ICSharpCode.SharpZipLib.GZip;
using System.Collections.Generic;
//using BestHTTP.Decompression.Zlib;
using System.IO.Compression;
using UnityEngine;
using Org.BouncyCastle.Utilities.Zlib;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.GZip;


public class UnityGZip {

    static Deflater deflater = new Deflater(Deflater.BEST_SPEED, true);
    static Inflater inflater = new Inflater(true);

    static  byte[] deflaterBuffer = new byte[4096];
    static byte[] inflaterBuffer = new byte[4096];

    static byte[] decompressBuffer = new byte[40960];


    //public static byte[] CompressBytesToBytes(byte[] binData) {
    //    MemoryStream ms = new MemoryStream();
    //    GZipOutputStream gzip = new GZipOutputStream(ms);
    //    gzip.Write(binData, 4, binData.Length - 4);
    //    gzip.Close();
    //    List<Byte> dataList = new List<Byte>();
    //    dataList.Add(binData[0]);
    //    dataList.Add(binData[1]);
    //    dataList.Add(binData[2]);
    //    dataList.Add(binData[3]);
    //    dataList.AddRange(ms.ToArray());
    //    return dataList.ToArray();
    //}

    //public static string CompressBytesToBase64(byte[] binData) {
    //    byte[] compressedData = CompressBytesToBytes(binData);
    //    return Convert.ToBase64String(compressedData);
    //}

    //public static string CompressStringToBase64(string strData) {
    //    byte[] binData = Encoding.Default.GetBytes(strData);
    //    return CompressBytesToBase64(binData);
    //}

    //public static byte[] DecompressBase64ToBytes(string b64Data) {
    //    byte[] compressingData = Convert.FromBase64String(b64Data);
    //    MemoryStream ms = new MemoryStream(compressingData);
    //    GZipInputStream gzip = new GZipInputStream(ms);
    //    MemoryStream outBuffer = new MemoryStream();
    //    byte[] block = new byte[1024];
    //    while (true) {
    //        int bytesRead = gzip.Read(block, 0, block.Length);
    //        if (bytesRead <= 0)
    //            break;
    //        else
    //            outBuffer.Write(block, 0, bytesRead);
    //    }
    //    gzip.Close();
    //    return outBuffer.ToArray();

    //}

    //public static byte[] DecompressBytesToBytes(byte[] compressData) {
    //    MemoryStream ms = new MemoryStream(compressData);
    //    GZipInputStream gzip = new GZipInputStream(ms);
    //    MemoryStream outBuffer = new MemoryStream();
    //    byte[] block = new byte[1024];
    //    while (true) {
    //        int bytesRead = gzip.Read(block, 0, compressData.Length);
    //        if (bytesRead <= 0)
    //            break;
    //        else
    //            outBuffer.Write(block, 0, bytesRead);
    //    }
    //    gzip.Close();
    //    return outBuffer.ToArray();
    //}

    //public static string DecompressBase64ToString(string b64Data) {
    //    byte[] bytes = DecompressBase64ToBytes(b64Data);
    //    return Encoding.UTF8.GetString(bytes);
    //}



    public static byte[] Compress(byte[] binary)
    {
        MemoryStream ms = new MemoryStream();
        GZipOutputStream gzip = new GZipOutputStream(ms);
        //gzip.SetLevel(-1);
        //Debug.Log("gzip.GetLevel()" + gzip.GetLevel());
        gzip.Write(binary, 0, binary.Length);
        gzip.Close();
        byte[] press = ms.ToArray();
        return press;
    }

    public static byte[] DeCompress(byte[] press)
    {
        GZipInputStream gzi = new GZipInputStream(new MemoryStream(press));
        MemoryStream re = new MemoryStream();
        int count = 0;
        int len = press.Length;
        byte[] data = new byte[len];
        while ((count = gzi.Read(data, 0, data.Length)) != 0)
        {
            re.Write(data, 0, count);
        }
        byte[] depress = re.ToArray();
        return depress;
    }



    public static byte[] Zip(byte[] byteArray)
    {
        //byte[] byteArray = Encoding.UTF8.GetBytes(value);
        byte[] tmpArray;

        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream sw = new GZipStream(ms, CompressionMode.Compress))
            {
                sw.Write(byteArray, 0, byteArray.Length);
                sw.Flush();
            }
            tmpArray = ms.ToArray();
        }
        return tmpArray;
    }


    //public static byte[] UnZip(byte[] byteArray)
    //{

    //    byte[] tmpArray;

    //    using (MemoryStream msOut = new MemoryStream())
    //    {
    //        using (MemoryStream msIn = new MemoryStream(byteArray))
    //        {
    //            using (GZipStream swZip = new GZipStream(msIn, CompressionMode.Decompress))
    //            {
    //                swZip.CopyTo(msOut);
    //                tmpArray = msOut.ToArray();
    //            }
    //        }
    //    }
    //    return tmpArray;
    //}


    //public static string Zip(string value)
    //{
    //    byte[] byteArray = Encoding.UTF8.GetBytes(value);
    //    byte[] tmpArray;

    //    using (MemoryStream ms = new MemoryStream())
    //    {

    //        using (JZlib.ZOutputStream outZStream = new zlib.ZOutputStream(ms, zlib.zlibConst.Z_DEFAULT_COMPRESSION))
    //        {
    //            outZStream.Write(byteArray, 0, byteArray.Length);
    //            outZStream.Flush();


    //        }
    //        tmpArray = ms.ToArray();
    //    }
    //    return Convert.ToBase64String(tmpArray);
    //}

    /// <summary>
    /// zlib.net 解压函数
    /// </summary>
    /// <param name="strSource">带解压数据源</param>
    /// <returns>解压后的数据</returns>
    //public static string DeflateDecompress(string strSource)
    //{
    //    int data = 0;
    //    int stopByte = -1;
    //    byte[] Buffer = Convert.FromBase64String(strSource); // 解base64
    //    MemoryStream intms = new MemoryStream(Buffer);
    //    JZlib.ZInputStream inZStream = new zlib.ZInputStream(intms);
    //    int count = 1024 * 1024;
    //    byte[] inByteList = new byte[count];
    //    int i = 0;
    //    while (stopByte != (data = inZStream.Read()))
    //    {
    //        inByteList[i] = (byte)data;
    //        i++;
    //    }
    //    inZStream.Close();
    //    return System.Text.Encoding.UTF8.GetString(inByteList, 0, inByteList.Length);

    //}

    ///// <summary>
    ///// zlib.net 压缩函数
    ///// </summary>
    ///// <param name="strSource">待压缩数据</param>
    ///// <returns>压缩后数据</returns>
    //public static string DeflateCompress(string strSource)
    //{
    //    MemoryStream outms = new MemoryStream();
    //    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strSource);
    //    MemoryStream inms = new MemoryStream(bytes);
    //    zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outms, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
    //    try
    //    {
    //        CopyStream(inms, outZStream);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        outZStream.Close();
    //    }
    //    return Convert.ToBase64String(outms.ToArray());
    //}


    //public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
    //{
    //    byte[] buffer = new byte[2000];
    //    int len;
    //    while ((len = input.Read(buffer, 0, 2000)) > 0)
    //    {
    //        output.Write(buffer, 0, len);
    //    }
    //    output.Flush();
    //}


    //static public byte[] GZip(byte[] input, int size, out int length)
    //{
    //    if (size == 0)
    //    {
    //        var memory = new MemoryStream();
    //        deflater.Reset();
    //        using (var stream = new GZipOutputStream(memory, deflater, 4096, deflaterBuffer))
    //        {
    //            stream.Write(input, 0, input.Length);
    //        }

    //        var array = memory.ToArray();
    //        length = array.Length;
    //        return array;
    //    }
    //    else
    //    {
    //        if (size > decompressBuffer.Length)
    //        {
    //            decompressBuffer = new byte[size];
    //        }

    //        inflater.Reset();
    //        using (var stream = new GZipInputStream(new MemoryStream(input), inflater, 4096, inflaterBuffer))
    //        {
    //            stream.Read(decompressBuffer, 0, size);
    //        }

    //        length = size;
    //        return decompressBuffer;
    //    }
    //}

    /// <summary>
    /// 复制流
    /// </summary>
    /// <param name="input">原始流</param>
    /// <param name="output">目标流</param>
    public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
    {
        byte[] buffer = new byte[2000];
        int len;
        while ((len = input.Read(buffer, 0, 2000)) > 0)
        {
            output.Write(buffer, 0, len);
        }
        output.Flush();
    }
  
    
}
