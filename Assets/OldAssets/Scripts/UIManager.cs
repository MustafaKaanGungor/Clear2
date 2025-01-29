using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject credits;
    private int countdownTime = 3;
    [SerializeField] private Text countdownText;
    [SerializeField] private GameManagerJam gameManager;
    private AudioManager audioManager;
    [SerializeField] private Text gameplayDeathsText;
    [SerializeField] private Text gameOverDeathsText;
    [SerializeField] private Text gameplaySavedText;
    [SerializeField] private Text gameOverSavedText;

    private void Start() {
        audioManager = AudioManager.instance;
        audioManager.PlaySound(audioManager.titleScreenMusic);
    }

    public void SetCredits() {
        credits.SetActive(!credits.activeSelf);
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void DisableMenu() {
        mainMenu.SetActive(false);
    }

    public void ActivateMenu() {
        audioManager.PlaySound(audioManager.titleScreenMusic);
        mainMenu.SetActive(true);
    }

    public void DisableGameOverMenu() {
        audioManager.StopAll();
        gameOverMenu.SetActive(false);
    }

    public void StartGame() {
        audioManager.StopAll();
        audioManager.PlaySound(audioManager.levelIntro);
        DisableMenu();
        StartCoroutine(CountdownToStart());
        gameplayUI.SetActive(true);
    }

    private IEnumerator CountdownToStart() {
        countdownTime = 3;
        countdownText.gameObject.SetActive(true);

        while(countdownTime > 0) {
            countdownText.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownText.text = "GO!";

        gameManager.StartGame();

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }


    public void GameOver() {
        gameOverDeathsText.text = gameplayDeathsText.text;
        gameOverSavedText.text = gameplaySavedText.text;
        
        gameplayUI.SetActive(false);
        gameOverMenu.SetActive(true);
    }
}
