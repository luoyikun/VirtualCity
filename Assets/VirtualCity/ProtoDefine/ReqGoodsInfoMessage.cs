using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGoodsInfoMessage {
    /**商品id*/
[ProtoMember(1)]
    public long? goodsId;

    public ReqGoodsInfoMessage() {
    }

    public ReqGoodsInfoMessage(long? goodsId) {
        this.goodsId = goodsId;
    }

    public long? getGoodsId() {
        return goodsId;
    }

    public void setGoodsId(long? goodsId) {
        this.goodsId = goodsId;
    }

}
}