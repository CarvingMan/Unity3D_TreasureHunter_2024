using UnityEngine;

/*
 * Description: A simple script that allows light sources to flicker like firelight, great for torches, candles, and other flame in your scenes.
 * Author: Sarrah Wilkinson - Red Nebula Studios
 * Created Date: 5/21/2022
 * Last Modified Date: 5/21/2022
 * Copyright: (c) 2022 Red Nebula Studios - Included with the RNLowPolyDungeon set for royalty free use by purchasers for finished games and other Unity projects.
 * Not for individual resale or redistribution.
 * Questions? Use the contact form at http://games.rednebulastudios.com/ to get in touch!
 */

namespace RNLowPolyDungeon
{
    public class LightFlicker : MonoBehaviour
    {
        [Tooltip("The speed (in seconds) at which the light source flickers.")]
        [SerializeField] float flickerSpeed = 0.3f;
        [Tooltip("How much the range varies from the light's default range value. For example, if the initial range is 2 and the range variation is 1, the light " +
            "will flicker between 1 and 3.")]
        [SerializeField] float rangeVariation = 1f;

        Light flickerLight;
        float minRange, maxRange;
        float prevRange, targetRange, timer = 0f;

        void Start()
        {
            // Retrieves the Light component from the game object at runtime and throws a warning if it cannot be located.
            flickerLight = GetComponent<Light>();
            if (!flickerLight)
                Debug.LogWarning("LightFlicker script can not find a Light component on object: " + gameObject.name);

            // Set the minimum and maximum range based on the light's default range value
            minRange = flickerLight.range - rangeVariation;
            maxRange = flickerLight.range + rangeVariation;

            // Sets the initial values
            RandomizeRange(flickerLight.range);
        }

        void Update()
        {
            if (flickerLight)
            {
                // Uses linear interpolation (Lerp) to go between the previous light range and the target range.
                flickerLight.range = Mathf.Lerp(prevRange, targetRange, timer / flickerSpeed);
                
                // Counts the timer up
                timer += Time.deltaTime;

                // If we've reached the flickerSpeed value, reset and re-randomize to a new target value, and start again.
                if (timer > flickerSpeed)
                {
                    timer = 0;
                    RandomizeRange(targetRange);
                }
            }
        }

        /// <summary>
        /// Sets the previous range value the linear interpolation starts from, and randomizes a new target range.
        /// </summary>
        /// <param name="previousRange">The previous range value to start from.</param>
        void RandomizeRange(float previousRange)
        {
            prevRange = previousRange;
            targetRange = Random.Range(minRange, maxRange);
        }
    }
}

