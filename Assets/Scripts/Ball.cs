using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody m_rigidBody;
    float m_speed = 20f;
    Vector3 m_velocity;
    Renderer m_renderer;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_renderer = GetComponent<Renderer>();
        Invoke("launch", 0.5f);
    }

    void launch()
    {
        m_rigidBody.velocity = Vector3.up * m_speed;
    }

    private void FixedUpdate() 
    {
        m_rigidBody.velocity = m_rigidBody.velocity.normalized * m_speed;
        m_velocity = m_rigidBody.velocity;
        if (!m_renderer.isVisible)
        {
            GameManager.s_instance.Balls--;
            Destroy(gameObject);
        }
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision) 
    {
      m_rigidBody.velocity = Vector3.Reflect(m_velocity, collision.contacts[0].normal);
    }
}
