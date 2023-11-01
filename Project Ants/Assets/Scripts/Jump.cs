using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class JumpController : MonoBehaviour
{
    [SerializeField]
    private float _jumpforce;
    private ContactFilter2D _platform;
    private bool _doublejump;
    private bool _isJumpScheduled = false;

    [SerializeField]
    private Rigidbody2D _rigidbody;
    private bool _isOnPlatform => _rigidbody.IsTouching(_platform);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (Input.GetKeyDown("w"))
        { 
            Jump();
        }
        else if (Input.GetKeyDown("d"))
        {
            RightJump();
        }
        else if (Input .GetKeyDown("a"))
        {
            LeftJump();
        }
    }

    public void Jump()
    {
        if (_isJumpScheduled)
        {
            // Якщо виклик Jump вже запланований, виходимо з функції
            return;
        }

        if (_isOnPlatform == true)
        {
            _rigidbody.AddForce(Vector2.up * _jumpforce, ForceMode2D.Impulse);
            _doublejump = true;
            //Debug.Log("Jump");
        }
        else if (_doublejump == true)
        {
            _rigidbody.AddForce(Vector2.up * _jumpforce/2, ForceMode2D.Impulse);
            _doublejump = false;
            //Debug.Log("Double Jump");
        }
        Invoke("DelayedJump", 0.2f);
        _isJumpScheduled = true;
    }

    private void DelayedJump()
    {
        // Код, що виконується після затримки
        // Наприклад, додаткові дії після стрибка
        // ...

        // Після виконання затримки знову дозволяємо виклик Jump
        _isJumpScheduled = false;
    }

    public void RightJump()
    {
        _rigidbody.AddForce(Vector2.right * _jumpforce, ForceMode2D.Impulse);
    }

    public void LeftJump()
    {
        _rigidbody.AddForce(Vector2.left * _jumpforce, ForceMode2D.Impulse);
    }
}
