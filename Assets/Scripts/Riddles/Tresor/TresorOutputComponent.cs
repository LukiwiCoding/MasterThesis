using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TresorOutputComponent : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI textfield;

    private int currentDigit = 0;
    public int GetCurrentDigit { get { return currentDigit; } }
    [ServerRpc]
    public void UpdateOutputFieldServerRPC(bool countUp)
    {
        if (int.TryParse(textfield.text, out int result))
        {
            int delta = countUp ? 1 : -1;
            result += delta;

            switch (result)
            {
                case -1:
                    {
                        result = 9;
                        textfield.text = result.ToString();
                        break;
                    }
                case 10:
                    {
                        result = 0;
                        textfield.text = result.ToString();
                        break;
                    }
                default: textfield.text = result.ToString(); break;
            }
            currentDigit = result;
        }
    }
}
