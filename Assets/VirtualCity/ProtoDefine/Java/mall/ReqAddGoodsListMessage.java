package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_ADDGOODSIST)
public class ReqAddGoodsListMessage extends Message {
    //购物车信息
    @Protobuf(order = 1)
    private String  goodsListStr;

    public String getGoodsListStr() {
        return goodsListStr;
    }

    public void setGoodsListStr(String goodsListStr) {
        this.goodsListStr = goodsListStr;
    }
}
