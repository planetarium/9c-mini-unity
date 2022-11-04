using UniRx;
using UnityEngine;

namespace Mini9C.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/EventChannels/Create SigninEventChannel",
        fileName = "SigninEventChannel",
        order = 0)]
    public class SigninEventChannel : ScriptableObject
    {
        public readonly Subject<string> SetAddress = new();
    }
}
