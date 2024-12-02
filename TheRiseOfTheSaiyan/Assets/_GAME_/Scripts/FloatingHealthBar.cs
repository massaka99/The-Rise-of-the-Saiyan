using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 offset = new Vector3(0, 1, 0);
    [SerializeField] private Transform target;
    
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider != null)
        {
            slider.value = currentValue / maxValue;
        }
    }

    public void AssignTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                AssignTarget(player.transform);
            }
        }

        if (target != null && canvas != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
            transform.position = screenPos;
        }

        transform.rotation = Quaternion.identity;
    }
}
