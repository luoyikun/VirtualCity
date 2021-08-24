using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqAddGoodsListMessage {
    //购物车信息
[ProtoMember(1)]
    public string  goodsListStr;

    public string getGoodsListStr() {
        return goodsListStr;
    }

    public void setGoodsListStr(string goodsListStr) {
        this.goodsListStr = goodsListStr;
    }
}
}