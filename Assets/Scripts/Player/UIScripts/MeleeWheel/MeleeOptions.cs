using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MeleeOptions : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] string label;
    [SerializeField] bool active = false;
    [SerializeField] RadialWheelBase parentRM;
    [SerializeField] float angleOffset;
    [SerializeField] float angleMin, angleMax;
    [SerializeField] int assignedIndex = 0;
    [SerializeField] Button button;
    private CanvasGroup canvasGroup;



    #region Getters and Setters
    public RadialWheelBase ParentRM { get { return parentRM; } set { parentRM = value; } }
    public float AngleOffset { get { return angleOffset; } set { angleOffset = value; } }
    public float AngleMin { get { return angleMin; } set { angleMin = value; } }
    public float AngleMax { get { return angleMax; } set { angleMax = value; } }
    public int AssignedIndex { get { return assignedIndex; } set { assignedIndex = value; } }
    public bool Active { get { return active; } set { active = value; } }
    #endregion

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        else canvasGroup = gameObject.GetComponent<CanvasGroup>();

        if(rectTransform == null)
        {
            Debug.LogError("Radial Menu: Rect Transform for radial element " + gameObject.name + " could not be found. Please ensure this is an object parented to a canvas.");
        }
        if(button == null)
        {
            Debug.LogError("Radial Menu: No button attached to " + gameObject.name + "!");
        }
    }

    private void Start()
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, -angleOffset);
        if(parentRM.UseLazySelection)
        {
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            EventTrigger trigger;
            if(button.GetComponent<EventTrigger>() == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
                trigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
            }
            else
            {
                trigger = GetComponent<EventTrigger>();
            }
            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener((eventData) => { SetParentMenuLabel(label); });

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) => { SetParentMenuLabel(""); });
            trigger.triggers.Add(enter);
            trigger.triggers.Add(exit);
        }
    }


    public void setAllAngles(float offset, float baseOffset)
    {

        angleOffset = offset;
        angleMin = offset - (baseOffset / 2f);
        angleMax = offset + (baseOffset / 2f);

    }
    public void HighlightThisElement(PointerEventData pointerEvent)
    {

        ExecuteEvents.Execute(button.gameObject, pointerEvent, ExecuteEvents.selectHandler);
        active = true;
        SetParentMenuLabel(label);

    }
    public void UnHighlightThisElement(PointerEventData pointerEvent)
    {

        ExecuteEvents.Execute(button.gameObject, pointerEvent, ExecuteEvents.deselectHandler);
        active = false;

        if (!parentRM.UseLazySelection)
            SetParentMenuLabel(" ");

    }

    public void SetParentMenuLabel(string label)
    {

        if (parentRM.TextLabel != null)
            parentRM.TextLabel.text = label;
    }

    public void ClickMeTest()
    {
        Debug.Log(assignedIndex);
    }
}
