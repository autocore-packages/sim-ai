using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.simai
{
    public class LogicTrafficLight : LogicObject
    {
        public Image[] ImagesA;
        public Image[] ImagesB;
        public Text textSecond;
        public Vector3 offset;
        public override void Start()
        {
            base.Start();
            if (textSecond == null)
                textSecond = transform.GetComponentInChildren<Text>();
            ImagesA = transform.GetChild(0).GetChild(1).GetComponentsInChildren<Image>();
            ImagesB = transform.GetChild(0).GetChild(2).GetComponentsInChildren<Image>();
            ElementsManager.Instance.OnCameraRotate += SetLogicTextAngle;
        }
        private void SetLogicTextAngle(Vector3 angle)
        {
            textSecond.transform.rotation = Quaternion.Euler(angle + offset);
        }
        public void SetLogicTrafficLight(int mode)
        {
            switch (mode)
            {
                case 0:
                    foreach (var item in ImagesA)
                    {
                        item.color = Color.yellow;
                    }
                    foreach (var item in ImagesB)
                    {
                        item.color = Color.yellow;
                    }
                    break;
                case 1:
                    foreach (var item in ImagesA)
                    {
                        item.color = Color.green;
                    }
                    foreach (var item in ImagesB)
                    {
                        item.color = Color.red;
                    }
                    break;
                case 2:
                    foreach (var item in ImagesA)
                    {
                        item.color = Color.red;
                    }
                    foreach (var item in ImagesB)
                    {
                        item.color = Color.green;
                    }
                    break;
                default:
                    break;
            }
        }
        public void SetLogicText(int second)
        {
            textSecond.text = second.ToString();
        }
    }
}