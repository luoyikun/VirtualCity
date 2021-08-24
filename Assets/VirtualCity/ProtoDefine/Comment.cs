using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class Comment {

[ProtoMember(1)]
    public long? id;

    /**
     商品id
     */
[ProtoMember(2)]
    public long goodsId;

    /**
     商品评价内容
     */
[ProtoMember(3)]
    public string text;

    /**
     评价星级
     */
[ProtoMember(4)]
    public int star;

    /**
     评价账户id
     */
[ProtoMember(5)]
    public long accountId;

    /**
     评论用户昵称
     */
[ProtoMember(6)]
    public string accountName;

    /**
     评论用户头像
     */
[ProtoMember(7)]
    public long moudleId;

    /**
     商品种类
     */
[ProtoMember(8)]
    public long goodsKindId;

    /**
     确认收货时间
     */
[ProtoMember(9)]
    public string confirmTime;

[ProtoMember(10)]
    public string createtime;

[ProtoMember(11)]
    public string updatetime;

    public long? getId() {
        return id;
    }

    public void setId(long? id) {
        this.id = id;
    }

    public long getGoodsId() {
        return goodsId;
    }

    public void setGoodsId(long goodsId) {
        this.goodsId = goodsId;
    }

    public string getText() {
        return text;
    }

    public void setText(string text) {
        this.text = text;
    }

    public int getStar() {
        return star;
    }

    public void setStar(int star) {
        this.star = star;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public string getAccountName() {
        return accountName;
    }

    public void setAccountName(string accountName) {
        this.accountName = accountName;
    }

    public long getMoudleId() {
        return moudleId;
    }

    public void setMoudleId(long moudleId) {
        this.moudleId = moudleId;
    }

    public long getGoodsKindId() {
        return goodsKindId;
    }

    public void setGoodsKindId(long goodsKindId) {
        this.goodsKindId = goodsKindId;
    }

    public string getConfirmTime() {
        return confirmTime;
    }

    public void setConfirmTime(string confirmTime) {
        this.confirmTime = confirmTime;
    }

    public string getCreatetime() {
        return createtime;
    }

    public void setCreatetime(string createtime) {
        this.createtime = createtime;
    }

    public string getUpdatetime() {
        return updatetime;
    }

    public void setUpdatetime(string updatetime) {
        this.updatetime = updatetime;
    }

}


}