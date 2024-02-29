using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCell : MonoBehaviour
{
    public bool AnimationEnded { get; private set; }
    private Cell _target;
    private Vector3 _translation;
    private int _moves;
    private int _nextValue;

    private void Update() {

        Move();
    }

    private void Move(){
        transform.Translate(_translation);
        _moves--;
        if (_moves == 0){
            Destroy(gameObject);
            _target.UpdateColor();
        }
    }

    private void CalculateTranslation(){
        _translation = (_target.transform.position - transform.position) / _moves;
    }

    public void SetProperties(Cell target, Color color, int nextValue){
        _target = target;
        transform.GetComponent<SpriteRenderer>().color = color;
        _moves = 100;
        _nextValue = nextValue;
        CalculateTranslation();
    }

}
