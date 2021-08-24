package com.kingston.jforgame.server.game.system.entity;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

public class PayAccount {
    @Protobuf(order = 1)
    private String account;
    @Protobuf(order = 2)
    private String realName;
    @Protobuf(order = 3)
    private int payType;
    @Protobuf(order = 4)
    private int isDefualt;

    public PayAccount() {
    }

    public PayAccount(String account, String realName, int payType, int isDefualt) {
        this.account = account;
        this.realName = realName;
        this.payType = payType;
        this.isDefualt = isDefualt;
    }

    public String getAccount() {
        return account;
    }

    public void setAccount(String account) {
        this.account = account;
    }

    public String getRealName() {
        return realName;
    }

    public void setRealName(String realName) {
        this.realName = realName;
    }

    public int getPayType() {
        return payType;
    }

    public void setPayType(int payType) {
        this.payType = payType;
    }
}
