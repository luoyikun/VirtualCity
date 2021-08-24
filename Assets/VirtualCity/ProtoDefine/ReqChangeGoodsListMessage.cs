using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqChangeGoodsListMessage {
[ProtoMember(1)]
    public List<long?> listGoodsId;
[ProtoMember(2)]
    public int handleFlag;

    public List<long?> getListGoodsId() {
        return listGoodsId;
    }

    public void setListGoodsId(List<long?> listGoodsId) {
        this.listGoodsId = listGoodsId;
    }

    public int getHandleFlag() {
        return handleFlag;
    }

    public void setHandleFlag(int handleFlag) {
        this.handleFlag = handleFlag;
    }
}
}