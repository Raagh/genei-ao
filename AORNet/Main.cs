using EasyHook;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private LocalHook LoopHook;
        public static string ChannelName;
        //public readonly RemoteService Interface;
        public readonly MainModel Interface;

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void THandleData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void TSendData([MarshalAs(UnmanagedType.BStr)] ref string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void TLoop();

        public static readonly THandleData PHandleData = (THandleData)Marshal.GetDelegateForFunctionPointer(new IntPtr(0x64E050), typeof(THandleData));
        public static readonly TSendData PSendData = (TSendData)Marshal.GetDelegateForFunctionPointer(new IntPtr(0x69A0D0), typeof(TSendData));
        public static readonly TLoop PLoop = (TLoop) Marshal.GetDelegateForFunctionPointer(LocalHook.GetProcAddress("MSVBVM60.DLL","rtcDoEvents"), typeof(TLoop));

        public Main(RemoteHooking.IContext inContext, String inChannelName)
        {
            try
            {
                Interface = RemoteHooking.IpcConnectClient<MainModel>(inChannelName);
                ChannelName = inChannelName;
                //RemoteHooking.GetCurrentProcessId()
                //Interface.IsInstalled(true);
                Interface.Status = true;
            }
            catch (Exception ex)
            {
                //Interface.ErrorHandler(ex);
                Interface.Error = ex.ToString();
            }
        }

        public unsafe void Run(RemoteHooking.IContext inContext, String inChannelName)
        {
            try
            {
                HandleHook = LocalHook.Create(new IntPtr(0x64E050), new THandleData(HKHandleData), this);
                HandleHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                SendHook = LocalHook.Create(new IntPtr(0x69A0D0), new TSendData(HKSendData), this);
                SendHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                LoopHook = LocalHook.Create(LocalHook.GetProcAddress("MSVBVM60.DLL", "rtcDoEvents"), new TLoop(HKLoop), this);
                LoopHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ex)
            {
                //Interface.ErrorHandler(ex);
                Interface.Error = ex.ToString();
            }
        }


        private static unsafe void HKHandleData([MarshalAs(UnmanagedType.BStr)] string data)
        {
            try
            {
                //(Main)HookRuntimeInfo.Callback).Interface.Receive("Handle= " + data );               
            }
            catch (Exception ex)
            {
                //((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                PHandleData(data);
            }
        }

        private static unsafe void HKSendData([MarshalAs(UnmanagedType.BStr)] ref string data)
        {
            try
            {
                //((Main)HookRuntimeInfo.Callback).Interface.Receive("Send=" + data );
                
            }
            catch (Exception ex)
            {
                //((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                PSendData(ref data);
            }
        }

        private static unsafe void HKLoop()
        {
            try
            {
                //Loop Shit
            }
            catch (Exception ex)
            {
                //((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                PLoop();
            }
        }

    }
}
