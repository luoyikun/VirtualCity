package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_COMMANDUI)
public class RspCommandUIMessage extends Message {
    @Protobuf(order = 1)
    private String ui;

    public String getUi() {
        return ui;
    }

    public void setUi(String ui) {
        this.ui = ui;
    }

    public RspCommandUIMessage(String ui) {
        this.ui = ui;
    }

    public RspCommandUIMessage() {
    }
}
