using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbe : MonoBehaviour, IClickable
{

    [SerializeField] SkillDescriptor _orbDescriptor;
    [SerializeField] Transform _position;
    [SerializeField] Remumus _remumus;

    private Player _player;
    public Vector3 PositionToGo { get => _position.position; set => _position.position = value; }

    
    public bool CanGoTo { get ; set ; }
    public bool NeedToFaceRight { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        PositionToGo = _position.position;
        CanGoTo = true;
        NeedToFaceRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void UseOrbe()
    //{
    //    CinematicSystem.Instance.PlayCinematic("Test");
    //    CinematicSystem.Instance.OnEndCinematic += CheckEndCinematic;
    //}

    private void CheckEndCinematic()
    {
        _remumus.HasTakenOrbFragment = true;
        //Player player = GameManager.Instance.Player.GetComponent<Player>();
        _player.StateMachine.ChangeState(_player.IdleState);

        CinematicSystem.Instance.OnEndCinematic -= CheckEndCinematic;
        GameManager.Instance.UIBlackscreen.GetComponent<CanvasGroup>().alpha = 0f;
        Destroy(gameObject);
    }

    public void OnClick()
    {
    }

    IEnumerator WaitToAddSkill()
    {
        yield return new WaitForSeconds(0.5f);
        _player.AddSkill(PlayerSkill.Orb, _orbDescriptor);
        StartCoroutine(WaitToPlayCinematic());
    }

    IEnumerator WaitToPlayCinematic()
    {

        yield return new WaitForSeconds(1f);
        GameManager.Instance.UIBlackscreen.GetComponent<CanvasGroup>().alpha = 1f;
        CinematicSystem.Instance.PlayCinematic("Remumus");
        CinematicSystem.Instance.OnEndCinematic += CheckEndCinematic;
    }

    public void OnDestinationReached()
    {
        _player = GameManager.Instance.Player.GetComponent<Player>();
        StartCoroutine(WaitToAddSkill());
    }
}
