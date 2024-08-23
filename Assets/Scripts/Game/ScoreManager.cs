using System.Threading.Tasks;
using R3;
using UnityEngine;
using Unityroom.Client;

public class ScoreManager : MonoBehaviour
{
    UnityroomClient _client = new()
    {
        HmacKey = Secrets.hmacKey
    };
    
    private int _currentLevel = 0;
    private static DifficultyLevel[] Levels = new[]
    {
        new DifficultyLevel(0, 5, 1, 5, 3),
        new DifficultyLevel(3, 4, 1.5f, 9, 4),
        new DifficultyLevel(10, 3, 1.5f, 12, 6),
        new DifficultyLevel(15, 2, 2f, 16, 9),
        new DifficultyLevel(30, 1, 2f, 20, 13)
    };
    
    private int _score = 0;
    private int _highScore = 0;
    
    public Subject<DifficultyLevel> OnLevelUpdate = new();
    
    public bool AddScore()
    {
        _score ++;
        if (_score >= Levels[_currentLevel].levelScore && _currentLevel < Levels.Length - 1)
        {
            _currentLevel++;
            OnLevelUpdate.OnNext(Levels[_currentLevel]);
        }
        if(_score > _highScore)
        {
            _highScore = _score;
            return true;
        }
        return false;
    }
    
    public async void SendScore()
    {
        var response = await _client.Scoreboards.SendAsync(new()
        {
            ScoreboardId = 1,
            Score = _score
        });

        if (response.ScoreUpdated)
        {
            Debug.Log("Score updated!");
        }
    }
    
    public void ResetScore()
    {
        _score = 0;
        _currentLevel = 0;
        OnLevelUpdate.OnNext(Levels[_currentLevel]);
    }
    
    public int GetScore()
    {
        return _score;
    }
    
    void OnDestroy()
    {
        _client.Dispose();
    }
}


public class DifficultyLevel
{
    public int levelScore;
    public float eachPlusTime;
    public float eachMinusTime;
    public int maxCardNum;
    public float minCardNum;
    
    public DifficultyLevel(int levelScore, float eachPlusTime, float eachMinusTime, int maxCardNum, float minCardNum)
    {
        this.levelScore = levelScore;
        this.eachPlusTime = eachPlusTime;
        this.eachMinusTime = eachMinusTime;
        this.maxCardNum = maxCardNum;
        this.minCardNum = minCardNum;
    }
}
