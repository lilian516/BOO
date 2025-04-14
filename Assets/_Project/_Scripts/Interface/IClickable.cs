using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable 
{
    Vector3 PositionToGo { get; set; }
    void OnClick();
}
