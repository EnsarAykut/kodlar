using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;


    void Reset()
    {
        // FirstPersonMovement scriptini ebeveynlerinde arayarak karakteri al.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Fare imlecini oyun ekranına kitle.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Yumuşatılmış hızı al.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Kamerayı yukarı-aşağı ve karakteri sol-sağ yönde hızdan döndür.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
