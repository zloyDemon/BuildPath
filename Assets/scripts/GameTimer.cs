using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    public Text timerText;


    public float start_seconds;
    public bool isTimerWork { get; private set; }
    public BPManager manager { get; set; }

    public void StartTime()
    {
        start_seconds++;
        isTimerWork = true;
        StartCoroutine(Timer());
        Debug.Log("Maaath " + Mathf.Floor(9.6f));
    }

    public void PauseTimer()
    {
        if (isTimerWork)
        {
            isTimerWork = false;
            Debug.Log("Time " + start_seconds);
        }
        else
        {
            StartTime();
        }

    }

    private IEnumerator Timer()
    {     
        while (isTimerWork)
        {
            if (start_seconds > 0)
            {
                start_seconds--;
                float minutes = Mathf.Floor(start_seconds / 60);
                float seconds = start_seconds % 60;               
                timerText.text = (minutes.ToString("0") + ":"+ seconds.ToString("00"));
                yield return new WaitForSeconds(1);
            }
            else
            {
                isTimerWork = false;
                Debug.Log("isDone!");
                manager.EndGame();
                yield return null;
            }
        }
    }
}
