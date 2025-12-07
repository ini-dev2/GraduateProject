using UnityEngine;

namespace CoreBox
{
    [CreateAssetMenu(fileName = "AuthServiceEP", menuName = "ScriptableObjects/API/AuthCfg")]
    internal sealed class AuthServiceEndPoints : ScriptableObject
    {
        [SerializeField] private string loginEndPoint;
        [SerializeField] private string registerEndPoint;

        public string LoginEndPoint => loginEndPoint;
        public string RegisterEndPoint => registerEndPoint;
    }
}