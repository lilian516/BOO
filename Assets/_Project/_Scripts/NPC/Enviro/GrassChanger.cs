using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassChanger : MonoBehaviour,IChangeable
{
    [SerializeField] Sprite _spriteBoo;
    [SerializeField] Sprite _spriteDarkBoo;

    private SpriteRenderer _renderer;
    public void Change()
    {

        _renderer.sprite = _spriteDarkBoo;
    }

    public void ResetChange()
    {
        _renderer.sprite = _spriteBoo;
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    private void OnDestroy()
    {
        if(AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements -= Change;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
