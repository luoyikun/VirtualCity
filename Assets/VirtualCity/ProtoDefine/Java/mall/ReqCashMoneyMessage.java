package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.server.game.system.entity.PayAccount;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_CASHMONEY)
public class ReqCashMoneyMessage extends Message {
    //金额
    @Protobuf(order = 1,fieldType = FieldType.FLOAT)
    private float cashMoney;
    //微信：1
    //支付宝：0
    @Protobuf(order = 2)
    private int payType;
    //账号信息
    @Protobuf(order = 3)
    private String payAccount;
    //提现密码
    @Protobuf(order = 4)
    private String password;
    @Protobuf(order = 5)
    private byte[] content;

    public byte[] getContent() {
        return content;
    }

    public void setContent(byte[] content) {
        this.content = content;
    }

    public float getCashMoney() {
        return cashMoney;
    }

    public void setCashMoney(float cashMoney) {
        this.cashMoney = cashMoney;
    }

    public int getPayType() {
        return payType;
    }

    public void setPayType(int payType) {
        this.payType = payType;
    }

    public String getPayAccount() {
        return payAccount;
    }

    public void setPayAccount(String payAccount) {
        this.payAccount = payAccount;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
