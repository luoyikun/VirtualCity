using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqGoodsKindInfoMessage {
    //商品类别ID
[ProtoMember(1)]
    public List<long?> listGoodsKindId;

    public List<long?> getListGoodsKindId() {
        return listGoodsKindId;
    }

    public void setListGoodsKindId(List<long?> listGoodsKindId) {
        this.listGoodsKindId = listGoodsKindId;
    }
}

}