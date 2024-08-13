using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody; // Reference to the car's Rigidbody component
    public TextMeshProUGUI speedText; // Reference to the UI Text element for displaying speed

    void Update()
    {
        if (carRigidbody != null && speedText != null)
        {
            // Calculate speed in km/h
            float speedKmh = carRigidbody.velocity.magnitude * 3.6f; // 1 m/s = 3.6 km/h

            // Update UI text with speed
            speedText.text = $"Speed: {speedKmh:0} km/h";
        }
    }
}
