using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arphros.Tools
{
    public static class InputToolkit
    {
        static int _layer;
        static bool _initialized;

        public static void Init()
        {
            if (_initialized) return;
            _layer = LayerMask.NameToLayer("UI");
            _initialized = true;
        }

        /// <summary>
        /// Intended for use with the new InputSystem
        /// </summary>
        /// <returns></returns>
        public static bool IsNewPointerOverUIElement()
        {
            Init();
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }
        public static bool IsNewPointerOverUIElement(Vector2 pointerPosition)
        {
            Init();
            return IsPointerOverUIElement(GetEventSystemRaycastResults(pointerPosition));
        }

        private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult raycastResult = eventSystemRaysastResults[index];
                if (raycastResult.gameObject.layer == _layer)
                    return true;
            }
            return false;
        }

        static List<RaycastResult> GetEventSystemRaycastResults(Vector2 position)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }

        static List<RaycastResult> GetEventSystemRaycastResults() => GetEventSystemRaycastResults(Input.mousePosition);

        public static bool IsPointerOverUIElement() => EventSystem.current.IsPointerOverGameObject();
        public static bool IsPointerOverUIElement(int id) => EventSystem.current.IsPointerOverGameObject(id);
    }
}
