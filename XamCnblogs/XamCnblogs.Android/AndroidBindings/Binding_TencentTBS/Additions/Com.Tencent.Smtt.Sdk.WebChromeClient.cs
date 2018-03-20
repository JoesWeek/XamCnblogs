using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Com.Tencent.Smtt.Sdk
{
    public partial class WebChromeClient : global::Java.Lang.Object
    {
        static Delegate cb_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I;
 
        static Delegate GetOnProgressChanged_Lcom_tencent_smtt_sdk_WebView_IHandler()
        {
            if (cb_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I == null)
                cb_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr, int>)n_OnProgressChanged_Lcom_tencent_smtt_sdk_WebView_I);
            return cb_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I;
        }
         
        static IntPtr id_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.tencent.smtt.sdk']/class[@name='WebChromeClient']/method[@name='onProgressChanged' and count(parameter)=2 and parameter[1][@type='com.tencent.smtt.sdk.WebView'] and parameter[2][@type='int']]"
        [Register("onProgressChanged", "(Lcom/tencent/smtt/sdk/WebView;I)V", "GetOnProgressChanged_Lcom_tencent_smtt_sdk_WebView_IHandler")]
        public virtual unsafe void OnProgressChanged(global::Com.Tencent.Smtt.Sdk.WebView p0, int p1)
        {
            if (id_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I == IntPtr.Zero)
                id_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I = JNIEnv.GetMethodID(ThresholdClass, "onProgressChanged", "(Lcom/tencent/smtt/sdk/WebView;I)V");
            try
            {
                JValue* __args = stackalloc JValue[2];
                __args[0] = new JValue(p0);
                __args[1] = new JValue(p1);

                if (((object)this).GetType() == ThresholdType)
                    JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, id_onProgressChanged_Lcom_tencent_smtt_sdk_WebView_I, __args);
                else
                    JNIEnv.CallNonvirtualVoidMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "onProgressChanged", "(Lcom/tencent/smtt/sdk/WebView;I)V"), __args);
            }
            finally
            {
            }
        }
         

        static void n_OnProgressChanged_Lcom_tencent_smtt_sdk_WebView_I(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, int p1)
        {
            try
            {
                global::Com.Tencent.Smtt.Sdk.WebChromeClient __this = global::Java.Lang.Object.GetObject<global::Com.Tencent.Smtt.Sdk.WebChromeClient>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
                global::Com.Tencent.Smtt.Sdk.WebView p0 = global::Java.Lang.Object.GetObject<global::Com.Tencent.Smtt.Sdk.WebView>(native_p0, JniHandleOwnership.DoNotTransfer);
                __this.OnProgressChanged(p0, p1);
            }
            catch
            {

            }

        }

    }
}