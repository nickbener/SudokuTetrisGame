using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Zenject;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _leaderboardText;

        private ISaveService _saveService;

        [Inject]
        public void Construct(ISaveService saveService)
        {
            _saveService = saveService;
        }

        private void Awake()
        {
            if (_leaderboardText == null)
                _leaderboardText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            RefreshText();
        }

        public void RefreshText()
        {
            if (_saveService == null || _leaderboardText == null) return;

            List<int> scores = _saveService.LoadTopScores();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                int val = (i < scores.Count) ? scores[i] : 0;
                sb.AppendLine($"{i + 1}. {val}");
            }

            _leaderboardText.text = sb.ToString().TrimEnd();
        }
}