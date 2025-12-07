using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoreBox
{
    internal sealed class RegisterView : MonoBehaviour, IRegisterView
    {
        public event Action<string, string, string> OnRegisterPressed;

        [SerializeField] private GameObject authPanel;

        [SerializeField] private Button registerButton;

        [SerializeField] private TMP_InputField userName;
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;

        [SerializeField] private TMP_Text errorText;

        private void Start()
        {
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        public void OnError(string message)
        {
            Debug.LogError(message);
            errorText.text = message;
            errorText.gameObject.SetActive(true);
        }

        public void OnSuccess(string message)
        {
            Debug.Log(message);
            authPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void OnRegisterButtonClicked()
        {
            OnRegisterPressed?.Invoke(userName.text, email.text, password.text);
        }

        private void OnDestroy()
        {
            registerButton.onClick.RemoveListener(OnRegisterButtonClicked);
        }
    }
}