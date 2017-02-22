﻿using EasyHook;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AORNet.Helpers;
using GeneiAO.Model;


namespace AORNet
{
    public class Main : IEntryPoint
    {
        #region -- Local Properties --

        private LocalHook HandleHook;
        private LocalHook SendHook;
        private LocalHook LoopHook;
        public static string ChannelName;
        public readonly RemoteService Interface;


        #endregion

        #region -- Static Properties --

        private static readonly IntPtr HandleAddress = new IntPtr(0x64E050);
        private static readonly IntPtr SendAddress = new IntPtr(0x69A0D0);
        private static readonly IntPtr LoopAddress = LocalHook.GetProcAddress("MSVBVM60.DLL", "rtcDoEvents");

        #endregion

        #region -- Function Pointers --
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void HandleData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void SendData([MarshalAs(UnmanagedType.BStr)] ref string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void Loop();

        public static readonly HandleData PHandleData = (HandleData)Marshal.GetDelegateForFunctionPointer(HandleAddress, typeof(HandleData));
        public static readonly SendData PSendData = (SendData)Marshal.GetDelegateForFunctionPointer(SendAddress, typeof(SendData));
        public static readonly Loop PLoop = (Loop)Marshal.GetDelegateForFunctionPointer(LoopAddress, typeof(Loop));

        #endregion

        #region -- Hooks --

        private static unsafe void HookedHandleData([MarshalAs(UnmanagedType.BStr)] string packet)
        {
            try
            {
                ((Main)HookRuntimeInfo.Callback).Interface.Receive("Recv= " + packet );  

                //packet = PacketsHelper.AnalyzeHandlePackets(packet);

            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                SendToClient(packet);
            }
        }

        private static unsafe void HookedSendData([MarshalAs(UnmanagedType.BStr)] ref string packet)
        {
            try
            {
                ((Main)HookRuntimeInfo.Callback).Interface.Receive("Send=" + packet );

                //packet = PacketsHelper.AnalyzeSendPackets(packet);

            }
            catch (Exception ex)
            {
                ((Main)HookRuntimeInfo.Callback).Interface.ErrorHandler(ex);
            }
            finally
            {
                SendToServer(packet);
            }
        }

        private static unsafe void HookedLoop()
        {
            try
            {
                //Loop Shit
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
            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }
        }
        #endregion

        #region -- Static Methods --

        public static void SendToClient(string packet)
        {
            PHandleData(packet);
        }

        public static void SendToServer(string packet)
        {
            PSendData(ref packet);
        }

        #endregion

    }
}
