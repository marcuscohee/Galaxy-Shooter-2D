using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _laserAmmoCountText;
    [SerializeField] private Text _droneAmmoCountText;
    [SerializeField] private RectTransform _thrusterScalingBar;
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

    public void DroneCount(int _droneCount)
    {
        _droneAmmoCountText.text = _droneCount + "/4";
        //When drones are used, then display it on screen using current/max form.
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