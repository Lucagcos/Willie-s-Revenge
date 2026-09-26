using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldScript : MonoBehaviour
{
    public Transform shieldPivot;

    public float rotationSpeed = 1000f;

    private float targetAngle;

    void Update()
    {
        float currentAngle = shieldPivot.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        shieldPivot.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    public void RotateShield(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input.sqrMagnitude > 0.01f)
        {
            targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        }
    }
}