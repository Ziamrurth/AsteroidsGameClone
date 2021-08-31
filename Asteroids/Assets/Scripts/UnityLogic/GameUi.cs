using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUi : MonoBehaviour {
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private Text _totalScoreText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _xValue;
    [SerializeField] private Text _yValue;
    [SerializeField] private Text _rotation;
    [SerializeField] private Text _speed;
    [SerializeField] private Text _laserCharges;
    [SerializeField] private Text _laserReload;

    private UnityGameManager _unityGameManager;

    void Start()
    {
        _scoreText.text = "0";
        _gameOverMenu.SetActive(false);

        _unityGameManager = GameObject.FindObjectOfType<UnityGameManager>();

        ScoreManager.GetInstance().onScoreChangedCallback += UpdateScore;
        GameManager.GetInstance().onPlayerKilledCallback += GameOverMenu;
    }

    void Update()
    {
        _xValue.text = _unityGameManager.player.Position.X.ToString();
        _yValue.text = _unityGameManager.player.Position.Y.ToString();
        _rotation.text = _unityGameManager.player.Rotation.ToString();
        _speed.text = _unityGameManager.player.instantSpeed.ToString();
        _laserCharges.text = _unityGameManager.player.laserCharges.ToString();
        _laserReload.text = _unityGameManager.player.laserRaload.ToString();
    }

    public void Retry()
    {
        //SceneManager.LoadScene(0);
        _gameOverMenu.SetActive(false);
        UpdateScore(0);
        GameManager.GetInstance().Restart();
    }

    private void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    private void GameOverMenu()
    {
        //Time.timeScale = 0;
        _gameOverMenu.SetActive(true);
        _totalScoreText.text = ScoreManager.GetInstance().Score.ToString();
    }
}
