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
    
    public void AddScore()
    {
        _score ++;
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
    
    void OnDestroy()
    {
        _client.Dispose();
    }
}
