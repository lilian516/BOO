using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;


[DefaultExecutionOrder(-2)]
public class DialogueSystem : Singleton<DialogueSystem>
{
    private const string DIALOGUE_TEXT_NAME = "SentenceText";

    
    
    
    private CanvasGroup _fadeCanvasBox;
    private TextMeshProUGUI _dialogueTextBox;
    public DialogueAsset ProcessingDialogue { get; private set; }

    private int _sectionIndex;
    private int _sentenceIndex;

    [SerializeField] float timeBtwnChars;
    private bool _isPlayingSentence;
    private bool _skipSentence;

    public delegate void DialogueEvent(DialogueEventType eventType);
    public event DialogueEvent OnDialogueEvent;

    public delegate void EndDialogueEvent();
    public event EndDialogueEvent OnEndDialogue;
    public void Init()
    {
        ProcessingDialogue = null;
        _fadeCanvasBox = GameManager.Instance.DialogueUI.GetComponent<CanvasGroup>();
        _dialogueTextBox = _fadeCanvasBox.transform.Find("TextDialogue").GetComponent<TextMeshProUGUI>();
        _isPlayingSentence = false;
        _skipSentence = false;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable() { 
       
    }

    public void BeginDialogue(DialogueAsset asset)
    {
        if (asset == null)
        {
            Debug.LogWarning("Dialogue started with unspecified or invalid Dialogue Asset !");
            return;
        }
        ProcessingDialogue = asset;
        _sectionIndex = 0;
        _sentenceIndex = 0;

        _fadeCanvasBox.alpha = 1;
        _fadeCanvasBox.interactable = true;
        _fadeCanvasBox.blocksRaycasts = true;

        if (ProcessingDialogue.OpeningTriggerEvent)
        {
            OnDialogueEvent?.Invoke(ProcessingDialogue.OpeningEventType);
        }

        UpdateSentence();
    }

    public void EndDialogue()
    {
        if (ProcessingDialogue.ClosureTriggerEvent)
        {
            OnDialogueEvent?.Invoke(ProcessingDialogue.ClosureEventType);
        }

        _fadeCanvasBox.alpha = 0;
        _fadeCanvasBox.interactable = false;
        _fadeCanvasBox.blocksRaycasts = false;

        OnEndDialogue?.Invoke();
    }

    public void AdvanceDialogue()
    {
        if(_skipSentence || !_isPlayingSentence)
        {
            _sentenceIndex++;
        }
        if (_sentenceIndex >= GetSentenceCount())
        {
            UpdateSection();
        }
        else
        {
            UpdateSentence();
        }
            
    }

    private void UpdateSection()
    {
        _sentenceIndex = 0;
        DialogueSection section = GetSection();
        if (section.TriggerEvent)
        {
            OnDialogueEvent?.Invoke(section.EventType);
        }
            
        _sectionIndex++;
        if(_sectionIndex >= GetSectionCount())
        {
            EndDialogue();
        }
        else
        {
            UpdateSentence();
        }
        
    }
    public void UpdateSentence()
    {
        string sentence = GetSentence();
        

        if (!_isPlayingSentence)
        {
            StartCoroutine(ShowProgressiveText(sentence));
        }
        else
        {
            _skipSentence = true;
        }
    }


    private IEnumerator ShowProgressiveText(string sentence)
    {
        _dialogueTextBox.text = sentence;
        int totalVisibleCharacters = sentence.Length;
        int counter = 0;
        _isPlayingSentence = true;
        _skipSentence = false;

        while (_isPlayingSentence)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _dialogueTextBox.maxVisibleCharacters = visibleCount;

            counter++;
            if (!_skipSentence)
            {
                yield return new WaitForSeconds(timeBtwnChars);
            }

            if (visibleCount >= totalVisibleCharacters)
            {
                _isPlayingSentence=false;
                break;
            }
        }
    }

    private int GetSectionCount() => ProcessingDialogue.Sections.Length;
    private int GetSentenceCount() => ProcessingDialogue.Sections[_sectionIndex].Sentences.Length;
    private DialogueSection GetSection() => ProcessingDialogue.Sections[_sectionIndex];
    private string GetSentence() => GetSection().Sentences[_sentenceIndex];
}

