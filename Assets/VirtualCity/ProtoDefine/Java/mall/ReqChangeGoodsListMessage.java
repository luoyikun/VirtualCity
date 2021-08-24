package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_CHANGEGOODSIST)
public class ReqChangeGoodsListMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.INT64)
    private List<Long> listGoodsId;
    @Protobuf(order = 2)
    private int handleFlag;

    public List<Long> getListGoodsId() {
        return listGoodsId;
    }

    public void setListGoodsId(List<Long> listGoodsId) {
        this.listGoodsId = listGoodsId;
    }

    public int getHandleFlag() {
        return handleFlag;
    }

    public void setHandleFlag(int handleFlag) {
        this.handleFlag = handleFlag;
    }
}
