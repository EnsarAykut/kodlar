using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Reset()
    {
        // groundCheck bileşenini almaya çalış.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Rigidbody'yi al.
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // Zıplama düğmesi basılı olduğunda ve yerdeysek zıpla.
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }
}
