package com.kingston.jforgame.server.game.mall;

public class MallDataPool {
    //cmd请求协议枚举
    /** 请求－查看订单 */
    public static final short REQ_QUERYORDER = 1;
    /** 请求－生成订单 */
    public static final short REQ_CREATEORDER = 2;
    /** 请求－管理订单 */
    public static final short REQ_MANAGEORDER = 3;
    /** 请求- 查看商品详情*/
    public static final short REQ_GOODSINFO = 4;
    /** 请求－查看评论 */
    public static final short REQ_QUERYCOMMENT = 5;
    /** 请求-新增评论*/
    public static final short REQ_CREATECOMMENT = 6;
    /** 请求-查看账单*/
    public static final short REQ_QUERYBILL = 7;
    /** 请求- 确认收货*/
    public static final short REQ_CONFIRM = 8;
    /** 请求- 查看商品列表*/
    public static final short REQ_GETGOODSLIST = 9;



    //cmd响应协议枚举
    /** 响应－查看订单 */
    public static final short RSP_QUERYORDER = 501;
    /** 响应－生成订单 */
    public static final short RSP_CREATEORDER = 502;
    /** 响应－管理订单 */
    public static final short RSP_MANAGEORDER = 503;
    /** 响应- 查看商品详情*/
    public static final short RSP_GOODSINFO = 504;
    /** 响应－查看评论 */
    public static final short RSP_QUERYCOMMENT = 505;
    /** 响应-新增评论*/
    public static final short RSP_CREATECOMMENT = 506;
    /** 响应-查看账单*/
    public static final short RSP_QUERYBILL = 507;
    /** 响应- 确认收货*/
    public static final short RSP_CONFIRM = 508;
    /** 响应- 查看商品列表*/
    public static final short RSP_GETGOODSLIST = 509;


    /** 失败标识 */
    public static final short FAIL = 0;
    /** 成功标识 */
    public static final short SUCC = 1;

    /**删除订单标识*/
    public static final short DELETEORDER=101;
    /**修改订单信息标识*/
    public static final short UPDATEORDER=102;

    /**全部订单标识*/
    public static final short ALL=-1;
    /**代理收入*/
    public static final short IN=0;
    /**消费支出*/
    public static final short OUT=1;
    /**提现*/
    public static final short CASH=2;


}
