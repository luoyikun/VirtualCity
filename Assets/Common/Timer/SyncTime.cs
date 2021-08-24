using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTime : MonoBehaviour {

    public static DateTime UtcNow { get { return DateTime.UtcNow.AddSeconds(DeltaTime); } }
    //public static DateTime Now { get { return DateTime.Now.AddSeconds(DeltaTime); } }

    private static double DeltaTime = 0;
    private static double ServerTime = 0; //服务器现在的时间戳
    private static double ValidStartGameTime = 0; //游戏启动的时间

    //同步服务器时间
    public static void Sync(long time)
    {
        ValidStartGameTime = Time.realtimeSinceStartup;
        
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(time);
        DeltaTime = (dt - DateTime.UtcNow).TotalSeconds;
        ServerTime = time;
    }

    //剩余时间
    public static TimeSpan GetLeftTime(long validTime)
    { 
        DateTime date1970 = new DateTime(1970, 1, 1, 0, 0, 0);
        date1970 = date1970.ToLocalTime();
        //TimeSpan ts = date1970.AddSeconds((double)validTime).Subtract(SyncTime.UtcNow);

        TimeSpan ts = date1970.AddSeconds((double)validTime).Subtract(GetSystemTime());
        return ts;
    }

    //当前服务器时间
    public static DateTime GetSystemTime()
    {
        DateTime date1970 = new DateTime(1970, 1, 1, 0, 0, 0);
        date1970 = date1970.ToLocalTime();
        DateTime dateTime = date1970.AddSeconds((double)ServerTime).AddSeconds((double)(Time.realtimeSinceStartup - ValidStartGameTime));
        return dateTime;
    }


    // 时间戳 转换为时间 毫秒
    public static DateTime MilliStampToDateTime(string timeStamp)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long mTime = long.Parse(timeStamp + "0000");
        //long mTime = long.Parse(timeStamp);
        TimeSpan toNow = new TimeSpan(mTime);
        //Debug.Log("\n 当前时间为：" + startTime.Add(toNow).ToString("yyyy/MM/dd HH:mm:ss:ffff"));
        Debug.Log("\n 当前时间为：" + startTime.Add(toNow).ToString("yyyy/MM/dd HH:mm:ss"));
        return startTime.Add(toNow);
    }

    // 时间转时间戳  毫秒
    public static string DateTimeToStampMilli(DateTime now)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (long)(now - startTime).TotalMilliseconds; // 相差毫秒数
        //long timeStamp = (long)(now - startTime).TotalSeconds; // 相差毫秒数
        Debug.Log("\n 当前 时间戳为：" + timeStamp);
        return timeStamp.ToString();
    }

    public static long DateTime2Stamp(DateTime now)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (long)(now - startTime).TotalSeconds; // 相差秒数
        return timeStamp;
    }

    public static DateTime Stamp2DataTime(long stamp)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        DateTime dt = startTime.AddSeconds(stamp);
        return dt;
    }

    public static long Server2Stamp(string dateTime)
    {
        long ret = 0;
        string[] bufBig = dateTime.Split('T');

        string[] bufDate = bufBig[0].Split('-');

        string[] bufTime = bufBig[1].Split(':');

        string[] second = bufTime[2].Split('.'); 
        DateTime date = new DateTime(int.Parse(bufDate[0]), int.Parse(bufDate[1]), int.Parse(bufDate[2]), int.Parse(bufTime[0]), int.Parse(bufTime[1]), int.Parse(second[0]));

        ret = DateTime2Stamp(date);
        return ret;
    }

}
