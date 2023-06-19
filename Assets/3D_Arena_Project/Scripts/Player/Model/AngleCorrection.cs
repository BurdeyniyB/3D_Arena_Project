using UnityEngine;

public class AngleCorrection
{
    public Quaternion Correction(Quaternion rotation, Rigidbody rb)
    {
        Quaternion newRotation = rb.rotation * rotation;
        Vector3 eulerRotation = newRotation.eulerAngles;
        eulerRotation.x = ClampAngle(eulerRotation.x, -20f, 20f);
        eulerRotation.z = 0f;

        newRotation = Quaternion.Euler(eulerRotation);
        return newRotation;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f)
        {
            angle += 360f;
        }

        if (angle > 180f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }
}

