using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{
    public float timer;
    public TMP_Text gameOverMsg;
    public GameObject popup;
    public GameObject gameStartMsgPopup;
    public GameObject gameOverMsgPopup;
    public Slider cubeCollectSlider;

    public int cubesCollected;
    private float currentTime;
    private bool timerStarted = false;

    private void Start()
    {
        ShowGameStartPopup();
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
        }
    }

    private void ShowGameStartPopup()
    {
        gameStartMsgPopup.SetActive(true);
        popup.SetActive(true);
        popup.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce);
    }

    public void StartGame()
    {
        popup.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameStartMsgPopup.SetActive(false);
            popup.SetActive(false);
            timerStarted = true;
        });
    }

    public void ShowGameOverPopup(int cubesCount, bool allCubesCollected)
    {
        cubesCollected = cubesCount;
        currentTime = timer;

        string gameOverMessage = allCubesCollected
            ? $"Hurray! All cubes are collected in time {currentTime:F2} seconds."
            : $"You collected {cubesCollected} cubes in {currentTime:F2} seconds.";

        gameOverMsg.text = gameOverMessage;

        // Show Game Over Popup
        gameOverMsgPopup.SetActive(true);
        popup.SetActive(true);
        popup.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
}
