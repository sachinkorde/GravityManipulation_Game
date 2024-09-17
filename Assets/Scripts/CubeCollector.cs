using UnityEngine;
using DG.Tweening;

public class CubeCollector : MonoBehaviour
{
    public int totalCubes = 5; // Set this to the total number of cubes in your game
    public UIHandler uiHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming cubes have the tag "Cube"
        {
            // Increment collected cubes
            uiHandler.cubesCollected++;

            // Update the slider with animation (every 5 cubes increases value by 1)
            if (uiHandler.cubesCollected % 5 == 0)
            {
                uiHandler.cubeCollectSlider.DOValue(uiHandler.cubeCollectSlider.value + 1, 0.5f).SetEase(Ease.OutBounce);
            }

            // If all cubes are collected, show the game-over popup
            if (uiHandler.cubesCollected >= totalCubes)
            {
                uiHandler.ShowGameOverPopup(uiHandler.cubesCollected, true); // true indicates all cubes collected
            }
            else
            {
                // Otherwise, just update the game over popup without completing all cubes
                uiHandler.ShowGameOverPopup(uiHandler.cubesCollected, false);
            }
        }
    }
}
