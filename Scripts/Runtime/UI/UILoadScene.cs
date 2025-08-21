using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TDKToolkit.UI
{
    public class UILoadScene : UIPageBase
    {

        [SerializeField] private Slider slider;
        [SerializeField] private Text textProcess;
        [SerializeField] private Text tips;
        [SerializeField] private Text onLoadedTips;
        [SerializeField] private Image backGround;
        [SerializeField] private Image FGimg;
        public float loadEntryTime = 1.5f;
        public float loadEndTime = 5.0f;
        public override void Ini()
        {
            base.Ini();

        }
        public void ResetValue()
        {
            slider.value = 0;
            textProcess.text = "0.0%";
            tips.text = "我去有没有搞错啊";
        }
        public void SetLoadReadyMode()
        {
            tips.gameObject.SetActive(false);
            onLoadedTips.gameObject.SetActive(true);
            onLoadedTips.color = Color.white;
        }
        public IEnumerator FGimgAnimation()
        {
            float time = 0;
            float progress = 0;
            FGimg.gameObject
            .SetActive(true);
            onLoadedTips.color = Color.gray;

            while (true)
            {
                time += Time.deltaTime;
                progress = time / loadEndTime;
                Color temp = FGimg.color;
                temp.a = progress;
                FGimg.color = temp;

                if (time >= loadEndTime)
                {
                    break;
                }
                yield return null;
            }
            yield return null;

        }
        public float SetLoadReadyEnd()
        {
            StartCoroutine(FGimgAnimation());
            return loadEndTime;
        }
        public override void ShowPage()
        {
            gameObject.SetActive(false);
            base.ShowPage();
            tips.gameObject.SetActive(true);
            onLoadedTips.gameObject.SetActive(false);
            FGimg.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        public void SetProcessValue(float var)
        {
            slider.value = var;
            var *= 100;
            textProcess.text = var.ToString("f2") + "%";
        }
        public void SetTips(string strTips)
        {
            tips.text = strTips;
        }
        public void SetLoadImg(Sprite img)
        {
            backGround.sprite = img;
        }


    }

}
