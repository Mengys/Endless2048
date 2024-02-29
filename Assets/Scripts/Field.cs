using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Field Properties")]
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int StartCellsCount;

    [Space(10)]
    [SerializeField]
    private Cell pfCell;

    private Cell[,] field;

    private bool anyCellMoved;

    private void Awake() {
        if (Instance == null){
            Instance = this;
        }
    }

    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
#endif
    }

    private void OnInput(Vector2 direction){
        if (!GameControler.GameStarted)
            return;

        anyCellMoved = false;
        ResetCellsFlags();

        Move(direction);

        if (anyCellMoved){
            GenerateCell();

        }
    }

    private void Move(Vector2 direction){
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++){
            for (int j = startXY; j >= 0 && j < FieldSize; j -= dir){
                var cell = direction.x != 0 ? field[j, i] : field[i,j];
                
                if (cell.IsEmpty)
                    continue;

                var cellToMerge = FindCellToMerge(cell, direction);
                if (cellToMerge != null){
                    cell.MergeWithCell(cellToMerge);
                    anyCellMoved = true;
                    continue;
                }

                var emptyCell = FindEmptyCell(cell, direction);
                if (emptyCell != null){
                    cell.MoveToCell(emptyCell);
                    anyCellMoved = true;
                }
            }
        }
    }

    private Cell FindCellToMerge(Cell cell, Vector2 direction){
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y){

            if (field[x,y].IsEmpty)
                continue;
            
            if (field[x,y].Value == cell.Value && !field[x,y].HasMerged)
                return field[x, y];

            break;
        }

        return null;
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction){
        Cell emptyCell = null;
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y){
            
            if (field[x,y].IsEmpty)
                emptyCell = field[x, y];
            else
                break;
        }

        return emptyCell;
    }

    private void Start() {
        GenerateField();
        for (int i = 0; i < StartCellsCount; i++){
            GenerateCell();
        }
    }

    private void CreateField(){
        field = new Cell[FieldSize, FieldSize];

        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        transform.GetComponent<SpriteRenderer>().size = new Vector2(fieldWidth, fieldWidth);

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++){
            for (int y = 0; y < FieldSize; y++){
                var cell = Instantiate(pfCell, transform, false);
                var position = new Vector3(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.position = position;
                cell.transform.GetComponent<SpriteRenderer>().size = new Vector2(CellSize, CellSize);

                field[x, y] = cell;

                cell.SetValue(x, y, 0);
                cell.UpdateColor();
            }
        }
    }

    public void GenerateField(){
        if (field == null){
            CreateField();
        }

        for (int x = 0; x < FieldSize; x++){
            for (int y = 0; y < FieldSize; y++){
                field[x, y].SetValue(x, y, 0);
                field[x, y].UpdateColor();
            }
        }
    }

    private void GenerateCell(){
        List<Cell> emptyCells = new List<Cell>();
        for (int x = 0; x < FieldSize; x++){
            for (int y = 0; y < FieldSize; y++){
                if (field[x,y].IsEmpty){
                    emptyCells.Add(field[x, y]);
                }
            }
        }
        if (emptyCells.Count == 0)
            throw new System.Exception("There is no any empty cells.");

        var cell = emptyCells[Random.Range(0, emptyCells.Count)];
        cell.SetValue(cell.X, cell.Y, 1);
        cell.UpdateColor();
    }

    private void ResetCellsFlags(){
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }
}
