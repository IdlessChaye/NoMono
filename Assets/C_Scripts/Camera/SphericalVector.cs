using UnityEngine;
public struct SphericalVector {
    public float length;
    public float azimuth;
    public float zenith;

    public SphericalVector(float l, float a, float z) {
        this.length = l;
        this.azimuth = a;
        this.zenith = z;
    }

    public Vector3 Position {
        get {
            return length * Direction;
        }
    }
    public Vector3 Direction {
        get {
            float z = zenith * Mathf.PI / 2;
            float a = azimuth * Mathf.PI;
            Vector3 ret = Vector3.zero;
            ret.y = Mathf.Sin(z);
            ret.x = Mathf.Cos(z) * Mathf.Sin(a);
            ret.z = Mathf.Cos(z) * Mathf.Cos(a);
            return ret;
        }
    }

}
