using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspQueryCommentsMessage {

[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
    /**订单对象*/
[ProtoMember(3)]
    public List<Comment> comments;


    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }

    public List<Comment> getComments() {
        return comments;
    }

    public void setComments(List<Comment> comments) {
        this.comments = comments;
    }
}

}