using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class DebugUIInteraction : MonoBehaviour
{
    public GameObject ui_canvas;
    GraphicRaycaster ui_raycaster;

    PointerEventData click_data;
    List<RaycastResult> click_results;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += GetUiElementsClicked;
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= GetUiElementsClicked;
    }


    void Start()
    {
        ui_raycaster = ui_canvas.GetComponent<GraphicRaycaster>();
        click_data = new PointerEventData(EventSystem.current);
        click_results = new List<RaycastResult>();
    }

    void GetUiElementsClicked(Finger fin)
    {
        /** Get all the UI elements clicked, using the current mouse position and raycasting. **/

        click_data.position = fin.screenPosition;

        click_results.Clear();

        ui_raycaster.Raycast(click_data, click_results);

        foreach(RaycastResult result in click_results)
        {
            GameObject ui_element = result.gameObject;

            Debug.Log("Name: " + ui_element.name);

            //Debug.Log("-----------------------");
        }
    }
}
