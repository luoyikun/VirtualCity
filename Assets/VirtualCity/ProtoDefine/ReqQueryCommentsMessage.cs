using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqQueryCommentsMessage {
    /**星数*/
[ProtoMember(1)]
    public int star;
    /**最后一条记录的时间*/
[ProtoMember(2)]
    public string lastDate;
    /**商品Id*/
[ProtoMember(3)]
    public long goodsId;

    public int getStar() {
        return star;
    }

    public void setStar(int star) {
        this.star = star;
    }

    public string getLastDate() {
        return lastDate;
    }

    public void setLastDate(string lastDate) {
        this.lastDate = lastDate;
    }

    public long getGoodsId() {
        return goodsId;
    }

    public void setGoodsId(long goodsId) {
        this.goodsId = goodsId;
    }
}
}