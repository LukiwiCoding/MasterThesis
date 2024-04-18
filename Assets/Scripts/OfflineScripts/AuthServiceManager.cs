using System;
using System.Net.Http;
using System.Text;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using WebSocketSharp;

public class AuthServiceManager : Singleton<AuthServiceManager>
{
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        if(UnityServices.State == ServicesInitializationState.Initialized)
        {
            AuthenticationService.Instance.SignedIn += UserSignedIn;
            AuthenticationService.Instance.SignInFailed += UserSignInFailed;
            //Später ersetzen mit persistenter Authentification ggf. Hochschulkennung
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void UserSignInFailed(RequestFailedException obj)
    {
        throw new Exception($"Signin failed with error code: {obj.ErrorCode}\nError: {obj.Message}");
    }

    private void UserSignedIn()
    {
        if (PlayerPrefs.GetString("Username").IsNullOrEmpty())
        {
            PlayerPrefs.SetString("Username", "Player");
        }

        MainMenuManager.Instance.EnableButtons();
    }
}
