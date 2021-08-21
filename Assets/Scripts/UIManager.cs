using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private bool _canRestart = false;
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_canRestart == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Starting_Scene");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if(currentLives == 0)
        {
            
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
            _canRestart = true;
        }

    }

    IEnumerator GameOverFlickerRoutine()

    {
        while (true)
        {
            _gameOverText.enabled = false;
            _restartText.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = true;
            _restartText.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}