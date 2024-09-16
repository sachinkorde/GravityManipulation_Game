using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 120f;
    public Text timerText;

    void Update()
    {
        timeLimit -= Time.deltaTime;

        timerText.text = "Time Left: " + Mathf.Ceil(timeLimit).ToString();

        if (timeLimit <= 0)
        {
            Debug.Log("Game Over! Time's up.");
        }
    }
}
