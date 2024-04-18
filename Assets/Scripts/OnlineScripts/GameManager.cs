using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {  
        if (RelayManager.Instance.IsHost) ConnectRelayHost();
        else ConnectRelayClient();  
    }

    private void ConnectRelayHost()
    {
        (byte[] allocationId, byte[] key, byte[] connectionData, string serverIp, int port) = RelayManager.Instance.GetHostConnectionInfo();
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(serverIp, (ushort)port, allocationId, key, connectionData, true);
        NetworkManager.Singleton.StartHost();
    }

    private void ConnectRelayClient()
    {
        (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string serverIp, int port) = RelayManager.Instance.GetClientConnectionInfo();
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(serverIp, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
        NetworkManager.Singleton.StartClient();
    }
}
