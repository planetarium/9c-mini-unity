using UniRx;
using UnityEngine;

namespace Mini9C.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/EventChannels/Create AgentStateEventChannel",
        fileName = "AgentStateEventChannel",
        order = 0)]
    public class AgentStateEventChannel : ScriptableObject
    {
        public readonly Subject<float> RequestCooldownProgress = new();
        public readonly Subject<string> Address = new();
        public readonly Subject<decimal> NCG = new();
    }
}
