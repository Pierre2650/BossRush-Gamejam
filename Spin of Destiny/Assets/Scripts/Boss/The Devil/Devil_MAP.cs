using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_MAP : MonoBehaviour
{

    [Header("Pacing")]
    public float stfu;

    [Header("Player Shadow")]
    private GameObject plShadowPrefab;
    private Devil_MAP_Shadow_Controller plsShadowController;

    [Header("Spawn Pos")]
    public float stfu2;

    // Start is called before the first frame update
    void Start()
    {
        plShadowPrefab = (GameObject)Resources.Load("Devil_MAP_Player_Shadow", typeof(GameObject));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waitToStart()
    {
        yield return null;
    }

    private void markPlayer()
    {

    }
    private void spawnPlayerShadow()
    {

    }

    private IEnumerator countdownToPull()
    {
        yield return null;

    }

}
