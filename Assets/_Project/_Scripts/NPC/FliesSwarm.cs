using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FliesSwarm : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem _flies;
    private ParticleSystem.Particle[] _particles;
    [SerializeField] AnimationClip _animationPlayerAngry;
    private bool _isEnabled;
    public GameObject EldenFlyPrefab;

    private void Start()
    {
        _particles = new ParticleSystem.Particle[_flies.main.maxParticles];
        _isEnabled = true;
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.PantsSkill:
                StartCoroutine(SlapFlies());
                break;

            case PlayerSkill.SmashSkill:
                AngrySystem.Instance.ChangeCalmLimits();
                StartCoroutine(SlapFlies());
                break;
        }
            
    }

    private IEnumerator SlapFlies()
    {
        _isEnabled = false;
        _flies.GetParticles(_particles);

        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].velocity = Random.onUnitSphere * 15.0f;
        }

        _flies.SetParticles(_particles);
        _flies.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        float duration = 3f;
        float timer = 0f;

        while (timer < duration)
        {
            int numAlive = _flies.GetParticles(_particles);
            float fadePercent = timer / duration;

            for (int i = 0; i < numAlive; i++)
            {
                Color32 color = _particles[i].startColor;
                color.a = (byte)(255 * (1f - fadePercent));
                _particles[i].startColor = color;
            }

            _flies.SetParticles(_particles, numAlive);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);

        GameManager.Instance.KilledFly++;

        if (GameManager.Instance.KilledFly >= 4)
        {
            GameObject newObject = Instantiate(EldenFlyPrefab, GameManager.Instance.Player.transform.position + new Vector3(10.0f, 0.0f, 0.0f), EldenFlyPrefab.transform.rotation);
            SceneManager.MoveGameObjectToScene(newObject, SceneManager.GetSceneByName("MainScene"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null) {
            if (!AngrySystem.Instance.IsAngry && _isEnabled)
            {
                player.ChangeAnimAngry(_animationPlayerAngry);
                player.StateMachine.ChangeState(player.AngryState);
            }

            
        }
    }
}
