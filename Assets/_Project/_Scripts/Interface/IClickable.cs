using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable 
{
    Vector3 PositionToGo { get; set; }
    bool CanGoTo { get; set; }
    bool NeedToFaceRight { get; set; }
    void OnClick();
    void OnDestinationReached();
}
