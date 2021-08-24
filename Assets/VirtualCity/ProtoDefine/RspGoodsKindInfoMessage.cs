using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGoodsKindInfoMessage {
    //商品类别列表
[ProtoMember(1)]
    public List<GoodsKind> listGoodsKids;

    public RspGoodsKindInfoMessage() {
    }

    public List<GoodsKind> getListGoodsKids() {
        return listGoodsKids;
    }

    public void setListGoodsKids(List<GoodsKind> listGoodsKids) {
        this.listGoodsKids = listGoodsKids;
    }

    public RspGoodsKindInfoMessage(List<GoodsKind> listGoodsKids) {
        this.listGoodsKids = listGoodsKids;
    }
}
}