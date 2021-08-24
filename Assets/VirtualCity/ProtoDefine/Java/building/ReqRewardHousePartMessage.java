package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_REWARDHOUSEPART)
public class ReqRewardHousePartMessage extends Message {
    @Protobuf(order = 1)
    private long accountId;
    @Protobuf(order = 2)
    private long goodsKindId;
    @Protobuf(order = 3)
    private int number;

    public ReqRewardHousePartMessage() {
    }

    public ReqRewardHousePartMessage(long accountId, long goodsKindId, int number) {
        this.accountId = accountId;
        this.goodsKindId = goodsKindId;
        this.number = number;
    }

    public long getGoodsKindId() {
        return goodsKindId;
    }

    public void setGoodsKindId(long goodsKindId) {
        this.goodsKindId = goodsKindId;
    }

    public int getNumber() {
        return number;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }
}
