package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.mall.Order;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_QUERYORDER)
public class RspQueryOrderMessage extends Message {

    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;
    /**订单对象*/
    @Protobuf(order = 3,fieldType = FieldType.OBJECT)
    private List<Order> orders;

    public RspQueryOrderMessage(int code, String tip, List<Order> orders) {
        this.code = code;
        this.tip = tip;
        this.orders = orders;
    }

    public RspQueryOrderMessage() {
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

    public List<Order> getOrders() {
        return orders;
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
    }
}

