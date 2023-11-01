using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    float i = 0;

    public GameObject objectToChange;

    private Renderer _renderer;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangeObjectColor);

        _renderer = objectToChange.GetComponent<Renderer>();
    }

    private void ChangeObjectColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        _renderer.material.color = randomColor;
    }

    public void SlowTime()
    {
        if (i == 0)
        {
            i = Time.timeScale;
            Time.timeScale = 0.03f;
        }
        else
        {
            Time.timeScale = i;
            i = 0;
        }
    }
}