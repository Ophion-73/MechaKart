using System.Collections.Generic;
using UnityEngine;

public class MechaLegIK : MonoBehaviour
{
    public float phi;
    public Transform movePoint;
    public List<float> lengths = new List<float>();
    public List<Transform> legTransforms = new List<Transform>();

    void Update()
    {
        float L1 = lengths[0]; float L2 = lengths[1]; float L3 = lengths[2];
        Vector3 P3 = movePoint.localPosition;
        Vector3 P2 = ChainInverseKinematics.Position2(phi * Mathf.Deg2Rad, L3, P3);
        legTransforms[2].localPosition = P2;
        float angle1 = ChainInverseKinematics.Angle1(L1, L2, P2);
        float angle2 = ChainInverseKinematics.Angle2(L1, L2, P2);
        float angle3 = ChainInverseKinematics.Angle3(phi *Mathf.Deg2Rad, L1, L2, P2);
        legTransforms[2].localRotation = Quaternion.Euler((-angle1 - angle2 - angle3) * Mathf.Rad2Deg, 0, 0);
        Vector3 P1 = L1 * new Vector3(0, Mathf.Sin(angle1), Mathf.Cos(angle1));
        legTransforms[1].localPosition = P1;
        legTransforms[1].localRotation = Quaternion.Euler((-angle1 - angle2) * Mathf.Rad2Deg, 0, 0);
        legTransforms[0].localRotation = Quaternion.Euler(-angle1 * Mathf.Rad2Deg, 0, 0);
    }
}
