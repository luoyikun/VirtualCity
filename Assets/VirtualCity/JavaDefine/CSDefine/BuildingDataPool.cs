using System.Collections.Generic;
namespace ProtoDefine {

public class BuildingDataPool {
    //cmd请求协议枚举
    /** 请求－建房屋 */
    public const  short REQ_BUILTEHOUSE = 1;
    /** 请求- 拆房屋*/
    public const  short REQ_REMOVEHOUSE= 2;
    /** 请求－开发建筑 */
    public const  short REQ_BUILTEDEVLOPMENT = 3;
    /** 请求－拆除建筑 */
    public const  short REQ_REMOVEDEVLOPMENT = 4;
    /** 请求-开垦土地*/
    public const  short REQ_OPENAREA = 5;
    /** 请求-获取建筑收益状态*/
    public const  short REQ_GETREWARD = 6;



    //cmd响应协议枚举
    /** 响应－建造 */
    public const  short RSP_BUILTEHOUSE = 501;



    /** 失败标识 */
    public const  short FAIL = 0;
    /** 成功标识 */
    public const  short SUCC = 1;

    /**角色信息标识--房屋*/
    public const  short HOUSE = 611;
    /**角色信息标识--土地*/
    public const  short LAND = 612;
    /**角色信息标识--建筑*/
    public const  short DEVLOPMENT = 613;
    /**角色信息标识--家具*/
    public const  short PART = 614;

    /**好友操作标识--帮忙*/
    public const  short HELP = 0;
    /**角色信息标识--收取*/
    public const  short STONE = 1;

    /**建筑状态标识--开发中*/
    public const  short BULIDING = 0;
    /**建筑状态标识--正在收益*/
    public const  short REWARDING = 1;
    /**建筑状态标识--待翻新*/
    public const  short REBUILDING = 3;


}
}