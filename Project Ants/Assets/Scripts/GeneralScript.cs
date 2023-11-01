using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneralScript : MonoBehaviour
{
    public int PlayerLifes;
    public int GameScore;
    static public TextMeshProUGUI _textMeshLife;
    public TextMeshProUGUI _textMeshControllLife;
    static public TextMeshProUGUI _textMeshScore;
    public TextMeshProUGUI _textMeshControllScore;
    public float spawnInterval = 0f;
    private float timer = 0f;

    void Start()
    {
        GameScore = 0;
        PlayerLifes = 3;
        _textMeshLife = _textMeshControllLife;
        _textMeshLife.text = "Players lifes: " + PlayerLifes.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            GameScore += 1;
            timer = 0f;
        }

        _textMeshControllScore.text = "Score " + GameScore.ToString();
        _textMeshLife.text = "Players lifes: " + PlayerLifes.ToString();
    }

    public void changeGui()
    {
        _textMeshLife.text = "Players lifes: " + PlayerLifes.ToString();
    }
}