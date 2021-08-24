using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspQueryOrderMessage {

[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
    /**订单对象*/
[ProtoMember(3)]
    public List<Order> orders;

    public RspQueryOrderMessage(int code, string tip, List<Order> orders) {
        this.code = code;
        this.tip = tip;
        this.orders = orders;
    }

    public RspQueryOrderMessage() {
    }

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

    public List<Order> getOrders() {
        return orders;
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
    }
}

}