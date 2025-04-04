using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseOrbe()
    {
        CinematicSystem.Instance.PlayCinematic("Test");
        CinematicSystem.Instance.OnEndCinematic += CheckEndCinematic;
    }

    private void CheckEndCinematic()
    {
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        player.StateMachine.ChangeState(player.IdleState);

        CinematicSystem.Instance.OnEndCinematic -= CheckEndCinematic;
    }
}
