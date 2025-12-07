using System;
using UnityEngine;

namespace CoreBox
{
    internal sealed class NavigationService : MonoBehaviour
    {
        [SerializeField] private GameObject authWindow;
        [SerializeField] private GameObject regWindow;
        [SerializeField] private GameObject scenariosWindow;

        public event Action OnAuthorizationSuccess;

        public void SetAuthorizationWindow()
        {
            DisableWindows();
            authWindow.SetActive(true);
        }

        public void SetScenariosWindow()
        {
            OnAuthorizationSuccess?.Invoke();
            DisableWindows();
            scenariosWindow.SetActive(true);
        }

        private void DisableWindows()
        {
            authWindow.SetActive(false);
            scenariosWindow.SetActive(false);
            regWindow.SetActive(false);
        }
    }
}