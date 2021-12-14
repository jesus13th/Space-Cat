using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
    private Image jsContainer;
    private Image joystick;
    public Vector2 axis;
    internal bool holdA = false;
    internal bool holdB = false;

    void Start() {

        jsContainer = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
        axis = Vector3.zero;
    }

    public void OnDrag(PointerEventData ped) {
        Vector2 position = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(jsContainer.rectTransform, ped.position, ped.pressEventCamera, out position);

        position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);

        axis = new Vector2(position.x * 2, 0);
        axis = (axis.magnitude > 1) ? axis.normalized : axis;

        joystick.rectTransform.anchoredPosition = new Vector3(axis.x * (jsContainer.rectTransform.sizeDelta.x / 3)
                                                               , axis.y * (jsContainer.rectTransform.sizeDelta.y) / 3);

    }

    public void OnPointerDown(PointerEventData ped) => OnDrag(ped);

    public void OnPointerUp(PointerEventData ped) {
        axis = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }
    public void HoldA() {
        if (GameManager.Instance.isPause) {
            return;
        }
        holdA = true;
        CharacterMovement.Instance._ps.Play();
        CharacterMovement.Instance.fireFX.Play();
    }
    public void ReleaseA() {
        holdA = false;
        CharacterMovement.Instance._ps.Stop();
        CharacterMovement.Instance.fireFX.Stop();
    }
    public void HoldB() => holdB = true;
    public void ReleaseB() => holdB = false;
    public void PressB() {
        if (GameManager.Instance.isPause) {
            return;
        }
        CharacterMovement.Instance.Fire();
    }
}
