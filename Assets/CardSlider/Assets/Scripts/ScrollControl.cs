using UnityEngine;
using UnityEngine.EventSystems;

namespace tkitfacn.UI
{
    public class ScrollControl : MonoBehaviour
    {
        [SerializeField] CardSlider cardSlider = default;
        [SerializeField] float speed = 1;
        [SerializeField] bool clamp = false;
        float controlSpeed = 0;
        float value = 0;
        RectTransform rect;
        Vector3 initPos = Vector3.zero;

        private void Start()
        {
            rect = gameObject.GetComponent<RectTransform>();
            controlSpeed = (Screen.width / 1080f) * 0.01f;
            //SetupEvent();
        }

        void SetupEvent()
        {
            if (gameObject.GetComponent<UnityEngine.UI.Image>() == null)
            {
                Debug.LogWarning("Use event in scroll control need image component");
            }
            //Delete Update() when use SetupEvent;
            var e = gameObject.AddComponent<EventTrigger>();
            var enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerDown;
            enter.callback.AddListener((d) =>
            {
                initPos = Input.mousePosition;
            });
            var move = new EventTrigger.Entry();
            move.eventID = EventTriggerType.Drag;
            move.callback.AddListener((d) =>
            {
                var delta = (Input.mousePosition - initPos) * controlSpeed * speed;
                value -= delta.x;
                value = Mathf.Clamp(value, -1, 1);
                initPos = Input.mousePosition;
            });
            var up = new EventTrigger.Entry();
            up.eventID = EventTriggerType.PointerUp;
            up.callback.AddListener((d) =>
            {
                cardSlider.SetCardIndex((int)Mathf.Clamp(cardSlider.CardIndex + Mathf.Clamp(value, -1, 1), 0, cardSlider.CardLength), true);
                value = 0;
            });
            e.triggers.Add(enter);
            e.triggers.Add(move);
            e.triggers.Add(up);
        }

        public bool isControl { get; set; }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (InsideRect(rect))
                {
                    initPos = Input.mousePosition;
                    isControl = true;
                }
            }

            if (!isControl) return;
            if (Input.GetMouseButton(0))
            {
                var delta = (Input.mousePosition - initPos) * controlSpeed * speed;
                value -= delta.x;
                if (clamp)
                {
                    value = Mathf.Clamp(value, -1, 1);
                }
                else
                {
                    if (Mathf.Abs(value) > 2)// cardSlider.GetCardSelectRect().sizeDelta.x / 100f)
                    {
                        value = Mathf.Clamp(value, -1, 1);
                        cardSlider.SetCardIndex((int)Mathf.Clamp(cardSlider.CardIndex + value, 0, cardSlider.CardLength), true);
                        value = 0;
                    }
                }
                initPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (clamp)
                    cardSlider.SetCardIndex((int)Mathf.Clamp(cardSlider.CardIndex + value, 0, cardSlider.CardLength), true);
                value = 0;
                isControl = false;
            }
        }

        [SerializeField] bool canvasWorldSpace = false;

        bool InsideRect(RectTransform content)
        {
            if (canvasWorldSpace)
                return content.rect.Contains(content.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            else return content.rect.Contains(content.InverseTransformPoint(Input.mousePosition));
        }
    }
}
