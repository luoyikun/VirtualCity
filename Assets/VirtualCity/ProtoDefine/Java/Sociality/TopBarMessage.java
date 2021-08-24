package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.TOP_BAR_MESSAGE)
public class TopBarMessage extends Message {
    @Protobuf(order = 1)
    private String message;

    public TopBarMessage() {
    }

    public TopBarMessage(String message) {
        this.message = message;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }
}
