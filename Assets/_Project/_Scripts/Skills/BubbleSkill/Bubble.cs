using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    private Vector3 _forward;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _timeMovement;
    [SerializeField] Animator _animator;
    [SerializeField] AnimationClip animationPlayerAngry;
    void Start()
    {
        _forward =  GameManager.Instance.Player.GetComponent<Player>().SkillDir.normalized;
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Bubble One", "Bubble Two", "Bubble Three", "Bubble Four", "Bubble Five", "Bubble Six" },
            transform.position);


        StartCoroutine(BubbleMovement());
    }

    void Update()
    {
        DestroyBubble();
    }

    IEnumerator BubbleMovement()
    {
        float elapsedTime = 0.1f;

        while (elapsedTime < _timeMovement)
        {
            transform.Translate(_forward * (_maxSpeed / elapsedTime) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        StartCoroutine(WaitBeforeDead());
        
    }

    IEnumerator WaitBeforeDead()
    {
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("Explosion");
    }

    private void DestroyBubble()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("A_Bubble_Explosion") && stateInfo.normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            return;
        }
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.ChangeAnimAngry(animationPlayerAngry);
            player.StateMachine.ChangeState(player.AngryState);
        }

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(PlayerSkill.BubbleSkill);
            Destroy(gameObject);
            return;
        }
        else
        {
            _forward = Vector3.zero;
            _animator.SetTrigger("Explosion");
        }

        WaitBeforeDead();
    }
}
