using TMPro;
using UnityEngine;

public class GameProcessManager : MonoBehaviour
{
    
    [SerializeField] private PlayfieldController _playfieldController;

    
    private Vector2 cellSize;
    private Vector2 cellScale;

    public static GameProcessManager Instance;
    
    public PlayfieldController PlayfieldController => _playfieldController;

    void Awake()
    {
        Instance = this;
        _playfieldController.Init(this);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
