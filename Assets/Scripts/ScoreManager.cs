using System.Threading.Tasks;
using UnityEngine;
using Unityroom.Client;

public class ScoreManager : MonoBehaviour
{
    UnityroomClient _client = new()
    {
        HmacKey = Secrets.hmacKey
    };
    
    public async void SendScore(int score)
    {
        var response = await _client.Scoreboards.SendAsync(new()
        {
            ScoreboardId = 1,
            Score = score
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
