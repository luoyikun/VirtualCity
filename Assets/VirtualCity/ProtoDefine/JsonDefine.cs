using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DicHomeBuildPos
{
   public Dictionary<string, HomeBuildPos> dic = new Dictionary<string, HomeBuildPos>();
}

public class HomeBuildPos
{
    public double x;
    public double y;
    public double z;
    public double dirX;
    public double dirY;
    public double dirZ;
}


public class OpenAreaCast
{
    public int d;
    public int g;
}

public class PosAndAngle
{
    public float x;
    public float y;
    public float z;
    public float dirX;
    public float dirY;
    public float dirZ;
}

public class SysVint
{
    public int v;
}

public class SysVString
{
    public string v;
}

public class SysVFloat
{
    public float v;
}
public class WalletUpdateDiff
{
    public int goldDiff;
    public int diamondDiff;
    public double sMoneyDiff;
    public double moneyDiff;
    public int assetDiff;
}

public class HttpJson
{
    public string code;
    public string message;
}

public class HttpServerInfo
{
    public string chatServerInfo;
    public string hallServerInfo;
}