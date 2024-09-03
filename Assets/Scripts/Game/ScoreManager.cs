using System.Collections.Generic;
using System.Linq;
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
    
    [SerializeField] private readonly Dictionary<int, LevelDataAsset> Levels; 
    
    private int _currentLevel = 0;
    private int _score = 0;
    private int _highScore = 0;
    private NoGameUIManager _screenUIManager;
    
    public bool AddScore()
    {
        _score ++;
        if (_score >= Levels.ElementAt(_currentLevel).Key && _currentLevel < Levels.Count - 1)
        {
            _currentLevel++;
            OnLevelUpdate.OnNext(Levels[_currentLevel]);
        }
        if(_score > _highScore)
        {
            _highScore = _score;
            return true;
        }
        _screenUIManager.SetScore(_score);
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
        _screenUIManager.SetScore(_score);
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
