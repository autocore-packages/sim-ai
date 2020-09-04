#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.simai
{
    public class LogicButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler
    {
        public ElementObject obj;
        public void SetButtonObj(ElementObject elementObject)
        {
            obj = elementObject;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (obj != null && !ElementsManager.Instance.IsInEdit  && eventData.button == PointerEventData.InputButton.Left)
            {
                ElementsManager.Instance.SelectedElement = obj;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (obj != null && !ElementsManager.Instance.IsInEdit && eventData.button == PointerEventData.InputButton.Left)
            {
                ElementsManager.Instance.ElementStartDrag(obj);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (obj != null && !ElementsManager.Instance.IsInEdit && eventData.button == PointerEventData.InputButton.Left)
            {
                ElementsManager.Instance.ElementDraging();
            }
        }
        private void OnDestroy()
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
    }

}
