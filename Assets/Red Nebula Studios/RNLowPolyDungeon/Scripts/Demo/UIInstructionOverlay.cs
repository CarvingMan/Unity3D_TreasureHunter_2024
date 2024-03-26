using UnityEngine;

/*
 * Description: UI script that controls aspects of the demo scenes in the RNLowPolyDungeon set. Its purpose is simply to provide a centralized location for other scripts to
 * retrieve the cursor texture that indicates activatable props in the demo scenes, and to toggle the ceiling in the demo build.
 * Author: Sarrah Wilkinson - Red Nebula Studios
 * Created Date: 5/28/2022
 * Last Modified Date: 5/28/2022
 * Copyright: (c) 2022 Red Nebula Studios - Included with the RNLowPolyDungeon set for royalty free use by purchasers for finished games and other Unity projects.
 * Not for individual resale or redistribution.
 * Questions? Use the contact form at http://games.rednebulastudios.com/ to get in touch!
 */

namespace RNLowPolyDungeon
{
    public class UIInstructionOverlay : MonoBehaviour
    {
        public Texture2D cursorTexture;
        public Vector2 cursorTarget;

        public Camera mainCamera = null;
        public GameObject ceilings = null;

        bool ceilingsDisplayed = true;

        // Sets up a static singleton so public fields in this script are accessible by other scripts in the demo scenes.
        public static UIInstructionOverlay Instance { get; private set; }
        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        /// <summary>
        /// Toggles the ceiling objects in the demo scene.
        /// </summary>
        public void DisplayCeilings()
        {
            ceilingsDisplayed = !ceilingsDisplayed;

            ceilings.SetActive(ceilingsDisplayed);
            mainCamera.useOcclusionCulling = ceilingsDisplayed;
        }
    }
}