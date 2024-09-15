using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 120f;  // 2-minute time limit
    public Text timerText;  // Reference to UI Text element

    void Update()
    {
        // Reduce time as the game progresses
        timeLimit -= Time.deltaTime;

        // Update the UI text
        timerText.text = "Time Left: " + Mathf.Ceil(timeLimit).ToString();

        // If time runs out, end the game
        if (timeLimit <= 0)
        {
            // Trigger game over
            Debug.Log("Game Over! Time's up.");
        }
    }
}
