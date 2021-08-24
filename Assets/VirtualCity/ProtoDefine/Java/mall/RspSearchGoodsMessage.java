package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.mall.Goods;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_SEARCH_GOODS)
public class RspSearchGoodsMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<Goods> goodsList;

    public RspSearchGoodsMessage(List<Goods> goodsList) {
        this.goodsList = goodsList;
    }

    public RspSearchGoodsMessage() {
    }

    public List<Goods> getGoodsList() {
        return goodsList;
    }

    public void setGoodsList(List<Goods> goodsList) {
        this.goodsList = goodsList;
    }
}
