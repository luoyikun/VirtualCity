using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGoodsInfoMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;
[ProtoMember(3)]
    public List<Comment> comments;
[ProtoMember(4)]
    public List<GoodsKind>goodsKinds;
[ProtoMember(5)]
    public Goods goods;

    public RspGoodsInfoMessage() {
    }

    public RspGoodsInfoMessage(int code, string tips, List<Comment> comments, List<GoodsKind> goodsKinds, Goods goods) {
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

    public string getTips() {
        return tips;
    }

    public void setTips(string tips) {
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

}