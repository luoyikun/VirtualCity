package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_CREATEORDER)
public class RspCreateOrderMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tips;
    /**订单支付信息*/
    @Protobuf(order = 3)
    private String payInfo;
    @Protobuf(order = 4)
    private double sMoney;
    @Protobuf(order = 5)
    private double money;

    public RspCreateOrderMessage() {
    }

    public RspCreateOrderMessage(int code, String tips) {
        this.code = code;
        this.tips = tips;
    }

    public RspCreateOrderMessage(int code, String tips, String payInfo, double sMoney, double money) {
        this.code = code;
        this.tips = tips;
        this.payInfo = payInfo;
        this.sMoney = sMoney;
        this.money = money;
    }

    public double getsMoney() {
        return sMoney;
    }

    public void setsMoney(double sMoney) {
        this.sMoney = sMoney;
    }

    public double getMoney() {
        return money;
    }

    public void setMoney(double money) {
        this.money = money;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTips() {
        return tips;
    }

    public void setTips(String tips) {
        this.tips = tips;
    }

    public String getPayInfo() {
        return payInfo;
    }

    public void setPayInfo(String payInfo) {
        this.payInfo = payInfo;
    }
}
