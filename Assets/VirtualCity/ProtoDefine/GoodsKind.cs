using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]




public class GoodsKind {


[ProtoMember(1)]
    public long? id;

    /**
     商品id
     */
[ProtoMember(2)]
    public long goodsId;

    /**
     款式价格
     */
[ProtoMember(3)]
    public double value;

    /**
     款式图片地址
     */
[ProtoMember(4)]
    public string kindPicture;

    /**
     款式名称
     */
[ProtoMember(5)]
    public string name;

    /**
     数量
     */
[ProtoMember(6)]
    public int number;

[ProtoMember(7)]
    public string createtime;

[ProtoMember(8)]
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

    public double getValue() {
        return value;
    }

    public void setValue(double value) {
        this.value = value;
    }

    public string getKindPicture() {
        return kindPicture;
    }

    public void setKindPicture(string kindPicture) {
        this.kindPicture = kindPicture;
    }

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
    }

    public int getNumber() {
        return number;
    }

    public void setNumber(int number) {
        this.number = number;
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