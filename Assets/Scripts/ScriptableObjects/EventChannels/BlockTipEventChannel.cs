using UniRx;
using UnityEngine;

namespace Mini9C.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/EventChannels/Create BlockTipEventChannel",
        fileName = "BlockTipEventChannel",
        order = 0)]
    public class BlockTipEventChannel : ScriptableObject
    {
        public readonly Subject<float> RequestCooldownProgress = new();
        public readonly Subject<long> BlockIndex = new();
    }
}
