package com.lcqp.lcqp.wxapi;

import com.tencent.mm.opensdk.modelbase.BaseReq;
import com.tencent.mm.opensdk.modelbase.BaseResp;
import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.IWXAPIEventHandler;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;

public class WXEntryActivity extends Activity implements IWXAPIEventHandler{
	public static WechatInterface mWechatResponse;
	
	//΢�Żص�ҳ���ʼ��
	public void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		//�����ǵ�ǰ��ҳ��ע��Ϊ΢�Żص�ҳ��
		MyApplication.WApi.handleIntent(getIntent(), this);
	}
	
	//��ʼ��΢��
	public static IWXAPI Init(Context text, String App_id)
	{
		IWXAPI sApi = WXAPIFactory.createWXAPI(text, App_id,true);
		sApi.registerApp(App_id);
		return sApi;
	}
	//΢�ŵ�¼
	public static void WechatLogin(WechatInterface interfaces)
	{
		UnityPlayer.UnitySendMessage("SDKMessage", "OpenCommonBox", "����WXEntryActivity�е�WechatLogin11111");
	    // ����һ��΢������
	    SendAuth.Req req = new SendAuth.Req();
	    // �����������:��¼����ȡ�û���Ϣ��΢��id,΢���ǳ�,΢��ͷ��,�Ա𡣡�������
	    req.scope = "snsapi_userinfo";
	    //������������ͷ��ص���ȷ��״̬�룬�ٷ���ʱ��ԭ������
	    req.state = "wechat_sdk_demo_test_1_12";
	    //��������
	    MyApplication.WApi.sendReq(req);
	    mWechatResponse = interfaces;
	    UnityPlayer.UnitySendMessage("SDKMessage", "OpenCommonBox", "����WXEntryActivity�е�WechatLogin2222");
	}

	@Override
	public void onReq(BaseReq arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onResp(BaseResp arg0) {
		// TODO Auto-generated method stub
		switch(arg0.errCode)
		{
		//���ص�¼�ɹ�
		case BaseResp.ErrCode.ERR_OK:
			if(mWechatResponse!=null){
				//��ȡ΢�ŵ�¼����ʱ��Ȩ��
				String code = ((SendAuth.Resp)arg0).code;
				System.out.println(code);
				mWechatResponse.getResponse(code);
				mWechatResponse = null;
			}
			break;
		}
		finish();
	}
	//΢������ɹ��Ľӿ�
	public interface WechatInterface{
		void getResponse(String Code);
	}
}
