using System.Collections;

using UnityEngine;

public class Bubble : MonoBehaviour
{

    private Vector3 _forward;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _timeMovement;
    [SerializeField] Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _forward = GameManager.Instance.Player.GetComponent<Player>().LookDir.normalized;
        StartCoroutine(BubbleMovement());
    }

    // Update is called once per frame
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
        Debug.Log("on va trigger");
        if (other.GetComponent<Player>())
        {
            AngrySystem.Instance.ChangeAngryLimits();
            Debug.Log("c'est le player");
        }

        Debug.Log(other.gameObject.name);

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(PlayerSkill.BubbleSkill);
            Debug.Log("un interact");
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
