using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLightsEvent : MonoBehaviour
{
    private Light[] lights; // Store all Light components in the scene
    private float minIntensity = 0.1f; // Minimum light intensity
    private float maxIntensity = 1.0f; // Maximum light intensity
    private float flickerDuration = 2.0f; // Duration of each flicker cycle

    private void Start()
    {
        lights = GameObject.FindObjectsOfType<Light>();
        StartFlickering();
    }

    private IEnumerator FlickerLights()
    {
        while (true)
        {
            foreach (Light light in lights)
            {
                // Calculate a random intensity within the min-max range
                float intensity = Random.Range(minIntensity, maxIntensity);

                // Flicker the light intensity
                light.intensity = intensity;

                // Wait for the flicker duration
                yield return new WaitForSeconds(flickerDuration);
            }
        }
    }

    private void StartFlickering()
    {
        StartCoroutine(FlickerLights());
    }
}
