using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqGetGoodsMessage {
    /**商品id*/
[ProtoMember(1)]
    public List<long?> goodsId;

    public ReqGetGoodsMessage() {
    }

    public List<long?> getGoodsId() {
        return goodsId;
    }

    public void setGoodsId(List<long?> goodsId) {
        this.goodsId = goodsId;
    }
}
}