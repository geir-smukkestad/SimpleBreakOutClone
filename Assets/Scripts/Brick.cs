using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hits = 1;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;

    Material m_orgMaterial;
    Renderer m_renderer;

    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
        m_renderer = GetComponent<Renderer>();
        m_orgMaterial = m_renderer.sharedMaterial;
    }

    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other) 
    {
        hits--;
        if (hits <= 0)
        {
            GameManager.s_instance.Score += points;
            Destroy(gameObject);
        }

        m_renderer.sharedMaterial = hitMaterial;
        Invoke("restoreMaterial", 0.05f);
    }

    void restoreMaterial()
    {
        m_renderer.sharedMaterial = m_orgMaterial;
    }
}
