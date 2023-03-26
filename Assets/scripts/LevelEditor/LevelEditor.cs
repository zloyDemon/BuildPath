using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    [SerializeField] private LevelDataListView levelDataListView;

    private LevelEditorCell[,] cells = default;
    private List<LevelEditorCell> cellsPool = new List<LevelEditorCell>();
    private List<LevelEditorCell> leftSideCells = new List<LevelEditorCell>();
    private List<LevelEditorCell> rightSideCells = new List<LevelEditorCell>();
    private LevelEditorCell currentSelectedCell;
    private LevelEditorCell currentEnterPoint;
    private LevelEditorCell currentExitPoint;
    private CellsController cellsController;
    private int xSize;
    private int ySize;

    private CellType[,] CurrentField => levelDataListView.CurrentListItem.LevelData.Field;
    
    private void Awake()
    {
        xSizeInput.text = "1";
        ySizeInput.text = "1";

        cellsController = new CellsController();
        
        levelDataListView.InitList();
        
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
        levelDataListView.OnItemSelected += OnLevelDataListItemSelected;
        
        BuildField(1,1);
    }

    private void OnDestroy()
    {
        enterPointDraggable.OnDragging -= OnEnterPointDragging;
        exitPointDraggable.OnDragging -= OnExitPointDragging;
        cellsController.OnCheckCompleted -= CellsControllerOnOnCheckCompleted;
        levelDataListView.OnItemSelected += OnLevelDataListItemSelected;
    }

    private void OnControlCellClicked(CellType type)
    {
        if (currentSelectedCell == null)
            return;
        
        var sprite = cellsSpritesHolder.GetSpriteByName(type.ToString());
        currentSelectedCell.SetChipData(type, sprite);
    }

    private void OnLevelDataListItemSelected(ListLevelItem item)
    {
        BindFiled(item.LevelData);
    }
    
    private void OnSaveButtonClick()
    {
        cellsController.SetCells(cells, currentEnterPoint, currentExitPoint);
        cellsController.CheckGame();
        SaveField();
    }

    private void OnEnterPointDragging(PointerEventData data)
    {
        ChooseSideCell(data, ref currentEnterPoint, enterPointDraggable ,leftSideCells, Vector3.left);
        levelDataListView.CurrentListItem.LevelData.EnterPointIndex = currentEnterPoint.CellPoint.column;
    }

    private void OnExitPointDragging(PointerEventData data)
    {
        ChooseSideCell(data, ref currentExitPoint, exitPointDraggable, rightSideCells, Vector3.right);
        levelDataListView.CurrentListItem.LevelData.ExitPointIndex = currentExitPoint.CellPoint.column;
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

        if (cells != null)
        {
            foreach (var cell in cells)
            {
                cell.gameObject.SetActive(false);
                cell.gameObject.name = "cell_in_pool";
                cell.transform.SetParent(cellPool);
                cellsPool.Add(cell);
            }
        }

        var needCellsCount = cellsXCount * cellsYCount;
        
        cells = new LevelEditorCell[cellsXCount, cellsYCount];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                var cellFromPool = cellsPool.First();
                cellFromPool.gameObject.SetActive(true);
                cellFromPool.transform.SetParent(gridLayout.transform);
                cellFromPool.CellPoint = CellPoint.CreatChipPoint(i, j);
                cellFromPool.gameObject.name = $"cell_{i}_{j}";
                cells[i, j] = cellFromPool;
                cellsPool.Remove(cellFromPool);
            }
        }
        gridRectTransform.sizeDelta = new Vector2(newSizeX, newSizeY);
        
        leftSideCells.Clear();
        rightSideCells.Clear();
        
        for (int i = 0; i < cells.GetLength(1); i++)
        {
            leftSideCells.Add(cells[i, 0]);
            cells[i, 0].gameObject.name += $"_left_edge";
        }
        
        for (int i = 0; i < cells.GetLength(1); i++)
        {
            rightSideCells.Add(cells[i, cells.GetLength(1) - 1]);
            cells[i, cells.GetLength(1) - 1].gameObject.name += $"_right_edge";
        }
        
        if (leftSideCells.Count > 0)
        {
            SetEdgeCell(ref currentEnterPoint, leftSideCells.First(), enterPointDraggable, Vector3.left);
        }

        if (rightSideCells.Count > 0)
        {
            SetEdgeCell(ref currentExitPoint, rightSideCells.Last(), exitPointDraggable, Vector3.right);
        }

        levelDataListView.CurrentListItem.LevelData.Width = xSize;
        levelDataListView.CurrentListItem.LevelData.Height = ySize;
    }

    private void BindFiled(LevelData levelData)
    {
        BuildField(levelData.Width, levelData.Height);

        for (var i = 0; i < levelData.Field.GetLength(0); i++)
        {
            for (var j = 0; j < levelData.Field.GetLength(1); j++)
            {
                var cellType = levelData.Field[i, j];
                cells[i, j].SetChipData(cellType, cellsSpritesHolder.GetSpriteByName(cellType.ToString()));
            }
        }
    }

    private void SaveField()
    {
        List<LevelData> dataForSave = new List<LevelData>();
        foreach (var listLevelItem in levelDataListView.ItemList)
        {
            dataForSave.Add(listLevelItem.LevelData);
        }
        
        GameFileUtils.SaveListLevelDataArray(dataForSave);
    }
}