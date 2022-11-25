using System.Collections;
using System.Collections.Generic;
using AntilatencyDeviceNetwork;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Linq;

namespace Antilatency {
    public class AltHmdTracking : MonoBehaviour {

        public bool Connected {
            get { return TrackingTask != null && TrackingTask.Connected; }
        }

        protected bool FactoryAcquired = false;
        protected FactoryObserver FactoryObserver = FactoryObserver.Initial;
        protected AltTracking TrackingTask = null;
        protected AltPlacement Placement;

        public AltEnvironment Environment;
		public float ExtrapolationTime = 0.03f;

        public bool TrackPosition = true;
        public bool TrackRotation = true;

        [HideInInspector]
        public Transform Target {
            get { return _target; }
            protected set {
                _target = value;
                if (Environment != null) {
                    Environment.SetContoursTarget(_target);
                }
            }
        }

        private Transform _target;
		
        private bool _firstUpdateSkipped = false;

        protected virtual void OnEnable() {
            if (Environment != null) { return; }

            var env = GetComponentsInParent<AltEnvironment>().Where(v => v.isActiveAndEnabled).ToList();
            if (env.Count == 1) {
                Environment = env[0];
                Debug.LogFormat("Environment found at gameobject \"{0}\", assigned to \"{1}\" component at \"{2}\"", Environment.gameObject.name, GetType().ToString(), gameObject.name);
            }
        }

        protected virtual void OnDisable() {
            StopTracking();
        }
        
        protected virtual void OnDestroy() {
            StopTracking();
        }

        protected virtual void InitTarget() {
            if (Target == null) {
                Target = transform;
            }
        }

        protected virtual void Start() {
            CheckSetup();
            InitTarget();
        }

        private void StopTracking() {
            if (TrackingTask != null) {
                TrackingTask.Dispose();
                TrackingTask = null;
            }

            if (FactoryAcquired) {
                Factory.Release();
                FactoryAcquired = false;
            }

            if (Placement != null) {
                Placement.Dispose();
                Placement = null;
            }

            FactoryObserver = FactoryObserver.Initial;
        }

        private AltTracking AcquireRootTracker(Environment environment) {
            var requiredInterface = AltTracking.InterfaceId;

            uint i = 0;
            var node = NodeHandle.Invalid;
            while (true) {
    
                node = Factory.GetNode(i);
                if (node._handle < 0)
                    break;
                i++;

                if (node.Parent == NodeHandle.Root) {
                    if (node.IsInterfaceSupported(requiredInterface))
                    {
                        var result = AltTracking.Start(node, environment);
                        if (result != null) {
                            return result;
                        }
                    } else {
                        //var r = CoreAPI.adnNodeIsInterfaceSupported(node, requiredInterface);
                    }
                } else {
                    Debug.Log("Node parent is not root");
                }
            }
            return null;
        }

        protected virtual void Update() {
            if (Target == null) {
                InitTarget();
            }
            //TODO: remove that bugfix
            if (_firstUpdateSkipped) {
                if (Placement == null) {
                    Placement = AltPlacement.Create();
                }
                if (!FactoryAcquired) {
                    FactoryAcquired = Factory.Acquire();
                    Factory.LogLevel = LogLevel.Info;
                    if (!FactoryAcquired) { return; }
                }

                if (FactoryObserver.Changed() || (TrackingTask != null && !TrackingTask.Connected)) {
                    if (TrackingTask != null) {
                        if (!TrackingTask.Connected) {
                            TrackingTask.Dispose();
                            TrackingTask = null;
                        }
                    }

                    if (TrackingTask == null) {
                        TrackingTask = AcquireRootTracker(Environment.Environment);
                    }
                }
            } else {
                _firstUpdateSkipped = true;
            }
        }

        protected virtual bool CheckSetup() {
            var envCheck = CheckEnvironment(true);

            return envCheck;
        }

        public virtual bool CheckEnvironment(bool debugLog) {
            var env = transform.GetComponentInParent<AltEnvironment>();
            if (env != null) { return true; }
            if (debugLog) {
                Debug.LogErrorFormat(
                    "AltEnvironment component not found! Add AltEnvironment to parent gameobject for {0}.",
                    GetType().ToString());
            }
            return false;
        }
    }
}