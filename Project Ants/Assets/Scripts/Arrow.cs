using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float rotationSpeed = 200f;  // �������� ���� ���� �������� �����

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        // �������� �������� ���� ����� � ������� ��������
       /* Vector2 direction = rb.velocity.normalized;

        // ������� ���� �������� ���� �����, ��������� ��� �������� ��������
        rb.velocity = direction * rb.velocity.magnitude;*/
    }
}