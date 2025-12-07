using System.Collections;
using UnityEngine;

namespace CoreBox
{
    public class ErrorView : MonoBehaviour
    {
        [SerializeField] private float presentTime;

        private void OnEnable()
        {
            StartCoroutine(TimerFroShow());
        }

        private IEnumerator TimerFroShow()
        {
            yield return new WaitForSeconds(presentTime);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopCoroutine(TimerFroShow());
        }
    }
}