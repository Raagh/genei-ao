using EasyHook;
using GeneiAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AORNet
{
    public class Main : IEntryPoint
    {
        LocalHook ReceiveHook;


        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void TReceive(IntPtr obj, Int16 packetId, [MarshalAs(UnmanagedType.BStr)] string data);

        public static readonly TReceive oReceive = (TReceive)Marshal.GetDelegateForFunctionPointer(new IntPtr(0x4B0000), typeof(TReceive));

        static string ChannelName;
        RemoteMon Interface;

        public Main(RemoteHooking.IContext InContext, String InChannelName)
        {
            try
            {
                Interface = RemoteHooking.IpcConnectClient<RemoteMon>(InChannelName);
                ChannelName = InChannelName;
                Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }
        }

        static unsafe void hkReceive(IntPtr obj, Int16 packetId, [MarshalAs(UnmanagedType.BStr)] string data)
        {
            try
            {
                ((Main)HookRuntimeInfo.Callback).Interface.Receive("--------------------------------------");
                ((Main)HookRuntimeInfo.Callback).Interface.Receive(string.Format("[pId=" + packetId.ToString() + "]"));
                ((Main)HookRuntimeInfo.Callback).Interface.Receive(string.Format("[data=" + data + "]"));
                oReceive(obj, packetId, data);
            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
                oReceive(obj, packetId, data);
            }
        }

        public unsafe void Run(RemoteHooking.IContext InContext, String InChannelName)
        {
            try
            {
                ReceiveHook = LocalHook.Create(new IntPtr(0x4B0000), new TReceive(hkReceive), this);
                ReceiveHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
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
    
    }
}
