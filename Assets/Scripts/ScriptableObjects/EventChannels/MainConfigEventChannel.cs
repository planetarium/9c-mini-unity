using UniRx;
using UnityEngine;

namespace Mini9C.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/EventChannels/Create MainConfigEventChannel",
        fileName = "MainConfigEventChannel",
        order = 0)]
    public class MainConfigEventChannel : ScriptableObject
    {
        public readonly Subject<float> BlockTipRequestCooldown = new();
        public readonly Subject<float> SetBlockTipRequestCooldown = new();
        
        public readonly Subject<float> AgentStateRequestCooldown = new();
        public readonly Subject<int> SetAgentStateRequestCooldown = new();
    }
}
