using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] AnimationClip _animationClipWater;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {

            
            if (!AngrySystem.Instance.IsAngry)
            {
                player.ChangeAnimAngry(_animationClipWater);
                player.StateMachine.ChangeState(player.AngryState);
            }


        }
    }
}
