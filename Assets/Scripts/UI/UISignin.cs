using Mini9C.ScriptableObjects.EventChannels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mini9C.UI
{
    public class UISignin : UIBehaviour
    {
        private const string RememberAddressToggleKey = "UISignin/RememberAddressToggle";
        private const string LastSignedInAddressKey = "UISignin/LastSignedInAddress";

        [SerializeField]
        private TMP_InputField addressInputField;

        [SerializeField]
        private Toggle rememberAddressToggle;

        [SerializeField]
        private Button signinButton;

        [SerializeField]
        private SigninEventChannel signinEventChannel;

        private void Awake()
        {
            // Initialize
            rememberAddressToggle.isOn =
                PlayerPrefs.GetInt(RememberAddressToggleKey, 0) is 1;
            addressInputField.text = rememberAddressToggle.isOn
                ? PlayerPrefs.GetString(LastSignedInAddressKey, string.Empty)
                : string.Empty;
            OnAddressChanged(addressInputField.text);

            // Subscribe internal events
            addressInputField.onEndEdit.AddListener(OnAddressChanged);
            signinButton.onClick.AddListener(OnSigninClicked);
        }

        private void OnEnable()
        {
            addressInputField.Select();
        }

        private void OnAddressChanged(string value)
        {
            signinButton.interactable = ValidateAddressHex(value);
        }

        private void OnSigninClicked()
        {
            if (rememberAddressToggle.isOn)
            {
                PlayerPrefs.SetString(LastSignedInAddressKey, addressInputField.text);
                PlayerPrefs.SetInt(RememberAddressToggleKey, 1);
            }
            else
            {
                PlayerPrefs.SetString(LastSignedInAddressKey, string.Empty);
                PlayerPrefs.SetInt(RememberAddressToggleKey, 0);
            }

            signinEventChannel.SetAddress.OnNext(addressInputField.text);
        }

        private static bool ValidateAddressHex(string hex)
        {
            if (hex == null)
            {
                return false;
            }

            if (hex.Length == 42)
            {
                int pos = hex.IndexOf('x');
                if (pos >= 0)
                {
                    hex = hex.Remove(0, pos + 1);
                }
            }

            return hex.Length == 40;
        }
    }
}
