using EasyHook;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeneiAO.Model;


namespace AORNet
{
    public class Main : IEntryPoint
    {
        private LocalHook HandleHook;
        private LocalHook SendHook;

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void THandleData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void TSendData([MarshalAs(UnmanagedType.BStr)] ref string data);

        public static readonly THandleData PHandleData = (THandleData)Marshal.GetDelegateForFunctionPointer(new IntPtr(0x655B10), typeof(THandleData));
        public static readonly TSendData PSendData = (TSendData)Marshal.GetDelegateForFunctionPointer(new IntPtr(0x6A21C0), typeof(TSendData));

        static string ChannelName;
        RemoteService Interface;

        public Main(RemoteHooking.IContext InContext, String InChannelName)
        {
            try
            {
                Interface = RemoteHooking.IpcConnectClient<RemoteService>(InChannelName);
                ChannelName = InChannelName;
                Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }
        }

        public unsafe void Run(RemoteHooking.IContext InContext, String InChannelName)
        {
            try
            {
                HandleHook = LocalHook.Create(new IntPtr(0x655B10), new THandleData(HKHandleData), this);
                HandleHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                SendHook = LocalHook.Create(new IntPtr(0x6A21C0), new TSendData(HKSendData), this);
                SendHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }


        static unsafe void HKHandleData([MarshalAs(UnmanagedType.BStr)] string data)
        {
            try
            {
                ((Main)HookRuntimeInfo.Callback).Interface.Receive("[recv=" + data + "]");
                PHandleData(data);
            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
                PHandleData(data);
            }
        }

        static unsafe void HKSendData([MarshalAs(UnmanagedType.BStr)] ref string data)
        {
            try
            {
                ((Main)HookRuntimeInfo.Callback).Interface.Receive("[send=" + data + "]");
                PSendData(ref data);
            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
                PSendData(ref data);
            }
        }
 
    }
}
