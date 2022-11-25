using System;
using System.Runtime.InteropServices;
using AntilatencyDeviceNetwork;

namespace Antilatency {

    public class AltBar : Antilatency.Object {

        [DllImport(LibraryName)]
        private static extern void AltApi_AltBar_getPositionAndDirection(IntPtr altBarPtr, ref AltTracking.float2 position, ref AltTracking.float2 direction);

        public AltBar(IntPtr nativePtr) : base(nativePtr) { }

        public void GetPositionAndDirection(ref AltTracking.float2 position, ref AltTracking.float2 direction) {
            AltApi_AltBar_getPositionAndDirection(NativePointer, ref position, ref direction);
        }
    }

    public class AltPlacement : Antilatency.Object {
        [DllImport(LibraryName)]
        private static extern IntPtr AltApi_AltPlacement_get();

        [DllImport(LibraryName)]
        private static extern void AltApi_AltPlacement_getPose(IntPtr _this, ref AltTracking.Pose sourcePose, ref AltTracking.Pose centerEyePose);

        public AltPlacement(IntPtr nativePtr) : base(nativePtr) { }

        public static AltPlacement Create(){
            return new AltPlacement(AltApi_AltPlacement_get());
        }

        public AltTracking.Pose GetCenterEyePose(AltTracking.Pose trackerPose) {
            AltTracking.Pose result = new AltTracking.Pose();
            AltApi_AltPlacement_getPose(NativePointer, ref trackerPose, ref result);
            return result;
        }
    }

    public class Environment : Antilatency.Object {
        //C API
        [DllImport(LibraryName)]
        private static extern IntPtr AltApi_Environment_getBars(IntPtr environmentPtr);

        [DllImport(LibraryName)]
        private static extern IntPtr AltApi_Environment_create();

        [DllImport(LibraryName)]
        private static extern UInt32 AltApi_Environment_getContoursCount(IntPtr _this);

        [DllImport(LibraryName)]
        private static extern UInt32 AltApi_Environment_getContourPointsCount(IntPtr _this, uint idContour);
        //private static extern UInt32 AltApi_Environment_getContourPointsCount(IntPtr _this, int idContour);

        [DllImport(LibraryName)]
        private static extern void AltApi_Environment_getContourPoint(IntPtr _this, uint idContour, uint idPoint, ref AltTracking.float2 point2);
        //private static extern void AltApi_Environment_getContourPoint(IntPtr _this, int idPoint, ref AltTracking.float2 point2);

        [DllImport(LibraryName)]
        private static extern byte AltApi_Environment_isValid(IntPtr _this);

        public Environment(IntPtr nativePtr) : base(nativePtr)
        { }

        public Antilatency.Collection<AltBar> GetBars() {
            return new Antilatency.Collection<AltBar>(AltApi_Environment_getBars(NativePointer));
        }

        public static Environment Create() {
            return new Environment(AltApi_Environment_create());
        }

        public uint GetContoursCount() {
            return AltApi_Environment_getContoursCount(NativePointer);
        }

        public uint GetContourPointsCount(uint contourId) {
            return AltApi_Environment_getContourPointsCount(NativePointer, contourId);
        }

        public AltTracking.float2 GetContourPoint(uint idContour, uint pointId) {
            AltTracking.float2 result = new AltTracking.float2();
            AltApi_Environment_getContourPoint(NativePointer, idContour, pointId, ref result);
            return result;
        }

        public bool IsValid {
            get { return AltApi_Environment_isValid(NativePointer) != 0; }
        }
    }

    public class AltTracking : Antilatency.Object {

        public static readonly InterfaceID InterfaceId = new InterfaceID(0x1000000000000000U);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct float2 {
            public float x;
            public float y;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct float3 {
            public float x;
            public float y;
            public float z;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct float4 {
            public float x;
            public float y;
            public float z;
            public float w;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Pose {
            public float3 Position;
            public float3 Velocity;
            public float3 Acceleration;

            public float4 Rotation;
            public float3 AngularVelocity;
            public float3 AngularAcceleration;
        }
        
        //C API
        [DllImport(LibraryName)]
        public static extern void AltApi_init(IntPtr context);

        [DllImport(LibraryName)]
        private static extern IntPtr AltApi_Tracking_create(NodeHandle node, IntPtr environment);

        [DllImport(LibraryName)]
        private static extern void AltApi_Tracking_finalize(IntPtr trackingPtr);

        [DllImport(LibraryName)]
        private static extern void AltApi_Tracking_getPose(IntPtr _this, ref Pose pose, float deltaTime);

        [DllImport(LibraryName)]
        private static extern void AltApi_Tracking_update(IntPtr trackingPtr);

        [DllImport(LibraryName)]
        private static extern IntPtr AltApi_Tracking_getEnvironment(IntPtr trackingPtr);

        [DllImport(LibraryName)]
        private static extern byte AltApi_Tracking_connected(IntPtr _this);
        
        public AltTracking(IntPtr nativePtr) : base(nativePtr) { }

        public Pose GetPose(float predictionTime = 0) {
            Pose pose = new Pose();
            AltApi_Tracking_getPose(NativePointer, ref pose, predictionTime);
            return pose;
        }

        public bool Connected {
            get { return AltApi_Tracking_connected(NativePointer) != 0; }
        }

        public Environment Environment {
            get { return new Environment(AltApi_Tracking_getEnvironment(NativePointer)); }
        }

        public static AltTracking Start(NodeHandle node, Environment environment) {
            var tracking = AltApi_Tracking_create(node, environment.NativePointer);
            if (tracking == IntPtr.Zero) {
                return null;
            }
            return new AltTracking(tracking);
        }

        protected override void Dispose(bool finalizer) {
            if (NativePointer != IntPtr.Zero) {
                AltApi_Tracking_finalize(NativePointer);
            }
            base.Dispose(finalizer);
        }
    }
}