package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_GETGOODS)
public class ReqGetGoodsMessage extends Message {
    /**商品id*/
    @Protobuf(order = 1)
    private List<Long> goodsId;

    public ReqGetGoodsMessage() {
    }

    public List<Long> getGoodsId() {
        return goodsId;
    }

    public void setGoodsId(List<Long> goodsId) {
        this.goodsId = goodsId;
    }
}