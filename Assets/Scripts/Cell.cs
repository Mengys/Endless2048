using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    [SerializeField]
    private Transform pfMovingCell;
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Value{ get; private set; }

    public bool IsEmpty => Value == 0;
    public bool HasMerged{ get; private set; }

    public void SetValue(int x, int y, int value){
        X = x;
        Y = y;
        Value = value > 3 ? 1 : value;
    }

    public void IncreaseValue(){
        Value = Value == 3 ? 1 : Value + 1;

        HasMerged = true;

        GameControler.Instance.AddPoints(1);
    }

    public void ResetFlags(){
        HasMerged = false;
    }

    public void MergeWithCell(Cell otherCell){

        var movingCell = Instantiate(pfMovingCell, transform, false);
        movingCell.GetComponent<MovingCell>().SetProperties(otherCell, transform.GetComponent<SpriteRenderer>().color, otherCell.Value + 1);
        SetValue(X, Y, 0);
        otherCell.IncreaseValue();
        
        UpdateColor();
    }

    public void MoveToCell(Cell target){
        var movingCell = Instantiate(pfMovingCell, transform, false);
        movingCell.GetComponent<MovingCell>().SetProperties(target, transform.GetComponent<SpriteRenderer>().color, Value);

        target.SetValue(target.X, target.Y, Value);
        SetValue(X, Y, 0);

        UpdateColor();
    }

    public void UpdateColor(){
        switch (Value)
        {
            case 0:
                transform.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
            case 1:
                transform.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 2:
                transform.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 3:
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            default:
                transform.GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }
    }
}
