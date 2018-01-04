using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    private const int MAX_ROW_VALUE = 4;
    private const int MAX_COLUMN_VALUE = MAX_ROW_VALUE;

    [SerializeField]
    private Chip emptySprite;

    public Sprite cellSprite;
    private GameObject gameO;

    Chip chip;

    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;


    private Vector2 cellSize;
    private Vector2 cellScale;


    void Start ()
    {
        gameO = new GameObject();
        gameO.AddComponent<SpriteRenderer>().sprite = cellSprite;

        cellSize = cellSprite.bounds.size;

        Debug.Log(cellSize);

        Vector3 newCellSize = new Vector3(gridSize.x / (float)cols, gridSize.y / (float)rows);

        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize;

        gameO.transform.localScale = new Vector3(cellScale.x, cellScale.y, 0);

        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;


        for (int row = 0; row < rows; row++)
        {
            for(int col=0; col < cols; col++)
            {
                Vector3 pos = new Vector3(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                GameObject co = Instantiate(gameO) as GameObject;
                co.transform.position = pos;
                co.transform.parent = transform;

            }
          
        }
        Destroy(gameO);

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,gridSize);
    }

}
