using System.Threading.Tasks;
using UnityEngine;
using Unityroom.Client;

public class ScoreManager : MonoBehaviour
{
    UnityroomClient _client = new()
    {
        HmacKey = Secrets.hmacKey
    };
    
    private int _score = 0;
    private int _highScore = 0;
    
    public bool AddScore()
    {
        _score ++;
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
