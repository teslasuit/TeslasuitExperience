#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine;
using Antilatency;

public class AltDirectHMDTracking : AltHmdTracking {
    public bool UsePlacement = false;

	private AltTracking.Pose _trackerPose;

    protected override void Update() {
        base.Update();
        if (TrackingTask == null) { return; }

        _trackerPose = TrackingTask.GetPose(ExtrapolationTime);

        if (UsePlacement) {
            _trackerPose = Placement.GetCenterEyePose(_trackerPose);
        }
		    
        if (TrackPosition) {
            Target.localPosition = _trackerPose.Position.ToVector3();
        }

        if (TrackRotation) {
            Target.localRotation = _trackerPose.Rotation.ToQuaternion();
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos() {
        if (Placement == null) { return; }

        var centerEyePose = Placement.GetCenterEyePose(_trackerPose);

        //Matrix4x4 parentTransform = Target.parent == null ? Matrix4x4.identity : Target.parent.localToWorldMatrix;

        var eyeWorldPosition = centerEyePose.Position.ToVector3();
        var trackerWorldPosition = _trackerPose.Position.ToVector3();

        var eyeWorldForward = centerEyePose.Rotation.ToQuaternion() * Vector3.forward;
        var trackerWorldForward = _trackerPose.Rotation.ToQuaternion() * Vector3.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(eyeWorldPosition, eyeWorldPosition + eyeWorldForward);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(trackerWorldPosition, trackerWorldPosition + trackerWorldForward);
    }
#endif
}
