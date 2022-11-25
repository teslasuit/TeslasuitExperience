using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;

namespace AntilatencyDeviceNetwork {

    public static class CoreAPI {
        public const string PluginName = "AntilatencyDeviceNetwork";

        static CoreAPI() {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            String currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            String dllPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Assets" +
                             Path.DirectorySeparatorChar + "Antilatency" + Path.DirectorySeparatorChar + "Plugins/";
            if (currentPath == null) {
                currentPath = "";
            }
            if (currentPath.Contains(dllPath) == false) {
                Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath,
                    EnvironmentVariableTarget.Process);
            }
#endif
        }

        [DllImport(PluginName)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string adnGetVerison();

        [DllImport(PluginName)]
        public static extern UInt32 adnGetUpdateID();

        [DllImport(PluginName)]
        public static extern void adnSetLogLevel(LogLevel level);

        [DllImport(PluginName)]
        public static extern Status adnAcquireFactory();

        [DllImport(PluginName)]
        public static extern void adnReleaseFactory();

        [DllImport(PluginName)]
        public static extern Int32 adnGetNode(UInt32 id);

        [DllImport(PluginName)]
        public static extern NodeStatus adnNodeGetStatus(NodeHandle node);

        [DllImport(PluginName)]
        public static extern Status adnNodeIsInterfaceSupported(NodeHandle node, InterfaceID interfaceID);

        [DllImport(PluginName)]
        public static extern Int32 adnNodeGetParent(NodeHandle node);

        [DllImport(PluginName)]
        public static extern Status adnAcquireNode(NodeHandle node, InterfaceID interfaceID, ReceiveDataCallback receiveDataCallback, IntPtr userContext);

        [DllImport(PluginName)]
        public static extern void adnReleaseNode(NodeHandle node);

        [DllImport(PluginName)]
        public static extern Status adnSendData(NodeHandle node, IntPtr data, UInt32 size);
    }

    public static class CoreDebugAPI
    {
        public const string PluginName = "AntilatencyDeviceNetwork";

        [DllImport(PluginName)]
        public static extern Int32 adnNodeHandleMarshalTest(NodeHandle node);

        [DllImport(PluginName)]
        public static extern UInt64 adnInterfaceIDMarshalTest0(InterfaceID id);

        [DllImport(PluginName)]
        public static extern void adnInterfaceIDMarshalTest1(InterfaceID id, ref InterfaceID outId);


        [DllImport(PluginName)]
        public static extern AntilatencyDeviceNetwork.Status adnStatusMarshalTest0(AntilatencyDeviceNetwork.Status status);

        [DllImport(PluginName)]
        public static extern void adnStatusMarshalTest1(AntilatencyDeviceNetwork.Status status, ref AntilatencyDeviceNetwork.Status outStatus);

        [DllImport(PluginName)]
        public static extern AntilatencyDeviceNetwork.NodeStatus adnNodeStatusMarshalTest0(AntilatencyDeviceNetwork.NodeStatus status);

        [DllImport(PluginName)]
        public static extern void adnNodeStatusMarshalTest1(AntilatencyDeviceNetwork.NodeStatus status, ref AntilatencyDeviceNetwork.NodeStatus outStatus);
    }

 
    public struct InterfaceID {
        public InterfaceID(UInt64 id) {
            value = id;
        }
        public UInt64 value;
    }

    public enum NodeStatus {
        Unacquired = 0,
        Valid,
        Invalid
    };

    public enum Status : int {
        Fail = 0,
        Ok
    };

    public enum LogLevel {
        Debug = 0,
        Info,
        Off
    };

    public struct NodeHandle {
        public Int32 _handle;

        public static readonly NodeHandle Root = new NodeHandle(0);
        public static readonly NodeHandle Invalid = new NodeHandle(-1);

        public NodeHandle(Int32 handle) {
            _handle = handle;
        }

        public override int GetHashCode() {
            return _handle;
        }

        public override string ToString() {
            return _handle.ToString();
        }
        
        public static bool operator ==(NodeHandle a, NodeHandle b) {
            return a._handle == b._handle;
        }

        public static bool operator !=(NodeHandle a, NodeHandle b) {
            return !(a == b);
        }

        public bool Equals(NodeHandle other) {
            return other == this;
        }

        public override bool Equals(object o) {
            if (o == null || o.GetType() != typeof(NodeHandle)) {
                return false;
            }
            return _handle == ((NodeHandle)o)._handle;
        }

        public NodeStatus Status {
            get {
                return CoreAPI.adnNodeGetStatus(this);
            }
        }

        public NodeHandle Parent {
            get { return new NodeHandle(CoreAPI.adnNodeGetParent(this)); }
        }

        public bool IsInterfaceSupported(InterfaceID interfaceId) {
            return CoreAPI.adnNodeIsInterfaceSupported(this, interfaceId) == AntilatencyDeviceNetwork.Status.Ok;
        }

        public void Release() {
            CoreAPI.adnReleaseNode(this);
        }

        public bool SendData(byte[] data) {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var status = CoreAPI.adnSendData(this, handle.AddrOfPinnedObject(), (uint)data.Length);
            handle.Free();
            return status == AntilatencyDeviceNetwork.Status.Ok;
        }
    }

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate uint ReceiveDataCallback(IntPtr data, UInt32 size, IntPtr userContext);
    
    public static class Factory {

        public static bool Acquire() {
            return CoreAPI.adnAcquireFactory() == Status.Ok;
        }

        public static void Release() {
            CoreAPI.adnReleaseFactory();
        }

        public static AntilatencyDeviceNetwork.LogLevel LogLevel {
            set { CoreAPI.adnSetLogLevel(value); }
        }

        public static uint GetUpdateId() {
            return CoreAPI.adnGetUpdateID();
        }
    
        public static NodeHandle GetNode(uint id) {
            return new NodeHandle(CoreAPI.adnGetNode(id));
        }

        public static List<NodeHandle> GetAllNodes() {
            var result = new List<NodeHandle>();
            for (uint nodeId = 0;; ++nodeId) {
                var node = GetNode(nodeId);
                if (node == NodeHandle.Invalid) {
                    break;
                }
                result.Add(node);
            }
            return result;
        }

        public static bool AcquireNode(NodeHandle node, InterfaceID interfaceId, ReceiveDataCallback receiveDataCallback) {
            return CoreAPI.adnAcquireNode(node, interfaceId, receiveDataCallback, IntPtr.Zero) == Status.Ok;
        }
    }

    public struct FactoryObserver {
        public static readonly FactoryObserver Initial = new FactoryObserver(-1);

        private int LastNumChanges;

        private FactoryObserver(int NumChanges) {
            LastNumChanges = NumChanges;
        }
       
        public bool Changed() {
            int CurrentNumChanges = (int)Factory.GetUpdateId();
            //Debug.LogWarning("Factory update ID " + CurrentNumChanges);
            if (CurrentNumChanges != LastNumChanges) {
                LastNumChanges = CurrentNumChanges;
                return true;
            }
            return false;
        }
    }

    public abstract class CustomTask {
        private NodeHandle _node = NodeHandle.Invalid;
        private Semaphore _completeSemaphore = new Semaphore(0, 1);

        public bool StartTask(NodeHandle node, InterfaceID interfaceID) {
            if (_node != NodeHandle.Invalid) {
                throw new Exception("Task already was started!");
            }

            if (Factory.AcquireNode(node, interfaceID, ReceiveDataImpl)) {
                _node = node;
                return true;
            }
            return false;
        }

        public void StopTask(){
            CoreAPI.adnReleaseNode(_node);
            _completeSemaphore.WaitOne();
            _node = NodeHandle.Invalid;
        }

        private uint ReceiveDataImpl(IntPtr dataPtr, UInt32 size, IntPtr userContext) {
            byte[] data = new byte[size];

            if (size != 0) {
                Marshal.Copy(dataPtr, data, 0, (int)size);
            }
          
            var result = ReceiveData(data);
            if ((dataPtr == IntPtr.Zero) || (data.Length == 0)) {
                _completeSemaphore.Release();
            }
            return result;
        }

        protected abstract uint ReceiveData(byte[] data);
    }
}
