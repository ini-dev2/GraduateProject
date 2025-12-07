using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CoreBox
{
    internal sealed class LoginView : MonoBehaviour, ILoginView
    {
        public event Action<string, string> OnLoginPressed;

        [SerializeField] private Button loginButton;

        [SerializeField] private TMP_InputField loginField;
        [SerializeField] private TMP_InputField passwordField;

        [SerializeField] private TMP_Text errorText;

        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
        }

        public void OnError(string message)
        {
            Debug.LogError(message);
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            loginButton.interactable = true;
        }

        public void OnSuccess(string message)
        {
            Debug.Log(message);
            loginButton.interactable = true;
        }

        private void OnLoginButtonClicked()
        {
            OnLoginPressed?.Invoke(loginField.text, passwordField.text);
            loginButton.interactable = false;
        }

        private void OnDestroy()
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClicked);
        }
    }
}