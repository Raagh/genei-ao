using EasyHook;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AORNet.Helpers;
using ConsoleTest;


namespace AORNet
{
    public class Main : IEntryPoint
    {

        #region -- Local Properties --

        private LocalHook HandleHook;
        private LocalHook SendHook;
        private LocalHook LoopHook;
        private LocalHook EncriptHook;
        public static string ChannelName;
        public readonly RemoteService Interface;

        #endregion

        #region -- Static Properties --

        private static readonly IntPtr HandleAddress = new IntPtr(0x64E550);
        private static readonly IntPtr SendAddress = new IntPtr(0x69A5D0);
        private static readonly IntPtr EncryptAddress = new IntPtr(0x6EBD10);
        private static readonly IntPtr LoopAddress = LocalHook.GetProcAddress("MSVBVM60.DLL", "rtcDoEvents");

        #endregion

        #region -- Function Pointers --

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void HandleData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void SendData([MarshalAs(UnmanagedType.BStr)] ref string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public unsafe delegate string EncryptData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void Loop();

        public static readonly HandleData PHandleData = (HandleData)Marshal.GetDelegateForFunctionPointer(HandleAddress, typeof(HandleData));
        public static readonly SendData PSendData = (SendData)Marshal.GetDelegateForFunctionPointer(SendAddress, typeof(SendData));
        public static readonly Loop PLoop = (Loop)Marshal.GetDelegateForFunctionPointer(LoopAddress, typeof(Loop));
        public static readonly EncryptData PEncryptData = (EncryptData) Marshal.GetDelegateForFunctionPointer(EncryptAddress, typeof(EncryptData));

        #endregion

        #region -- Hooks --

        private static unsafe void HookedHandleData([MarshalAs(UnmanagedType.BStr)] string packet)
        {
            try
            {
                //((Main)HookRuntimeInfo.Callback).Interface.Message("Recv= " + packet);

                PacketsHelper.AnalyzeHandlePackets(packet);

            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                PHandleData(packet);
            }
        }

        private static unsafe void HookedSendData([MarshalAs(UnmanagedType.BStr)] ref string packet)
        {
            try
            {
                //((Main)HookRuntimeInfo.Callback).Interface.Message("Send=" + packet);

                PacketsHelper.AnalyzeSendPackets(packet);

            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                PSendData(ref packet);
            }
        }

        private static unsafe void HookedLoop()
        {
            try
            {       
                //TODO Revisar poner en distintos Threads para poder tomar potas y usar el AutoAim   
                                            
                //CheatingHelper.AutoRemo();              
                CheatingHelper.AutoAim();
                CheatingHelper.AutoPotas();
                //CheatingHelper.SpeedHack();
            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                PLoop();
            }
        }

        [return: MarshalAs(UnmanagedType.BStr)]
        private static unsafe string HookedEncrypt([MarshalAs(UnmanagedType.BStr)] string packet)
        {
            try
            {
                //((Main)HookRuntimeInfo.Callback).Interface.Message("Encrypt= " + packet);
            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }

            return PEncryptData(packet);
        }

        #endregion

        #region -- DLL Methods --
        public Main(RemoteHooking.IContext inContext, String inChannelName)
        {
            try
            {
                Interface = RemoteHooking.IpcConnectClient<RemoteService>(inChannelName);
                ChannelName = inChannelName;
                Interface.IsInstalled(true);
            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }
        }

        public unsafe void Run(RemoteHooking.IContext inContext, String inChannelName)
        {
            try
            {
                HandleHook = LocalHook.Create(HandleAddress, new HandleData(HookedHandleData), this);
                HandleHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                SendHook = LocalHook.Create(SendAddress, new SendData(HookedSendData), this);
                SendHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                LoopHook = LocalHook.Create(LoopAddress, new Loop(HookedLoop), this);
                LoopHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                EncriptHook = LocalHook.Create(EncryptAddress, new EncryptData(HookedEncrypt), this);
                EncriptHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                while (true)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }
        }
        #endregion
    }

}
