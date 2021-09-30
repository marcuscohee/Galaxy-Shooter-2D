using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Starting_Game_UI : MonoBehaviour
{
    [SerializeField] private Text _countdownText;
    void Start()
    {
        _countdownText.gameObject.SetActive(false);
        StartCoroutine(CountdownTextRoutine());
    }

    IEnumerator CountdownTextRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        _countdownText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        _countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(2); // Game Scene
    }

}
