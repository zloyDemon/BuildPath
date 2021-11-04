using System;
using System.Linq;
using UnityEngine;


public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer energyPath;

    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private float width = 0.1f;
    
    private void Awake()
    {
        BPManager.Instance.ChipController.OnCheckCompleted += ChipControllerOnOnCheckCompleted;
        energyPath.startWidth = width;
        energyPath.endWidth = width;
        energyPath.widthMultiplier = width;
    }

    private void OnDestroy()
    {
        if (BPManager.Instance == null) return;
        BPManager.Instance.ChipController.OnCheckCompleted += ChipControllerOnOnCheckCompleted;
    }

    private void ChipControllerOnOnCheckCompleted()
    {
        var rightChips = BPManager.Instance.ChipController.RightChips;
        energyPath.positionCount = BPManager.Instance.ChipController.RightChips.Count() + 1;
        int position = 0;

        var enterPoint = BPManager.Instance.PlayfieldController.EnterPoint;
        var exitPoint = BPManager.Instance.PlayfieldController.ExitPoint;

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
