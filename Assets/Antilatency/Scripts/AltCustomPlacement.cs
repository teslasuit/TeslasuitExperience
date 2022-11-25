using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antilatency {
    public class AltCustomPlacement : MonoBehaviour {
        public static AltTracking.Pose Apply(AltTracking.Pose pose, Vector3 offset, Quaternion rotation) {
            var result = pose;

            result.Rotation = AltMathUnity.Float4FromQuaternion(result.Rotation.ToQuaternion() * rotation);
            result.Position = (result.Position.ToVector3() + result.Rotation.ToQuaternion() * offset).ToAltFloat3();

            return result;
        }

        public static AltTracking.Pose ApplyGearVR(AltTracking.Pose pose) {
            return Apply(pose, Vector3.back * 0.069f + Vector3.up * 0.055f,
                Quaternion.AngleAxis(-45.0f, Vector3.right));
        }

        public static AltTracking.Pose ApplyOculusRift(AltTracking.Pose pose) {
            return Apply(pose, Vector3.back * 0.091f + Vector3.up * 0.0209f,
                Quaternion.AngleAxis(180.0f, Vector3.forward) * Quaternion.AngleAxis(-46.9f, Vector3.right));
        }

        public static AltTracking.Pose ApplyOculusGo(AltTracking.Pose pose) {
            return Apply(pose, Vector3.back * 0.085f + Vector3.up * 0.005f,
                Quaternion.AngleAxis(180.0f, Vector3.forward) * Quaternion.AngleAxis(-45.0f, Vector3.right));
        }

        public static AltTracking.Pose ApplyVive(AltTracking.Pose pose) {
            return Apply(pose, Vector3.back * 0.091f + Vector3.up * 0.0209f,
                Quaternion.AngleAxis(180.0f, Vector3.forward) * Quaternion.AngleAxis(-45.0f, Vector3.right));
        }

        public static AltTracking.Pose ApplyFocus(AltTracking.Pose pose) {
            return Apply(pose, Vector3.back * 0.0958f + Vector3.up * 0.023f,
                Quaternion.AngleAxis(180.0f, Vector3.forward) * Quaternion.AngleAxis(-45.0f, Vector3.right));
        }

        public static AltTracking.Pose ApplyPicoGoblin(AltTracking.Pose pose) {
            return Apply(pose, Vector3.back * 0.07f + Vector3.up * 0.09f,
                Quaternion.AngleAxis(-45.0f, Vector3.right));
        }
    }
}