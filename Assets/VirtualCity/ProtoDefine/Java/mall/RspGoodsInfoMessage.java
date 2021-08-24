package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.mall.Comment;
import com.kingston.jforgame.server.game.entity.mall.Goods;
import com.kingston.jforgame.server.game.entity.mall.GoodsKind;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_GOODSINFO)
public class RspGoodsInfoMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tips;
    @Protobuf(order = 3,fieldType = FieldType.OBJECT)
    private List<Comment> comments;
    @Protobuf(order = 4,fieldType = FieldType.OBJECT)
    private List<GoodsKind>goodsKinds;
    @Protobuf(order = 5,fieldType = FieldType.OBJECT)
    private Goods goods;

    public RspGoodsInfoMessage() {
    }

    public RspGoodsInfoMessage(int code, String tips, List<Comment> comments, List<GoodsKind> goodsKinds, Goods goods) {
        this.code = code;
        this.tips = tips;
        this.comments = comments;
        this.goodsKinds = goodsKinds;
        this.goods = goods;
    }

    public Goods getGoods() {
        return goods;
    }

    public void setGoods(Goods goods) {
        this.goods = goods;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTips() {
        return tips;
    }

    public void setTips(String tips) {
        this.tips = tips;
    }


    public List<Comment> getComments() {
        return comments;
    }

    public void setComments(List<Comment> comments) {
        this.comments = comments;
    }

    public List<GoodsKind> getGoodsKinds() {
        return goodsKinds;
    }

    public void setGoodsKinds(List<GoodsKind> goodsKinds) {
        this.goodsKinds = goodsKinds;
    }
}

