using UnityEngine;

public class TitanLegsAnimation : MonoBehaviour
{
    public float speed;
    public Transform movePointRight;
    public Transform movePointLeft;
    Vector3 P0, center, amplitude;
    float time, period, restTime;

    void Start()
    {
        P0 = new Vector3(0,-1.225f, 0.25f);
        center = P0 + 0.5f * amplitude.y * Vector3.up;
        amplitude = new Vector3(0.75f, 0.3f);
    }

    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
        period = 2 * Mathf.PI / speed;
        
        time += Time.deltaTime * vInput;
        movePointRight.localPosition = EllipsePath(time + 0.25f * period);
        movePointLeft.localPosition = EllipsePath(time - 0.25f * period);

        if (vInput == 0f)
        {
            float hInput = Input.GetAxis("Horizontal");
            time -= Time.deltaTime * hInput;
            movePointRight.localPosition = EllipsePath(time + 0.25f * period);
            movePointLeft.localPosition = EllipsePath(time - 0.25f * period);
        }

    }

    Vector3 EllipsePath(float time)
    {
        float y = -amplitude.y * Mathf.Cos(speed * time);
        float z = -amplitude.x * Mathf.Sin(speed * time);
        return center + 0.5f * new Vector3(0, y, z);
    }
}
