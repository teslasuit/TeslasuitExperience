using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Antilatency {
    public static class AltApiDebug {
        public const string PluginName = "AltTracking";

        [DllImport(PluginName)]
        public static extern uint AltApi_getLiveObjectsCount();

        [DllImport(PluginName)]
        public static extern void AltApi_reportLiveObjects();
    }

    public class Object : IDisposable {
        public const string LibraryName = "AltTracking";

        [DllImport(LibraryName)]
        private static extern UInt32 AltApi_Object_release(IntPtr obj);

        [DllImport(LibraryName)]
        private static extern UInt32 AltApi_Object_addref(IntPtr obj);

        private IntPtr _nativePtr;

        public IntPtr NativePointer {
            get {
                if (_nativePtr == IntPtr.Zero) {
                    throw new Exception("Accessing released AltApi object!");
                }
                return _nativePtr;
            }
        }

        public Object(IntPtr nativePtr) {
            _nativePtr = nativePtr;
        }

        public void Dispose() {
            Dispose(false);
            // GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool finalizer) {
            if (_nativePtr != IntPtr.Zero) {
                AltApi_Object_release(_nativePtr);
                _nativePtr = IntPtr.Zero;
            }
        }
    }

    public static class CollectionCApi{
		[DllImport(Object.LibraryName)]
		public static extern IntPtr AltApi_Collection_getItem(IntPtr descriptorsList, UInt32 id);

		[DllImport(Object.LibraryName)]
		public static extern UInt32 AltApi_Collection_getItemsCount(IntPtr descriptorsList);
	}

	public class Collection<T> : Antilatency.Object {

		public Collection(IntPtr nativePtr) : base(nativePtr) { }

		public int Count {
			get { return (int)CollectionCApi.AltApi_Collection_getItemsCount(NativePointer); }
		}

        public T this[int index] {
            get { return GetItem(index); }
        }

        private T GetItem(int id) {
            var rawPtr = CollectionCApi.AltApi_Collection_getItem(NativePointer, (UInt32)id);
            if (rawPtr == IntPtr.Zero) {
                throw new IndexOutOfRangeException();
            }
			if (typeof(T).IsSubclassOf(typeof(Antilatency.Object))) {
				return (T)Activator.CreateInstance (typeof(T), rawPtr);
			} else {
				return (T)Marshal.PtrToStructure (rawPtr, typeof(T));
			}            
        }
    }
}