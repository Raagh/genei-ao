﻿using EasyHook;

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
        #region -- Instance Properties --

        private LocalHook HandleHook;
        private LocalHook SendHook;
        private LocalHook LoopHook;
        public static string ChannelName;
        //public readonly RemoteService Interface;
        public readonly MainModel Interface;

        #endregion

        #region -- Static Properties --

        private static IntPtr HandleAddress = new IntPtr(0x64E050);
        private static IntPtr SendAddress = new IntPtr(0x69A0D0);
        private static IntPtr LoopAddress = LocalHook.GetProcAddress("MSVBVM60.DLL", "rtcDoEvents");

        #endregion

        #region -- Function Pointers --
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void THandleData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void TSendData([MarshalAs(UnmanagedType.BStr)] ref string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public unsafe delegate void TLoop();

        public static readonly THandleData PHandleData = (THandleData)Marshal.GetDelegateForFunctionPointer(HandleAddress, typeof(THandleData));
        public static readonly TSendData PSendData = (TSendData)Marshal.GetDelegateForFunctionPointer(SendAddress, typeof(TSendData));
        public static readonly TLoop PLoop = (TLoop)Marshal.GetDelegateForFunctionPointer(LoopAddress, typeof(TLoop));

        #endregion


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
                HandleHook = LocalHook.Create(HandleAddress, new THandleData(HKHandleData), this);
                HandleHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                SendHook = LocalHook.Create(SendAddress, new TSendData(HKSendData), this);
                SendHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                LoopHook = LocalHook.Create(LoopAddress, new TLoop(HKLoop), this);
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
                ((Main)HookRuntimeInfo.Callback).Interface.Error = ex.ToString();
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
                ((Main)HookRuntimeInfo.Callback).Interface.Error = ex.ToString();
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
                ((Main)HookRuntimeInfo.Callback).Interface.Error = ex.ToString();
            }
            finally
            {
                PLoop();
            }
        }

    }
}
