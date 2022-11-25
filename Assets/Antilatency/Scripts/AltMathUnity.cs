using UnityEngine;
using Antilatency;

public static class AltMathUnity {

    public static Vector2 ToVector2(this AltTracking.float2 a) {
        return new Vector2(a.x, a.y);
    }

    public static Vector3 ToVector3(this AltTracking.float3 a) {
        return new Vector3(a.x, a.y, a.z);
    }

    public static AltTracking.float3 ToAltFloat3(this Vector3 a) {
        return new AltTracking.float3 { x = a.x, y = a.y, z = a.z };
    }
    
    public static Quaternion ToQuaternion(this AltTracking.float4 a) {
        return new Quaternion(a.x, a.y, a.z, a.w);
    }

    public static AltTracking.float4 Float4FromQuaternion(Quaternion q) {
        return new AltTracking.float4() { x = q.x, y = q.y, z = q.z, w = q.w };
    }

    public static Quaternion Diff(Quaternion from, Quaternion to) {
        return to * Quaternion.Inverse(from);
    }

    public static Quaternion QuaternionFromAngularVelocity(Vector3 v) {
        var angle = v.magnitude;
        if (angle > 0.0f) {
            var k = Mathf.Sin(angle * 0.5f) / angle;
            return new Quaternion() { x = v.x * k, y = v.y * k, z = v.z * k, w = Mathf.Cos(angle * 0.5f) };
        } else {  //to avoid illegal expressions
            return Quaternion.identity;
        }
    }
    
    public static Vector3 ToAngularVelocity(Quaternion q) {
        var rw = q.w;
        if (rw > 1f) {
            rw = 1f;
        } else if (rw < -1f) {
            rw = -1f;
        }
        var v = new Vector3(q.x, q.y, q.z).normalized;
        return v * 2f * Mathf.Acos(rw);
    }

    // extrapolation
    public static float Fx(float x, float k) {
        var xDk = x / k;
        return 1.0f / (xDk * xDk + 1);
    }
}