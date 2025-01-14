using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerMovements : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lateralSpeed;

    private Rigidbody _rb;
    private GameObject turn;

    private float moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveInput = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * speed + transform.right * lateralSpeed * moveInput;

        if (turn != null)
        {
            if (moveInput == 1 && turn.transform.rotation.eulerAngles.y == 90)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                turn = null;
            } 
            else if (moveInput == -1 && turn.transform.rotation.eulerAngles.y == 270)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
                turn = null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (Mathf.Abs(scalarProduct(collision.GetContact(0).impulse, transform.forward)) > 0.1f)
        //{
        //    Debug.Log("Hit");
        //}
        if (collision.gameObject.CompareTag("wall"))
        {
            if (Mathf.Abs(scalarProduct(collision.gameObject.transform.forward.normalized, transform.forward.normalized)) < 0.1f)
            {
                Debug.Log(collision.gameObject.transform.forward + transform.forward.normalized);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("turn"))
        {
            turn = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("turn") && other.gameObject == turn)
        {
            turn = null;
        }
    }

    float scalarProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
}
