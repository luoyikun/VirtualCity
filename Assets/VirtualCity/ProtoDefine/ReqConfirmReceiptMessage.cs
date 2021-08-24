using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqConfirmReceiptMessage {
    /**评论信息*/
[ProtoMember(1)]
    public Order order;

    public ReqConfirmReceiptMessage() {
    }

    public ReqConfirmReceiptMessage(Order order) {
        this.order = order;
    }

    public Order getOrder() {
        return order;
    }

    public void setOrder(Order order) {
        this.order = order;
    }
}
}