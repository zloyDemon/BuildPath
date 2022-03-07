using System;
using System.Linq;
using UnityEngine;


public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer energyPath;

    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private float width = 0.01f;
    
    private void Awake()
    {
        GameProcessManager.Instance.PlayfieldController.OnCheckCompleted += ChipControllerOnOnCheckCompleted;
        energyPath.startWidth = width;
        energyPath.endWidth = width;
        energyPath.widthMultiplier = width;
    }

    private void OnDestroy()
    {
        if (GameProcessManager.Instance == null) 
            return;
        
        GameProcessManager.Instance.PlayfieldController.OnCheckCompleted += ChipControllerOnOnCheckCompleted;
    }

    private void ChipControllerOnOnCheckCompleted()
    {
        var rightChips = GameProcessManager.Instance.PlayfieldController.RightChips;
        energyPath.positionCount = GameProcessManager.Instance.PlayfieldController.RightChips.Count() + 1;
        int position = 0;

        var enterPoint = GameProcessManager.Instance.PlayfieldController.EnterPoint;
        var exitPoint = GameProcessManager.Instance.PlayfieldController.ExitPoint;

        foreach (var rightChip in rightChips)
        {
            if (enterPoint == rightChip)
            {
                energyPath.SetPosition(position++, rightChip.transform.position + Vector3.left * 2);
            }

            energyPath.SetPosition(position, rightChip.transform.position);
            position++;
            
            if (rightChip == exitPoint)
            {
                energyPath.positionCount++;
                energyPath.SetPosition(position, rightChip.transform.position + Vector3.right * 2);
            }
        }
        
        energyPath.startWidth = width;
        energyPath.endWidth = width;
        energyPath.widthMultiplier = width;
    }


}
