using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class ChainInverseKinematics
{
    static float CosAngle2(float L1, float L2, Vector3 P2)
    {
        float result = (P2.z * P2.z + P2.y * P2.y - L1 * L1 - L2 * L2) / (2 * L1 * L2);
        return result;
    }
    
    static float SinAngle2(float cosAngle2)
    {
        //float sign = 1f;
        float sign = -1f;
        float result = sign * Mathf.Sqrt(1 - cosAngle2 * cosAngle2);
        return result;
    }

    static float CosAngle1(float L1, float L2, Vector3 P2)
    {
        float cosAngle2 = CosAngle2(L1, L2, P2);
        float sinAngle2 = SinAngle2(cosAngle2);
        float result = ((L1 + L2 * cosAngle2) * P2.z +L2 * sinAngle2 * P2.y) / (P2.z * P2.z + P2.y * P2.y);
        return result;
    }

    static float SinAngle1(float L1, float L2, Vector3 P2)
    {
        float cosAngle2 = CosAngle2(L1, L2, P2);
        float sinAngle2 = SinAngle2(cosAngle2);
        float result = ((L1 + L2 * cosAngle2) * P2.y - L2 * sinAngle2 * P2.z) / (P2.z * P2.z + P2.y * P2.y);
        return result;
    }

    public static float Angle1(float L1, float L2, Vector3 P2)
    {
        float cosAngle1 = CosAngle1(L1, L2, P2);
        float sinAngle1 = SinAngle1(L1, L2, P2);
        return Mathf.Atan2(sinAngle1, cosAngle1);
    }

    public static float Angle2(float L1, float L2, Vector3 P2)
    {
        float cosAngle2 = CosAngle2(L1, L2, P2);
        float sinAngle2 = SinAngle2(cosAngle2);
        return Mathf.Atan2(sinAngle2, cosAngle2);
    }

    public static float Angle3(float phi, float L1, float L2, Vector3 P2)
    {
        return phi - Angle1(L1, L2, P2)- Angle2(L1,L2,P2);
    }

    public static Vector3 Position2(float phi, float L3, Vector3 P3)
    {
        float cosPhi = Mathf.Cos(phi);
        float SinPhi = Mathf.Sin(phi);
        return P3 - L3 * new Vector3(0, SinPhi, cosPhi);
    }

    public static Vector3 Position1(float L1, float L2, Vector3 P2)
    {
        float cosAngle1 = CosAngle1(L1, L2, P2);
        float sinAngle1 = SinAngle1(L1, L2, P2);
        return new Vector3(0f, L1 * sinAngle1, L1* cosAngle1);
    }

}
