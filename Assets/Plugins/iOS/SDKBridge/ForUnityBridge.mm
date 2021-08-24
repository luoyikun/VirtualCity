//unity与ios sdk(微信 支付宝..)桥接的脚本

#import <Foundation/Foundation.h>
#import <AlipaySDK/AlipaySDK.h>
#import "ForUnityBridge.h"
#import "WXApi.h"
#import "WXApiObject.h"
//#import "SugramApiManager.h"
//#import "SugramApiObject.h“


extern "C"
{
	//创建字符串
    NSString* _CreateNSString (const char* string)
    {
        if (string)
            return [NSString stringWithUTF8String: string];
        else
            return [NSString stringWithUTF8String: ""];
    }
    
	///微信登录
    void IOS_WeChatLogon(const char* state)
    {
        if ([WXApi isWXAppInstalled]) {
            SendAuthReq *req = [[SendAuthReq alloc] init];
            req.scope = @"snsapi_userinfo";
            req.state = _CreateNSString(state);
            
            [WXApi sendReq:req];
        }
    }
    
	///是否安装了微信
    bool IOS_IsInstallWechat()
    {
        return [WXApi isWXAppInstalled];
    }
    
	///是否支持API的版本
    bool IOS_IsWeChatSupportApi() {
        return [WXApi isWXAppSupportApi];
    }
	
    ///注册APP
    void IOS_RegisteredWeChat(const char* appId){
        [WXApi registerApp:_CreateNSString(appId) enableMTA:YES];
    }
	
	
    ///分享图片的接口
    void IOS_WeChatShareImage(int scene, Byte*  ptr, int size, Byte* ptrThumb, int sizeThumb){
        WXMediaMessage *message = [WXMediaMessage message];
        
        NSData *data = [[NSData alloc] initWithBytes:ptr length:size];
        NSData *dataThumb = [[NSData alloc] initWithBytes:ptrThumb length:sizeThumb];
        
        [message setThumbImage:[UIImage imageWithData:dataThumb scale:1]];
        
        WXImageObject *ext = [WXImageObject object];
        ext.imageData = data;
        
        UIImage* image = [UIImage imageWithData:ext.imageData];
        ext.imageData = UIImagePNGRepresentation(image);
        message.mediaObject = ext;
        
        SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
        req.bText = NO;
        req.message = message;
        req.scene = scene;
        
        [WXApi sendReq:req];
    }
    
	///分享文本的接口
    void IOS_WeChatShareText(int scene, char * content){
        SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
        req.text = _CreateNSString(content);
        req.bText = YES;
        req.scene = scene;
        
        [WXApi sendReq:req];
    }
    
	///分享链接的接口
    void IOS_WeChatShareWebPage(int scene, char* url, char* title, char* content, Byte* ptrThumb, int sizeThumb){
        
        WXMediaMessage *message = [WXMediaMessage message];
        message.title = _CreateNSString(title);
        message.description = _CreateNSString(content);
        NSData *data = [[NSData alloc] initWithBytes:ptrThumb length:sizeThumb];
        [message setThumbImage:[UIImage imageWithData:data scale:1]];
        
        WXWebpageObject *ext = [WXWebpageObject object];
        ext.webpageUrl = _CreateNSString(url);
        
        message.mediaObject = ext;
        
        SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
        req.bText = NO;
        req.message = message;
        req.scene = scene;
        
        [WXApi sendReq:req];
    }
	
	///支付API
	void IOS_WechatPay(char* appId, char* partnerId, char* prepayId, char* nonceStr, int timeStamp, char* packageValue, char* sign)
	{
		PayReq *request = [[PayReq alloc]init];
		//request.appId = appId;
        request.partnerId = _CreateNSString(partnerId);
        request.prepayId = _CreateNSString(prepayId);
        request.package = _CreateNSString(packageValue);
        request.nonceStr = _CreateNSString(nonceStr);
        request.timeStamp = timeStamp;
        request.sign = _CreateNSString(sign);
        
        [WXApi sendReq:request];
	}
	
	
	///阿里支付 IOS接口
    void IOS_AliPay(char* orderInfo)
    {
        [[AlipaySDK defaultService] payOrder:_CreateNSString(orderInfo) fromScheme:@"virtualcityalipay" callback:^(NSDictionary *resultDic) {
            NSLog(@"阿里支付结果来了   reslut = %@",resultDic);
            if([resultDic[@"resultStatus"]  isEqual: @"9000"])
            {
                UnitySendMessage("SDKManager", "AliPayCallback", "true");
            }
            else
            {
                UnitySendMessage("SDKManager", "AliPayCallback", "false");
            }
        }];
    }

	//复制文本到剪贴板
    void IOS_CopyTextToClipboard(const char *textList)
    {
		UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
		pasteboard.string = _CreateNSString(textList);
    }
	
    ///获取电池
    float IOS_GetBattery()
    {
        [[UIDevice currentDevice] setBatteryMonitoringEnabled:YES];
        return [[UIDevice currentDevice] batteryLevel];
    }
	
	
	
}


//implementation是一个编译器制定，表明你将为某个类提供代码。
//类名出现在@implemention之后，该行的结尾处没有分号。
//比较旧版的回调如下进行监听 但现在新的监听是以URL的形式进行监听的
@implementation ForUnityBridge

+(instancetype)forUnityBridgeInstance {
    static dispatch_once_t onceToken;
    static ForUnityBridge *instance;
    dispatch_once(&onceToken, ^{
        instance = [[ForUnityBridge alloc] init];
    });
    return instance;
}


-(void) onReq:(BaseReq *)req{}

/*! 微信回调，不管是登录还是分享成功与否，都是走这个方法 @brief 发送一个sendReq后，收到微信的回应
 
 *
 * 收到一个来自微信的处理结果。调用一次sendReq后会收到onResp。
 * 可能收到的处理结果有SendMessageToWXResp、SendAuthResp等。
 * @param resp 具体的回应内容，是自动释放的;
 */
-(void) onResp:(BaseResp*)resp{
    NSLog(@"微信支付结果来了 resp %d",resp.errCode);
    /*
     enum  WXErrCode {
     WXSuccess           = 0,    成功
     WXErrCodeCommon     = -1,  普通错误类型
     WXErrCodeUserCancel = -2,    用户点击取消并返回
     WXErrCodeSentFail   = -3,   发送失败
     WXErrCodeAuthDeny   = -4,    授权失败
     WXErrCodeUnsupport  = -5,   微信不支持
     };
     */
    if ([resp isKindOfClass:[SendAuthResp class]]) {   //授权登录的类。
        if (resp.errCode == 0) {
            UnitySendMessage("SDKManager", "LoginCallBack",[((SendAuthResp *)resp).code UTF8String]);
        }
        
    }else if ([resp isKindOfClass:[SendMessageToWXResp class]]) {
        UnitySendMessage("SDKManager", "WechatCallBack",[[NSString stringWithFormat:@"%d",((SendMessageToWXResp*)resp).errCode] UTF8String]);
    }else if ([resp isKindOfClass:[PayResp class]]){ 
		UnitySendMessage("SDKManager", "WechatPayCallback",[[NSString stringWithFormat:@"%d",((PayResp*)resp).errCode] UTF8String]);
    }
}

@end
