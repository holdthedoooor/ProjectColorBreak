using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoUI : MonoBehaviour
{
    public GameObject go_Logo;
    
    void Start()
    {
        StartCoroutine( LogoCoroutine() );
    }
    
    public IEnumerator LogoCoroutine()
    {
        yield return new WaitForSeconds( 2.5f );

        go_Logo.SetActive( false );
        UIManager.instance.lobbyUI.go_LobbyUI.SetActive( true );
    }
}
