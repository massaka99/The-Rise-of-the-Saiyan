using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider; // The health slider
    private Transform target; // The enemy to follow
    private Vector3 offset = new Vector3(0, 1, 0); // Position offset (e.g., above the enemy)

    // Updates the slider value to reflect health
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider != null)
        {
            slider.value = currentValue / maxValue; // Updates the slider's value
        }
    }

    // Assigns the health bar to an enemy to follow
    public void AssignTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Keep the health bar above the enemy's position
            transform.position = target.position + offset;
        }
    }
}
