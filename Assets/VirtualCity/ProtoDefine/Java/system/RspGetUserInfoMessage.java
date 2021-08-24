package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.master.Account;
import com.kingston.jforgame.server.game.login.entity.Player;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.Map;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_GETUSERINFO)
public class RspGetUserInfoMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.MAP)
    private Map<Long, Player> players;

    public RspGetUserInfoMessage() {
    }

    public RspGetUserInfoMessage(Map<Long, Player> players) {
        this.players = players;
    }

    public Map<Long, Player> getPlayers() {
        return players;
    }

    public void setPlayers(Map<Long, Player> players) {
        this.players = players;
    }
}
