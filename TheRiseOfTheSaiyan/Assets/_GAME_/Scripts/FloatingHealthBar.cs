using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Vector3 offset = new Vector3(0, 1, 0);
    [SerializeField] private Transform target;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    public void AssignTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }

        transform.rotation = Quaternion.identity;
    }
}
