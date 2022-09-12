using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RadialWheelBase : MonoBehaviour
{

    [SerializeField] RectTransform rectTransform;
    [SerializeField] bool useLazySelection = true;
    [SerializeField] bool useSelectionPointer = true;
    [SerializeField] float currentAngle = 0;
    [SerializeField] Text textLabel;
    [SerializeField] List<MeleeOptions> meleeOptions = new List<MeleeOptions>();
    [SerializeField] float globalOffset = 0f;
    [SerializeField] int index = 0;
    private int elementCount;
    private float angleOffset;
    private int previousActiveIndex = 0;
    private PointerEventData pointer;
    private float inputX;
    private float inputY;
    private PlayerInput playerInput;
    private PlayerInput.OnMeleeActions onMelee;


    #region Getters and Setters
    public bool UseLazySelection { get { return useLazySelection; } set { useLazySelection = value; } }
    public Text TextLabel { get { return textLabel; } set { textLabel = value; } }
    #endregion

    private void Awake()
    {
        pointer = new PointerEventData(EventSystem.current);
        rectTransform = GetComponent<RectTransform>();
        playerInput = new PlayerInput();
        onMelee = new PlayerInput().OnMelee;
        if (rectTransform == null)
        {
            Debug.LogError("Radial Menu: Rect Transform for radial menu " + gameObject.name + " could not be found. Please ensure this is an object parented to a canvas.");
        }
        elementCount = meleeOptions.Count;
        angleOffset = (360f/(float)elementCount);

        for (int i = 0; i < elementCount; i++)
        {
            if (meleeOptions[i] == null)
            {
                Debug.LogError("Radial Menu: melee options " + i.ToString() + " in the radial menu " + gameObject.name + " is null!");
                continue;
            }
            meleeOptions[i].ParentRM = this;
            meleeOptions[i].setAllAngles((angleOffset * i) + globalOffset, angleOffset);
            meleeOptions[i].AssignedIndex = i;
        }
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(gameObject, null);//We'll make this the active object when we start it. Comment this line to set it manually from another script.
    }
    public void MeleeSelection(Vector2 input)
    {
        inputX = input.x;
        inputY = input.y;
    }

    private void Update()
    {
        float rawAngle;
        rawAngle = Mathf.Atan2(inputX, inputY) * Mathf.Rad2Deg;

        if(inputX != 0 || inputY != 0)
        {
            currentAngle = NormalizeAngle(-rawAngle + 90 - globalOffset + (angleOffset / 2f));
        }
        
        if(angleOffset != 0 && useLazySelection)
        {
            index = (int)(currentAngle / angleOffset);
        }
        if (meleeOptions[index] != null)
        {
            SelectButton(index);
        }
    }





    private void SelectButton(int i)
    {
        if (meleeOptions[i].Active == false)
        {
            meleeOptions[i].HighlightThisElement(pointer);
            if(previousActiveIndex != i)
            {
                meleeOptions[previousActiveIndex].UnHighlightThisElement(pointer);
            }
        }
        previousActiveIndex = i;
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if(angle < 0)
        {
            angle += 360f;
        }
        return angle;
    }

    public void SelectMeleeStrike()
    {
        ExecuteEvents.Execute(meleeOptions[index].MeleeButton.gameObject, pointer, ExecuteEvents.submitHandler);
    }
}
