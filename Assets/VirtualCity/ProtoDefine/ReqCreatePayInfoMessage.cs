using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqCreatePayInfoMessage {
[ProtoMember(1)]
    public string orderNo;

    public string getOrderNo() {
        return orderNo;
    }

    public void setOrderNo(string orderNo) {
        this.orderNo = orderNo;
    }
}
}