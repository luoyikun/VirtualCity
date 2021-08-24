using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetGoodsMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;

    /**商品List*/
[ProtoMember(3)]
    public List<Goods> goodsList;

    public RspGetGoodsMessage() {
    }

    public RspGetGoodsMessage(int code, string tip, List<Goods> goodsList) {
        this.code = code;
        this.tip = tip;
        this.goodsList = goodsList;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }

    public List<Goods> getGoodsList() {
        return goodsList;
    }

    public void setGoodsList(List<Goods> goodsList) {
        this.goodsList = goodsList;
    }
}
}