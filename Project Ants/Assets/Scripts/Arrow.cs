using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float rotationSpeed = 200f;  // Ўвидк≥сть зм≥ни кута повороту стр≥ли

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        // ќтримуЇмо напр€мок руху стр≥ли з вектора швидкост≥
       /* Vector2 direction = rb.velocity.normalized;

        // «м≥нюЇмо лише напр€мок руху стр≥ли, залишаючи кут повороту незм≥нним
        rb.velocity = direction * rb.velocity.magnitude;*/
    }
}