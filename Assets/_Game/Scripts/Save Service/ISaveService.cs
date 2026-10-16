using System.Collections.Generic;
using UnityEngine;

public interface ISaveService
{
    void SaveScore(int newScore);
    List<int> LoadTopScores();
}
