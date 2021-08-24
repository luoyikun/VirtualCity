package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.mall.GoodsKind;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_GETGOODSKINDINFO)
public class RspGoodsKindInfoMessage extends Message {
    //商品类别列表
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<GoodsKind> listGoodsKids;

    public RspGoodsKindInfoMessage() {
    }

    public List<GoodsKind> getListGoodsKids() {
        return listGoodsKids;
    }

    public void setListGoodsKids(List<GoodsKind> listGoodsKids) {
        this.listGoodsKids = listGoodsKids;
    }

    public RspGoodsKindInfoMessage(List<GoodsKind> listGoodsKids) {
        this.listGoodsKids = listGoodsKids;
    }
}
