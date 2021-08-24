using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqSearchGoodsMessage {
[ProtoMember(1)]
    public string goodsName;

    public string getGoodsName() {
        return goodsName;
    }

    public void setGoodsName(string goodsName) {
        this.goodsName = goodsName;
    }
}
}