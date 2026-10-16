using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public void UpdateScore(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score:    {score}";
        }
    }
}
