using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Text _youWinText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _waveDisplay;
    [SerializeField] private Text _laserAmmoCountText;
    [SerializeField] private RectTransform _thrusterScalingBar;
    [SerializeField] private RectTransform _bossHealthBar;
    [SerializeField] private Text _bossHealthText;
    [SerializeField] private GameManager _gameManager;
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("The Game_Manager is NULL");
        }
    }

    public void LaserCount(int _laserCount)
    {
        _laserAmmoCountText.text = _laserCount + "/15";
        //When lasers are used, then display it on screen using current/max form.
    }

    public void ThrusterGauge(float thrusterLimit)
    {
        _thrusterScalingBar.localScale = new Vector3((thrusterLimit * 0.01f), 1, 1);
        //This is changing the scale of the bar.
            //Since we used 100 for the _thrusterLimit, you will need to convert it here.
        if(thrusterLimit <= 25)
        {
            _thrusterScalingBar.GetComponent<Image>().color = Color.red;
        }
        else
        {
            _thrusterScalingBar.GetComponent<Image>().color = Color.blue;
        }
    }

    public void BossWave()
    {
        _bossHealthBar.gameObject.SetActive(true);
        _bossHealthText.gameObject.SetActive(true);
    }

    public void BossHealth(float bossHealth)
    {
        _bossHealthBar.localScale = new Vector3((bossHealth * 0.01f * 2), 1, 1);

        if(bossHealth <= 0)
        {
            _bossHealthBar.gameObject.SetActive(false);
            _bossHealthText.gameObject.SetActive(false);
            YouWin();
            _gameManager.GameOver();
        }
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];
        if(currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    public void WaveDisplay(int wave)
    {
        _waveDisplay.text = "WAVE: " + wave;
        if(wave == 11)
        {
            _waveDisplay.text = "!!!BOSS INCOMING!!!";
        }
        StartCoroutine(WaveDisplayRoutine());
    }

    IEnumerator WaveDisplayRoutine()
    {
        _waveDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        _waveDisplay.gameObject.SetActive(false);
    }

    public void YouWin()
    {
        _restartText.gameObject.SetActive(true);
        _youWinText.gameObject.SetActive(true);
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _gameManager.GameOver();
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