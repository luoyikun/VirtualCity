package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.bill.AccountBill;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_QUERYBILL)
public class RspQueryBillMessage extends Message {

    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;
    /**账单对象*/
    @Protobuf(order = 3,fieldType = FieldType.OBJECT)
    private List<AccountBill> bills;

    public RspQueryBillMessage() {
    }

    public RspQueryBillMessage(int code, String tip, List<AccountBill> bills) {
        this.code = code;
        this.tip = tip;
        this.bills = bills;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
        this.tip = tip;
    }

    public List<AccountBill> getBills() {
        return bills;
    }

    public void setBills(List<AccountBill> bills) {
        this.bills = bills;
    }
}

