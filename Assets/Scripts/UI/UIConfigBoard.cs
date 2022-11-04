using System.Globalization;
using Mini9C.ScriptableObjects;
using Mini9C.ScriptableObjects.EventChannels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Mini9C.UI
{
    public class UIConfigBoard : UIBehaviour
    {
        [SerializeField]
        private Button configuration;

        [SerializeField]
        private GameObject body;

        [SerializeField]
        private Button apply;

        [SerializeField]
        private TMP_InputField blockTipRequestCooldown;

        [SerializeField]
        private TMP_InputField agentStateRequestCooldown;

        [SerializeField]
        private MainConfig mainConfig;

        [SerializeField]
        private MainConfigEventChannel mainConfigEventChannel;

        private void Awake()
        {
            // Initialization
            apply.interactable = false;
            SetText(blockTipRequestCooldown, mainConfig.blockTipRequestCooldown);
            SetText(agentStateRequestCooldown, mainConfig.agentStateRequestCooldown);

            // Subscribe internal events
            configuration.onClick.AddListener(OnConfiguration);
            apply.onClick.AddListener(OnApply);
            blockTipRequestCooldown.onValueChanged.AddListener(_ => apply.interactable = true);
            agentStateRequestCooldown.onValueChanged.AddListener(_ => apply.interactable = true);

            // Subscribe external events
            mainConfigEventChannel.BlockTipRequestCooldown
                .Subscribe(value => SetText(blockTipRequestCooldown, value))
                .AddTo(gameObject);
            mainConfigEventChannel.AgentStateRequestCooldown
                .Subscribe(value => SetText(agentStateRequestCooldown, value))
                .AddTo(gameObject);
        }

        private static void SetText(TMP_InputField inputField, float value) =>
            inputField.text = value.ToString(CultureInfo.InvariantCulture);

        private void OnConfiguration()
        {
            body.SetActive(!body.activeSelf);
        }

        private void OnApply()
        {
            apply.interactable = false;
            if (float.TryParse(blockTipRequestCooldown.text.Trim(), out var fValue))
            {
                mainConfigEventChannel.SetBlockTipRequestCooldown.OnNext(fValue);
            }

            if (int.TryParse(agentStateRequestCooldown.text.Trim(), out var iValue))
            {
                mainConfigEventChannel.SetAgentStateRequestCooldown.OnNext(iValue);
            }
        }
    }
}
