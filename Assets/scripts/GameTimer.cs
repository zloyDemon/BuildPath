using System;
using System.Collections;
using UnityEngine;

public class GameTimer 
{
    public bool IsTimerWork { get; private set; }
    public int CurrentSeconds => _seconds;
    
    public event Action<int> SecondsChanged; 
    public event Action TimerEnded; 

    private int _seconds;
    private Coroutine _timerCoroutine;
    private MonoBehaviour _context;

    public void InitTimer(int seconds)
    {
        _seconds = seconds;
        
        StartTimer();
    }
    
    public void StartTimer()
    {
        IsTimerWork = true;
        _timerCoroutine = _context.StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            _context.StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    public void GetMinuteAndSeconds(out int minutes, out int seconds)
    {
        minutes = (int)Mathf.Floor(_seconds / 60);
        seconds = _seconds % 60;
    }

    private IEnumerator Timer()
    {     
        while (_seconds > 0)
        {
            _seconds--;
            SecondsChanged?.Invoke(_seconds);
            yield return new WaitForSeconds(1);
        }
        
        TimerEnded?.Invoke();
    }
}
