using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqManageOrderMessage {
    /**订单列表*/
[ProtoMember(1)]
    public List<Order> orders;
    /**修改订单信息*/
[ProtoMember(2)]
    public int manage ;

    public List<Order> getOrders() {
        return orders;
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
    }

    public int getManage() {
        return manage;
    }

    public void setManage(int manage) {
        this.manage = manage;
    }
}
}