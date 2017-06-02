using EasyHook;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AORNet.Helpers;
using AORNet.Model;
using ConsoleTest;


namespace AORNet
{
    public class Main : IEntryPoint
    {
        #region -- Local Properties --

        private LocalHook HandleHook;
        private LocalHook SendHook;
        private LocalHook LoopHook;
        private LocalHook EncryptHook;
        public static string ChannelName;
        public readonly RemoteService Interface;

        #endregion

        #region -- Threads --

        public static Thread AutoAimThread;
        public static Thread SpeedhackThread;
        public static Thread AutoRemoThread;

        #endregion

        #region -- Static Properties --

        private static readonly IntPtr HandleAddress = new IntPtr(0x64E480);
        private static readonly IntPtr SendAddress = new IntPtr(0x69A6D0);
        private static readonly IntPtr EncryptAddress = new IntPtr(0x6EBE20);
        private static readonly IntPtr LoopAddress = LocalHook.GetProcAddress("MSVBVM60.DLL", "rtcDoEvents");

        #endregion

        #region -- Function Pointers --

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public delegate void HandleData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public delegate void SendData([MarshalAs(UnmanagedType.BStr)] ref string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public delegate string EncryptData([MarshalAs(UnmanagedType.BStr)] string data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public delegate void Loop();

        public static HandleData PHandleData = (HandleData)Marshal.GetDelegateForFunctionPointer(HandleAddress, typeof(HandleData));
        public static SendData PSendData = (SendData)Marshal.GetDelegateForFunctionPointer(SendAddress, typeof(SendData));
        public static Loop PLoop = (Loop)Marshal.GetDelegateForFunctionPointer(LoopAddress, typeof(Loop));
        public static EncryptData PEncryptData = (EncryptData)Marshal.GetDelegateForFunctionPointer(EncryptAddress, typeof(EncryptData));




        #endregion

        #region -- Hooks --

        private static void HookedHandleData([MarshalAs(UnmanagedType.BStr)] string packet)
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

        private static void HookedSendData([MarshalAs(UnmanagedType.BStr)] ref string packet)
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

        private static void HookedLoop()
        {
            try
            {
                CheatingOnMultipleThreads();

                ClearPacketFromStack();
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
        private static string HookedEncrypt([MarshalAs(UnmanagedType.BStr)] string packet)
        {
            return PEncryptData(packet);
            //((Main)HookRuntimeInfo.Callback).Interface.Message("Encrypt= " + packet);
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

        public void Run(RemoteHooking.IContext inContext, String inChannelName)
        {
            try
            {
                HandleHook = LocalHook.Create(HandleAddress, new HandleData(HookedHandleData), this);
                HandleHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                SendHook = LocalHook.Create(SendAddress, new SendData(HookedSendData), this);
                SendHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                LoopHook = LocalHook.Create(LoopAddress, new Loop(HookedLoop), this);
                LoopHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                EncryptHook = LocalHook.Create(EncryptAddress, new EncryptData(HookedEncrypt), this);
                EncryptHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            }
            catch (Exception ex)
            {
                Interface.ErrorHandler(ex);
            }

            // wait for host process termination...
            try
            {
                while (true)
                {
                }
            }
            catch
            {
                // NET Remoting will raise an exception if host is unreachable
            }
        }
        #endregion

        #region -- Extra Methods --

        private static void ClearPacketFromStack()
        {
            if (PacketsHelper.PacketsStack.Any())
            {
                var packet = PacketsHelper.PacketsStack.Pop();
                if (packet.Direction == PacketDirection.ToServer)
                {
                    var packetStr = packet.Content;
                    PSendData(ref packetStr);
                }
                else
                {
                    PHandleData(packet.Content);
                }
            }
        }

        private static void CheatingOnMultipleThreads()
        {
            // Here we start all cheating methods, AutoPotas runs on the same thread as the Game
            // Everything else runs on special threads.

            CheatingHelper.AutoPotas();

            if (AutoAimThread == null)
            {
                AutoAimThread = new Thread(CheatingHelper.AutoAim);
                AutoAimThread.Start();
            }

            //CheatingHelper.AutoRemo();    
            //CheatingHelper.SpeedHack();

        }

        #endregion

    }

}
