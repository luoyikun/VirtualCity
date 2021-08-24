using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRewardHousePartMessage {
[ProtoMember(1)]
    public long accountId;
[ProtoMember(2)]
    public long goodsKindId;
[ProtoMember(3)]
    public int number;

    public ReqRewardHousePartMessage() {
    }

    public ReqRewardHousePartMessage(long accountId, long goodsKindId, int number) {
        this.accountId = accountId;
        this.goodsKindId = goodsKindId;
        this.number = number;
    }

    public long getGoodsKindId() {
        return goodsKindId;
    }

    public void setGoodsKindId(long goodsKindId) {
        this.goodsKindId = goodsKindId;
    }

    public int getNumber() {
        return number;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }
}
}