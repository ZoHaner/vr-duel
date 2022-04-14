using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField]
        private Image _curtain;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            SetAlpha(1f);
        }

        public void Hide() => 
            StartCoroutine(FadeIn());

        private IEnumerator FadeIn()
        {
            while (_curtain.color.a > 0)
            {
                SetAlpha(_curtain.color.a - 0.03f);
                yield return new WaitForSeconds(0.03f);
            }
            
            gameObject.SetActive(false);
        }

        private void SetAlpha(float alpha)
        {
            var color = _curtain.color;
            color.a = alpha;
            _curtain.color = color;
        }
    }
}