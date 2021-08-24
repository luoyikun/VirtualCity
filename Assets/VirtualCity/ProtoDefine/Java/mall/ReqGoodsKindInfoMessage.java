package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_GETGOODSKINDINFO)
public class ReqGoodsKindInfoMessage extends Message {
    //商品类别ID
    @Protobuf(order = 1,fieldType = FieldType.INT64)
    private List<Long> listGoodsKindId;

    public List<Long> getListGoodsKindId() {
        return listGoodsKindId;
    }

    public void setListGoodsKindId(List<Long> listGoodsKindId) {
        this.listGoodsKindId = listGoodsKindId;
    }
}

