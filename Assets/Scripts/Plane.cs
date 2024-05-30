using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float touchTime = 0f;
    public float maxJumpForce = 1000f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touchTime += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<Rigidbody>(out Rigidbody rb);

        if(other.CompareTag("Player"))
        {
            float jumpForce = Mathf.Min(touchTime * 100, maxJumpForce);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        touchTime = 0f;
    }
}
