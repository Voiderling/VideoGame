using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour 
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
     public void updateHealthBar(float currentHealth, float maxValue)
    {
        slider.value = currentHealth / maxValue;
    }
    private void Update()
    {
        if (target != null)
        {
            transform.rotation = Camera.main.transform.rotation;
            transform.position = target.position + offset;
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                     Camera.main.transform.rotation * Vector3.up);
        }
    }
    private Vector3 _initialScale;

    void Start()
    {
        _initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        // Invert the parent's X scale
        float invertedX = Mathf.Sign(target.localScale.x);
        transform.localScale = new Vector3(_initialScale.x * invertedX, _initialScale.y, _initialScale.z);
    }
}