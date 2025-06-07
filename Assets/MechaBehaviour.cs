using Unity.VisualScripting;
using UnityEngine;

public class MechaBehaviour : MonoBehaviour
{
    public float forceMagnitude, torqueMagnitude;
    public float linearDrag, angularDrag;
    private Rigidbody rb;
    public bool p1;
   

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    
    void Update()
    {
        
       
        if(p1)
        {
            rb.linearDamping = linearDrag;
            rb.angularDamping = angularDrag;
            float hInput = Input.GetAxis("Horizontal");
            float vInput = Input.GetAxis("Vertical");

            Vector3 torque = hInput * torqueMagnitude * transform.up;
            Vector3 force = vInput * forceMagnitude * transform.forward;

            rb.AddTorque(torque, ForceMode.Force);
            rb.AddForce(force, ForceMode.Force);
        }
        else
        {
            rb.linearDamping = linearDrag;
            rb.angularDamping = angularDrag;
            float hInput = Input.GetAxis("Horizontal-LS");
            float vInput = Input.GetAxis("Vertical-LS");

            Vector3 torque = hInput * torqueMagnitude * transform.up;
            Vector3 force = vInput * forceMagnitude * transform.forward;

            rb.AddTorque(torque, ForceMode.Force);
            rb.AddForce(force, ForceMode.Force);
        }
        
    }
    
    
}
