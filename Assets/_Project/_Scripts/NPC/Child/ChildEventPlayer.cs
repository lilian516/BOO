using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimEventPlayer;

public class ChildEventPlayer : MonoBehaviour
{
    public delegate void EndWindAnim();
    public event EndWindAnim OnEndWindAnim;

    public void WindAnimInvoker()
    {
        OnEndWindAnim?.Invoke();
    }
}
