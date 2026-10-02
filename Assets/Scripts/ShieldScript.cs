using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class ShieldScript : MonoBehaviour
{
    public Transform shieldPivot;

    public float rotationSpeed = 1000f;

    private float targetAngle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //smoothly rotates shield towards angle 
        float currentAngle = shieldPivot.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle,targetAngle, rotationSpeed * Time.deltaTime);

        shieldPivot.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    public void RotateShield(InputValue value)
    {
        //changes the target angle based of input
        Vector2 input = value.Get<Vector2>();

        if (input.sqrMagnitude > 0.01f)
        {
            targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        }
    }
}