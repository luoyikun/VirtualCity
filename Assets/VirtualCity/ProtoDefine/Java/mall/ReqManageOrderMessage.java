package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.mall.Order;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_MANAGEORDER)
public class ReqManageOrderMessage extends Message {
    /**订单列表*/
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<Order> orders;
    /**修改订单信息*/
    @Protobuf(order = 2)
    private int manage ;

    public List<Order> getOrders() {
        return orders;
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
    }

    public int getManage() {
        return manage;
    }

    public void setManage(int manage) {
        this.manage = manage;
    }
}
