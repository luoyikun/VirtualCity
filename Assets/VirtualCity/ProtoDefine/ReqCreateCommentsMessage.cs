using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqCreateCommentsMessage {
    /**评论信息*/
[ProtoMember(1)]
    public Comment comment;
[ProtoMember(2)]
    public string orderNo;

    public string getOrderNo() {
        return orderNo;
    }

    public void setOrderNo(string orderNo) {
        this.orderNo = orderNo;
    }

    public Comment getComment() {
        return comment;
    }

    public void setComment(Comment comment) {
        this.comment = comment;
    }
}
}