using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePrompt : MonoBehaviour
{
    [SerializeField] private GameObject _promptObject;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _dialogueBubble;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] [TextArea] private List<string> _dialogueLines;
    private int _currentLine = 0;

    private TextMeshProUGUI dialogueBubbleTMP;

    private bool isInDialogueRange = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueBubbleTMP = _dialogueBubble.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _playerObject)
        {
            isInDialogueRange = true;
            _promptObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _playerObject)
        {
            isInDialogueRange = false;
            _promptObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDialogueRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _currentLine = _currentLine % _dialogueLines.Count;
                dialogueBubbleTMP.SetText(_dialogueLines[_currentLine]);
                _dialogueText.SetText(_dialogueLines[_currentLine]);
                _currentLine++;
                _dialogueBubble.SetActive(true);
                _promptObject.SetActive(false);
            }
        }
        else _dialogueBubble.SetActive(false);
    }
}
