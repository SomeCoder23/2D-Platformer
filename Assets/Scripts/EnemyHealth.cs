using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : HealthBar
{
    public Gradient gradient;
    public Image fill;
    public Vector3 offset = new Vector3(0, 1, 0);
    public Camera cam;

    private void Start()
    {
        if (PlayerUI.instance.singlePlayer)
            cam = Camera.main;
    }
    public void SetMaxHealth(float health)
    {
        base.SetMaxHealth(health / 100);
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        healthSlider.gameObject.SetActive(true);
        base.SetHealth(health/100);
        fill.color = gradient.Evaluate(healthSlider.normalizedValue); 
    }
    void Update()
    {
        healthSlider.transform.position = cam.WorldToScreenPoint(transform.parent.position + offset);
    }
}
