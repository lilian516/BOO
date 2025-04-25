using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[DefaultExecutionOrder(-2)]
public class DialogueSystem : Singleton<DialogueSystem>
{
    private const string DIALOGUE_TEXT_NAME = "SentenceText";

    private CanvasGroup _fadeCanvasBox;
    private TextMeshProUGUI _dialogueTextBox;
    private Image _leftImage;
    private Image _rightImage;
    private Button _cancelButton;
    private Button _choiceButton;
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

    public delegate void ChoiceEvent();
    public event ChoiceEvent OnChoice;

    public delegate void TakeEvent(DialogueEventType eventType);
    public event TakeEvent OnTakeEvent;

    public void Init()
    {
        ProcessingDialogue = null;
        GameObject dialogue = GameManager.Instance.DialogueUI;
        _fadeCanvasBox = dialogue.GetComponent<CanvasGroup>();
        _dialogueTextBox = dialogue.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        _isPlayingSentence = false;
        _skipSentence = false;

        _leftImage = dialogue.transform.GetChild(1).gameObject.GetComponent<Image>();
        _rightImage = dialogue.transform.GetChild(2).gameObject.GetComponent<Image>();
        _choiceButton = GameManager.Instance.DialogueSkillBtn.GetComponentInChildren<Button>();
        _cancelButton = GameManager.Instance.DialogueQuitBtn.GetComponentInChildren<Button>();

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
        InputManager.Instance.DisableControllerStick();
        InputManager.Instance.DisableSkillStick();

        ProcessingDialogue = asset;
        _sectionIndex = 0;
        _sentenceIndex = 0;

        _fadeCanvasBox.alpha = 1;
        _fadeCanvasBox.interactable = true;
        _fadeCanvasBox.blocksRaycasts = true;

        _fadeCanvasBox.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = ProcessingDialogue.DialogueBackground;
        _choiceButton.gameObject.GetComponent<Image>().sprite = ProcessingDialogue.SkillDescriptor.Sprite;


        if (_leftImage != null)
        {
            _leftImage.sprite = ProcessingDialogue.LeftCharacter;
            
        }
            

        if (_rightImage != null)
        {
            //_rightImage.sprite = ProcessingDialogue.RightCharacter;
            _rightImage.gameObject.GetComponent<Animator>().SetTrigger(asset.Name);
        }
            

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

        InputManager.Instance.EnableControllerSticks();

        Helpers.HideCanva(_fadeCanvasBox.transform.GetChild(3).GetComponent<CanvasGroup>());
        Helpers.ShowCanva(_fadeCanvasBox.transform.GetChild(4).GetComponent<CanvasGroup>());

        _fadeCanvasBox.alpha = 0;
        _fadeCanvasBox.interactable = false;
        _fadeCanvasBox.blocksRaycasts = false;

        _rightImage.gameObject.GetComponent<Animator>().SetTrigger("EndDialogue");
        _choiceButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

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
            if (ProcessingDialogue.ChoiceOnEnd)
                ShowChoice();
            else
                EndDialogue();
        }
        else
        {
            UpdateSentence();
        }
        
    }

    private void ShowChoice()
    {
        OnChoice?.Invoke();

        Helpers.ShowCanva(_fadeCanvasBox.transform.GetChild(3).GetComponent<CanvasGroup>());
        Helpers.HideCanva(_fadeCanvasBox.transform.GetChild(4).GetComponent<CanvasGroup>());

        GameManager.Instance.DialogueSkillBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ProcessingDialogue.SkillDescriptor.Name;
        GameManager.Instance.DialogueSkillBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ProcessingDialogue.SkillDescriptor.Desc;

        StartCoroutine(WaitToActivateChoice());
    }

    private IEnumerator WaitToActivateChoice()
    {
        yield return new WaitForSeconds(0.5f);
        _choiceButton.onClick.AddListener(TakeChoice);
        _cancelButton.onClick.AddListener(EndDialogue);
    }

    private void TakeChoice()
    {
        OnTakeEvent?.Invoke(ProcessingDialogue.TakeEventType);
        EndDialogue();
        GameManager.Instance.Player.GetComponent<Player>().AddSkill(ProcessingDialogue.SkillToGive, ProcessingDialogue.SkillDescriptor);
        InputManager.Instance.EnableSkillStick();

        _choiceButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
    }
    
    public void UpdateSentence()
    {
        string sentence = GetSentence();
        if(ProcessingDialogue.Name != "Seamone")
        {
            SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { ProcessingDialogue.Name+ " Dialogue One", ProcessingDialogue.Name + " Dialogue Two", ProcessingDialogue.Name + " Dialogue Three" }, 
                GameManager.Instance.Player.transform.position);
        }

        



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

