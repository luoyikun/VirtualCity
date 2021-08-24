//
//  ForUnityBridge.h
//  Unity-iPhone
//
//  Created by imac on 2017/12/13.
//
//
#import <Foundation/Foundation.h>
#import "WXApi.h"

//#import "SugramApiManager.h"

@protocol WXDelegate <NSObject>

@end

@interface ForUnityBridge : UIResponder <UIApplicationDelegate,WXApiDelegate>
@property (strong,nonatomic) UIWindow *window;
+(instancetype)forUnityBridgeInstance;

@property (nonatomic,weak) id<WXDelegate> wxDelegate;

@end

