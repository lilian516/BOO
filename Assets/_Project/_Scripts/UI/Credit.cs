using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    private CanvasGroup _canvasGroupCredit;
    [SerializeField] CanvasGroup _canvasMainMenuGroup;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] float _scrollSpeed;
    [SerializeField] float _endPosition;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroupCredit = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScrollCredit()
    {
        while (_rectTransform.anchoredPosition.y < _endPosition)
        {
            _rectTransform.anchoredPosition += new Vector2(0, _scrollSpeed * Time.deltaTime);
            yield return null;
        }
        CloseCredit();
    }

    public void OpenCredit()
    {
        Helpers.ShowCanva(_canvasGroupCredit);
        StartCoroutine(ScrollCredit());
    }

    public void CloseCredit() {
        Helpers.HideCanva(_canvasGroupCredit);
        Helpers.ShowCanva(_canvasMainMenuGroup);
        _rectTransform.anchoredPosition = Vector2.zero;
    }
}
