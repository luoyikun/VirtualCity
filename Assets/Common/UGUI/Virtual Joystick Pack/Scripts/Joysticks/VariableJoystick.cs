using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    //public GameObject CharacterObj;
    [Header("Variable Joystick Options")]
    public bool isFixed = true;
    public Vector2 fixedScreenPosition;
    Vector2 joystickCenter = Vector2.zero;

    Vector3 m_oriPos;
    void Start()
    {
        if (isFixed)
            OnFixed();
        else
            OnFloat();
        m_oriPos = background.transform.localPosition;
    }

    public void ChangeFixed(bool joystickFixed)
    {
        if (joystickFixed)
            OnFixed();
        else
            OnFloat();
        isFixed = joystickFixed;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (!isFixed)
        {
            background.gameObject.SetActive(true);
            background.position = eventData.position;
            handle.anchoredPosition = Vector2.zero;
            joystickCenter = eventData.position;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //if (!isFixed)
        //{
        //    background.gameObject.SetActive(false);
        //}
        base.OnPointerUp(eventData);
        Reset();


    }

    public void Reset()
    {
        inputVector = Vector2.zero;
        background.transform.localPosition = m_oriPos;
        handle.anchoredPosition = Vector2.zero;
    }
    void OnFixed()
    {
        joystickCenter = fixedScreenPosition;
        background.gameObject.SetActive(true);
        handle.anchoredPosition = Vector2.zero;
        background.anchoredPosition = fixedScreenPosition;
    }

    void OnFloat()
    {
        handle.anchoredPosition = Vector2.zero;
        //background.gameObject.SetActive(false);
    }
}

//测试用
//public class Player3DExampleTest : MonoBehaviour
//{

//    public float moveSpeed = 8f;
//    public Joystick joystick;

//    void Update()
//    {
//        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

//        if (moveVector != Vector3.zero)
//        {
//            transform.rotation = Quaternion.LookRotation(moveVector);
//            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
//        }
//    }
//}