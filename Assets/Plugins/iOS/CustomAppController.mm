#import "UnityAppController.h"
#import "WXApi.h"
#import <AlipaySDK/AlipaySDK.h>

#define ksendAuthRequestNotification @"ksendAuthRequestNotification"

@interface CustomAppController : UnityAppController < WXApiDelegate >
@end
 
IMPL_APP_CONTROLLER_SUBCLASS (CustomAppController)
 
@implementation CustomAppController
 

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(sendAuthRequest) name:ksendAuthRequestNotification object:nil]; // 微信
    
    //向微信注册
    [WXApi registerApp:@"wx09f530f1e33f2cb0" ];
    
    return [super application:application didFinishLaunchingWithOptions:launchOptions];

}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options{
    NSLog(@"denglude.... = %@",url.host);
    
    //分享
    if ([url.host isEqualToString:@"platformId=wechat"]) {
        return [WXApi handleOpenURL:url delegate:self];
    }
    
    //登陆
    if ([url.host isEqualToString:@"oauth"]) {
        return [WXApi handleOpenURL:url delegate:self];
    }
    
    //微信支付回调
    if ([url.host isEqualToString:@"pay"]) {
        return [WXApi handleOpenURL:url delegate:self];
    }
    //支付宝支付回调
    if ([url.host isEqualToString:@"safepay"]) {
        //跳转支付宝钱包进行支付，处理支付结果
        [[AlipaySDK defaultService] processOrderWithPaymentResult:url standbyCallback:^(NSDictionary *resultDic) {
            NSLog(@"result = %@",resultDic);
            NSString *resultStatus = [resultDic objectForKey:@"resultStatus"];
            NSString *memo;
            if ([resultStatus intValue] == 9000) {
                memo = @"支付成功!";
                UnitySendMessage("AliComponent", "AliPayCallback", "true");
            }else {
                
                switch ([resultStatus intValue]) {
                    case 4000:
                        UnitySendMessage("AliComponent", "AliPayCallback", "flase");
                        memo = @"失败原因:订单支付失败!";
                        break;
                    case 6001:
                        NSLog(@"失败原因:用户中途取消!");
                        UnitySendMessage("AliComponent", "AliPayCallback", "flase");
                        memo = @"失败原因:用户中途取消!";
                        break;
                    case 6002:
                        UnitySendMessage("AliComponent", "AliPayCallback", "flase");
                        memo = @"失败原因:网络连接出错!";
                        break;
                    case 8000:
                        UnitySendMessage("AliComponent", "AliPayCallback", "flase");
                        memo = @"正在处理中...";
                        break;
                    default:
                        UnitySendMessage("AliComponent", "AliPayCallback", "flase");
                        memo = [resultDic objectForKey:@"memo"];
                        break;
                }
                
            }
            
        }];
    }
    return YES;
}




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
            UnitySendMessage("WeChatComponent", "LogonCallback",[((SendAuthResp *)resp).code UTF8String]);
        }
        
    }else if ([resp isKindOfClass:[SendMessageToWXResp class]]) {
        UnitySendMessage("WeChatComponent", "ShareImage_Callback",[[NSString stringWithFormat:@"%d",((SendMessageToWXResp*)resp).errCode] UTF8String]);
    }else if ([resp isKindOfClass:[PayResp class]]){
        UnitySendMessage("WeChatComponent", "WechatPayCallback",[[NSString stringWithFormat:@"%d",((PayResp*)resp).errCode] UTF8String]);
    }
}

@end
