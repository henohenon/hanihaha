using System.Collections.Generic;
using UnityEngine;

public class ComboManager
{
    private TextMesh comboTextPrefab;
    
    private MeshCardsManager _meshCardsManager;
    private TimerManager _timerManager;
    private float lastAnswerTime = 0;
    private int comboCount = 0;
    
    private readonly float[] _comboAddTimes = new float[]
    {
        0, // 0
        0.1f,
        0.1f,
        0.2f,
        0.2f,
        0.3f, // 5
        0.3f,
        0.5f,
        0.5f,
        0.5f,
        0.6f, // 10
        0.7f,
        0.8f,
        0.9f,
        1f,
        1.1f, // 15
    };

    public bool OnAnswer(Vector3 position)
    {
        bool result = false;
        var deltaTime = Time.time - lastAnswerTime;
        if (comboCount == -1 || deltaTime < 1f)
        {
            comboCount++;
            if (comboCount != 0)
            {
                result = true;

                AddComboCard(position);
                if (comboCount < _comboAddTimes.Length)
                {
                    _timerManager.AddTime(_comboAddTimes[comboCount]);
                }
                else
                {
                    _timerManager.AddTime(0.5f);
                }
            }
        }
        else
        {
            comboCount = 0;
        }

        lastAnswerTime = Time.time;
        
        return result;
    }

    public void ResetCombo()
    {
        comboCount = -1;
    }
    
    
    public void AddComboCard(Vector3 comboPos)
    {
        var comboCardInstance = GameObject.Instantiate(comboTextPrefab, comboPos, Quaternion.identity);
        new ComboCardController(comboCount, comboCardInstance);
    }
}
