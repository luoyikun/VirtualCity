using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspSearchGoodsMessage {
[ProtoMember(1)]
    public List<Goods> goodsList;

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
}