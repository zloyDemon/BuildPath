using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private TMP_InputField xSizeInput;
    [SerializeField] private TMP_InputField ySizeInput;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform cellPool;
    [SerializeField] private RectTransform controlCellsContainer;
    [SerializeField] private SpriteHolder cellsSpritesHolder;
    [SerializeField] private DraggableElement enterPointDraggable;
    [SerializeField] private DraggableElement exitPointDraggable;
    [SerializeField] private Button saveButton;

    private List<LevelEditorCell> cells = new List<LevelEditorCell>();
    private LevelEditorCell[,] cells_array;
    private List<LevelEditorCell> cellsPool = new List<LevelEditorCell>();
    private List<LevelEditorCell> leftSideCells = new List<LevelEditorCell>();
    private List<LevelEditorCell> rightSideCells = new List<LevelEditorCell>();
    private LevelEditorCell currentSelectedCell;
    private LevelEditorCell currentEnterPoint;
    private LevelEditorCell currentExitPoint;
    private CellsController cellsController;
    private int xSize;
    private int ySize;
    

    private void Awake()
    {
        xSizeInput.text = "1";
        ySizeInput.text = "1";

        cellsController = new CellsController();
        
        for (int i = 0; i < cellPool.childCount; i++)
        {
            cellsPool.Add(cellPool.GetChild(i).GetComponent<LevelEditorCell>());
            cellPool.GetChild(i).GetComponent<LevelEditorCell>().SetClickListener(OnCellClick);
        }

        for (int i = 0; i < controlCellsContainer.childCount; i++)
        {
            controlCellsContainer.GetChild(i).GetComponent<LevelEditorButtonCell>().SetClickListener(OnControlCellClicked);
        }

        xSizeInput.onValueChanged.AddListener(OnCellCountChange);
        ySizeInput.onValueChanged.AddListener(OnCellCountChange);
        saveButton.onClick.AddListener(OnSaveButtonClick);

        enterPointDraggable.OnDragging += OnEnterPointDragging;
        exitPointDraggable.OnDragging += OnExitPointDragging;
        cellsController.OnCheckCompleted += CellsControllerOnOnCheckCompleted;
        
        BuildField(1,1);
    }

    private void OnDestroy()
    {
        enterPointDraggable.OnDragging -= OnEnterPointDragging;
        exitPointDraggable.OnDragging -= OnExitPointDragging;
        cellsController.OnCheckCompleted -= CellsControllerOnOnCheckCompleted;
    }

    private void OnControlCellClicked(CellType type)
    {
        if (currentSelectedCell == null)
            return;
        
        var sprite = cellsSpritesHolder.GetSpriteByName(type.ToString());
        currentSelectedCell.SetChipData(type, sprite);
    }

    private void OnSaveButtonClick()
    {
        TransformListToDoubleArray();
        cellsController.SetCells(cells_array, currentEnterPoint, currentExitPoint);
        cellsController.CheckGame();
    }

    private void OnEnterPointDragging(PointerEventData data)
    {
        ChooseSideCell(data, ref currentEnterPoint, enterPointDraggable ,leftSideCells, Vector3.left);
    }

    private void OnExitPointDragging(PointerEventData data)
    {
        ChooseSideCell(data, ref currentExitPoint, exitPointDraggable, rightSideCells, Vector3.right);
    }

    private void SetEdgeCell(ref LevelEditorCell currentCell, LevelEditorCell cellFrom, DraggableElement sideMarkElement, Vector3 direction)
    {
        currentCell = cellFrom;
        sideMarkElement.transform.position = cellFrom.transform.position + direction * cellFrom.RectTransform.rect.width * 2;
    }

    private void ChooseSideCell(PointerEventData data, ref LevelEditorCell currenSideCell, DraggableElement sideMarkElement, List<LevelEditorCell> cellsList, Vector3 markDirection)
    {
        if (currenSideCell == null)
        {
            return;
        }
        
        foreach (var cell in cellsList)
        {
            var distance = data.position.y - cell.transform.position.y;
            if (Mathf.Abs(distance) < cell.RectTransform.rect.height)
            {
                SetEdgeCell(ref currenSideCell, cell, sideMarkElement, markDirection);
            }
        } 
    }
    
    private void CellsControllerOnOnCheckCompleted(CellsController.CheckStatus status)
    {
        Debug.Log($"Status check {status}");
    }
    
    private void WriteToFile()
    {
        File.WriteAllText("Assets/Resources/test.txt", "Test");
        var t = File.ReadAllText("Assets/Resources/test.txt");
    }

    private void OnCellClick(LevelEditorCell cell)
    {
        if (currentSelectedCell != null)
        {
            currentSelectedCell.GetComponent<Image>().color = Color.white;
            currentSelectedCell = null;
        }

        currentSelectedCell = cell;
        currentSelectedCell.GetComponent<Image>().color = Color.yellow;
    }

    private void OnCellCountChange(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        
        int cellsXCount = int.Parse(xSizeInput.text, CultureInfo.InvariantCulture.NumberFormat);
        int cellsYCount = int.Parse(ySizeInput.text, CultureInfo.InvariantCulture.NumberFormat);

        BuildField(cellsXCount, cellsYCount);
    }

    private void BuildField(int cellsXCount, int cellsYCount)
    {
        xSize = cellsXCount;
        ySize = cellsYCount;
        
        var gridRectTransform = (RectTransform) gridLayout.transform;
        gridRectTransform.sizeDelta = gridLayout.cellSize;
        
        var newSizeX = gridRectTransform.sizeDelta.x * cellsXCount + Mathf.Clamp(gridLayout.spacing.x * (cellsXCount - 1), 0, 100);
        var newSizeY = gridRectTransform.sizeDelta.y * cellsYCount + Mathf.Clamp(gridLayout.spacing.y * (cellsYCount - 1), 0, 100);

        foreach (var cell in cells)
        {
            cell.gameObject.SetActive(false);
            cell.transform.SetParent(cellPool);
            cellsPool.Add(cell);
        }
        
        cells.Clear();

        var needCellsCount = cellsXCount * cellsYCount;

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                //cells_array[i,j] = 
            }
        }
        
        for (int i = 0; i < needCellsCount; i++)
        {
            var cellFromPool = cellsPool.First();
            cellFromPool.gameObject.SetActive(true);
            cellFromPool.transform.SetParent(gridLayout.transform);
            cells.Add(cellFromPool);
            cellsPool.Remove(cellFromPool);
        }
        
        leftSideCells.Clear();
        rightSideCells.Clear();
        
        for (int i = 0; i < cells.Count; i += xSize)
        {
            leftSideCells.Add(cells[i]);
        }
        
        for (int i = xSize - 1; i < needCellsCount; i += (xSize))
        {
            rightSideCells.Add(cells[i]);
        }

        gridRectTransform.sizeDelta = new Vector2(newSizeX, newSizeY);
        
        Canvas.ForceUpdateCanvases();
        
        if (leftSideCells.Count > 0)
        {
            SetEdgeCell(ref currentEnterPoint, leftSideCells.First(), enterPointDraggable, Vector3.left);
        }

        if (rightSideCells.Count > 0)
        {
            SetEdgeCell(ref currentExitPoint, rightSideCells.Last(), exitPointDraggable, Vector3.right);
        }
    }

    private void TransformListToDoubleArray()
    {
        int step = 0;
        cells_array = null;
        cells_array = new LevelEditorCell[xSize, ySize];
        string str = "";
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                cells[step].CellPoint = CellPoint.CreatChipPoint(j, (ySize - 1) - i);
                cells[step].SetP();
                cells[step].IsOnWay = false;
                cells_array[i, j] = cells[step];
                step++;
                step = Mathf.Clamp(step, 0, cells.Count - 1);
                str += cells_array[i,j].name +":";
            }

            str += "\n";
        }
        
        Debug.LogError(str);
    }
}