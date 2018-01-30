using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    public Text timerText;
    public float startTime;

    public bool isTimerWork = false;

    public void StartTime()
    {

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        isTimerWork = true;
        while (isTimerWork)
        {
            if (startTime > 0)
            {
                startTime--;
                timerText.text = "0:"+startTime.ToString("f0");
                yield return new WaitForSeconds(1);
            }
            else
            {
                isTimerWork = false;
                Debug.Log("isDone!");
            }
        }
    }
}
