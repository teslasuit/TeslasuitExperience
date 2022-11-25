using UnityEngine;
using System.Collections;
using UnityEngine.XR;

namespace Antilatency {
    public class AltOculusHMDTracking : AltHmdTracking {
        public enum OculusDeviceType {
            Undefined,
            GearVR,
            Rift,
            Go
        }

        private OVRCameraRig _rig;
        private const float _k = 6.0f;
        private bool _checkDeviceRequiered = false;

        private OculusDeviceType _deviceType = OculusDeviceType.Undefined;

        protected override void OnEnable() {
            base.OnEnable();

            _deviceType = GetDeviceType();

            if (_deviceType != OculusDeviceType.Undefined) {
                _checkDeviceRequiered = false;
                CheckHeadsetType();
            } else {
                _checkDeviceRequiered = true;
            }

            OVRManager.boundary.SetVisible(false);
        }

        private void CheckHeadsetType() {
            if (_deviceType == OculusDeviceType.Rift) {
#if UNITY_2017_1_OR_NEWER
                Application.onBeforeRender += OnBeforeRender;
                Debug.Log("Rift");
#else
                Camera.onPreCull += OnCameraPreCull;
#endif
            }
        }

        protected override void OnDisable() {
            base.OnDisable();

            if (_deviceType == OculusDeviceType.Rift) {
#if UNITY_2017_1_OR_NEWER
                Application.onBeforeRender -= OnBeforeRender;
#else
                Camera.onPreCull -= OnCameraPreCull;
#endif
            }
            if(_rig) _rig.UpdatedAnchors -= OnUpdatedAnchors;

            OVRManager.boundary.SetVisible(true);
        }

        protected override void InitTarget() {
            if (Target == null && _rig != null) {
                Target = _rig.trackingSpace;
            }
        }

        protected override void Start() {
            _rig = GetComponent<OVRCameraRig>();

            if (_rig != null) {
                _rig.UpdatedAnchors += OnUpdatedAnchors;
            } else {
                Debug.LogError("OVRCameraRig not found");
            }

            base.Start();
        }

#if UNITY_2017_1_OR_NEWER
        private void OnBeforeRender() {
#else
        private void OnCameraPreCull(Camera cam) {
#endif
            if (_rig == null) {
                Debug.LogWarning("OVRCameraRig not found");
                return;
            }
            //Override position, we don't need hardcoded user height
            _rig.leftEyeAnchor.localPosition = Vector3.zero;
            _rig.rightEyeAnchor.localPosition = Vector3.zero;
            _rig.centerEyeAnchor.localPosition = Vector3.zero;
        }

        private AltTracking.Pose GetPose() {
            var pose = TrackingTask.GetPose(ExtrapolationTime);

            //TODO: We need to get RIGHT placement from AltSystem for Oculus Rift and Oculus Go to avoid hardcoded transformations below
            switch (_deviceType) {
                case OculusDeviceType.GearVR: {
                    pose = Placement.GetCenterEyePose(pose);
                    break;
                }
                case OculusDeviceType.Rift: {
                    pose = AltCustomPlacement.ApplyOculusRift(pose);
                    break;
                }
                case OculusDeviceType.Go: {
                    pose = AltCustomPlacement.ApplyOculusGo(pose);
                    break;
                }
                default: {
                    var headsetType = OVRPlugin.GetSystemHeadsetType();
                    if (headsetType != OVRPlugin.SystemHeadset.None) {
                        Debug.LogWarningFormat("Unknown device type {0}, placement shall be not applyed", headsetType.ToString());
                    }
                    break;
                }
            }
            return pose;
        }

        protected override void Update() {
            base.Update();

            if (_checkDeviceRequiered) {
                _deviceType = GetDeviceType();

                if (_deviceType != OculusDeviceType.Undefined) {
                    _checkDeviceRequiered = false;
                    CheckHeadsetType();
                }
            }

            if (TrackingTask != null && TrackPosition) {
                _rig.trackingSpace.localPosition = GetPose().Position.ToVector3();
            }
        }

        protected virtual void OnUpdatedAnchors(OVRCameraRig rig) {
            if (!enabled || TrackingTask == null || !TrackRotation) { return; }

            var pose = GetPose();
            var oculusRotation = _rig.centerEyeAnchor.localRotation;

            var altEnvSpaceRotation = pose.Rotation.ToQuaternion();
            var pa = altEnvSpaceRotation * Quaternion.Inverse(oculusRotation);
            var diff = Quaternion.Inverse(Target.localRotation) * pa;
            diff = Quaternion.Lerp(Quaternion.identity, diff, 1.0f - AltMathUnity.Fx(Quaternion.Angle(Quaternion.identity, diff), _k));
            rig.trackingSpace.localRotation = rig.trackingSpace.localRotation * diff;
        }

        protected override bool CheckSetup() {
            var envCheck = CheckEnvironment(true);
            var rigCheck = RigCheck(true);

            return envCheck && rigCheck;
        }

        public bool RigCheck(bool debugLog) {
            var rig = GetComponent<OVRCameraRig>();
            if (rig != null) { return true; }
            if (debugLog) {
                Debug.LogErrorFormat("{0} must be placed on OVRCameraRig gameobject", GetType().ToString());
            }
            return false;
        }

        private OculusDeviceType GetDeviceType() {
            var result = OculusDeviceType.Undefined;
            var curDevice = OVRPlugin.GetSystemHeadsetType().ToString().ToLower();

            if (curDevice.Contains("gearvr")) {
                result = OculusDeviceType.GearVR;
            }else if (curDevice.Contains("rift")) {
                result = OculusDeviceType.Rift;
            } else if (curDevice.Contains("oculus_go")) {
                result = OculusDeviceType.Go;
            }

            return result;
        }
    }
}
