using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GameplayUI : MonoBehaviour
{
    [Header("Config")]
    public Vector3 arrowRoot;

    [Header("Internal Refs")]
    public Button batalion1;
    public TextMeshProUGUI batalionQty1;

    public Button batalion2;
    public TextMeshProUGUI batalionQty2;

    public Button batalion3;
    public TextMeshProUGUI batalionQty3;

    public Button rocket;
    public Transform rocketGrid;

    public Button nextWave;

    [Header("Prefabs")]
    public GameObject mediumPlayer;
    public GameObject rocketPrefab;
    public GameObject decalPrefab;

    public UIState state = UIState.DEFAULT;

    private int selectedOption; // 1-3 = Batalion, 4 = rocket

    public Material targetGood;
    public Material targetBad;

    private GameObject activeDecal;
    private GameplayManager gameplayManager;

    public delegate void OnEvent();
    public OnEvent tutorialBatalionSpawned;
    public OnEvent tutorialNextWave;

    private void Start()
    {
        gameplayManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameplayManager>();
        activeDecal = GameObject.Instantiate(decalPrefab);
        decalPrefab.SetActive(false);
        batalion1.onClick.AddListener(OnBatalion1Press);
        batalion2.onClick.AddListener(OnBatalion2Press);
        batalion3.onClick.AddListener(OnBatalion3Press);
        rocket.onClick.AddListener(OnRocketPressed);
        nextWave.onClick.AddListener(DoNextWave);
        UpdateUI();
    }

    private void Update()
    {
        if (state == UIState.SELECTION)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // Debug.Log("Point was " + hit.point);
                activeDecal.transform.position = hit.point + new Vector3(0, 3f, 0);

                DecalProjector decal = activeDecal.GetComponent<DecalProjector>();
                if (Mathf.Floor(hit.point.z) == 0.0f && selectedOption - 1 >= 0 && selectedOption - 1 < 3 && gameplayManager.batalionCounts[selectedOption - 1] != 0)
                {
                    if (decal.material != targetGood)
                    {
                        decal.material = targetGood;
                    }
                }
                else
                {
                    if (decal.material != targetBad)
                    {
                        decal.material = targetBad;
                    }
                }

                if(Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse clicked, do action");
                    if(selectedOption <= 3)
                    {
                        Vector3 tempPos;
                        tempPos.x = Mathf.Floor(hit.point.x);
                        tempPos.y = Mathf.Floor(hit.point.y);
                        tempPos.z = Mathf.Floor(hit.point.z);
                        if (tempPos.z == 0.0f && gameplayManager.batalionCounts[selectedOption - 1] != 0)
                        {
                            gameplayManager.batalionCounts[selectedOption - 1]--;
                            UpdateUI();
                            DoPlayerAction(tempPos, selectedOption);
                            TowerAudio.Instance.Play("click");
                        }
                    }
                    else if (selectedOption == 4)
                    {
                        DoRocketAction(hit.point);
                    }
                    
                    state = UIState.DEFAULT;

                    activeDecal.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Didn't hit, this shouldn't be doable");
                Debug.DrawLine(arrowRoot, ray.direction * 100);
            }
        }
    }

    public void UpdateUI()
    {
        batalionQty1.text = gameplayManager.batalionCounts[0].ToString();
        batalionQty2.text = gameplayManager.batalionCounts[1].ToString();
        batalionQty3.text = gameplayManager.batalionCounts[2].ToString();

        int rocketsAvailable = gameplayManager.availableRockets;
        for(int i = 0; i < rocketGrid.childCount; i++)
        {
            Transform child = rocketGrid.GetChild(i);
            Image target = child.gameObject.GetComponent<Image>();

            if(i < rocketsAvailable)
            {
                target.enabled = true;
            }
            else
            {
                target.enabled = false;
            }
        }
    }

    public void OnBatalion1Press()
    {
        OnBatalionButtonPressed(1);
    }

    public void OnBatalion2Press()
    {
        OnBatalionButtonPressed(2);
    }

    public void OnBatalion3Press()
    {
        OnBatalionButtonPressed(3);
    }

    private void OnBatalionButtonPressed(int batalion)
    {
        if (state == UIState.DEFAULT)
        {
            state = UIState.SELECTION;
            selectedOption = batalion;
            activeDecal.SetActive(true);
        }
    }

    private void OnRocketPressed()
    {
        Debug.Log("Rocket start");
        if(state == UIState.DEFAULT)
        {
            Debug.Log("Default State");
            if(gameplayManager.availableRockets > 0)
            {
                Debug.Log("rocket select mode");
                selectedOption = 4;
                state = UIState.SELECTION;
                activeDecal.SetActive(true);
            }
        }
    }

    private void DoRocketAction(Vector3 target)
    {
        Debug.Log("Do player action rocket");
        DoRocket(target);
    }

    private void DoPlayerAction(Vector3 target, int type)
    {

        StartCoroutine(SpawnBattalion(target, type));
        tutorialBatalionSpawned?.Invoke();
        UpdateUI();
    }

    private void DoRocket(Vector3 where)
    {
        Debug.Log("DoRocket");
        GameObject spawnedRocket = GameObject.Instantiate(rocketPrefab);
        Rocket rocketCode = spawnedRocket.GetComponent<Rocket>();
        rocketCode.Init(where);
        gameplayManager.availableRockets--;
        UpdateUI();
    }

    private void DoNextWave()
    {
        //TODO Add sound
        gameplayManager.NextWave();
        tutorialNextWave?.Invoke();
        UpdateUI();
        TowerAudio.Instance.Play("readygo");
    }

    private IEnumerator SpawnBattalion(Vector3 target, int type)
    {
        int end = 0;
        if(type == 1)
        {
            end = 10;
        }
        else if(type == 2)
        {
            end = 2;
        }
        else if(type == 3)
        {
            end = 1;
        }
        for (int i = 0; i < end; i++)
        {
            MediumPlayer player = Instantiate(mediumPlayer, target, Quaternion.Euler(0,0,0)).GetComponent<MediumPlayer>();
            if(type == 1)
            {
                player.state = PlayerType.SMALL;
            }
            else if(type == 2)
            {
                player.state = PlayerType.MEDIUM;
            }
            else if(type == 3)
            {
                player.state = PlayerType.LARGE;
            }
            yield return new WaitForSeconds(0.16f);
        }
    }
}

public enum UIState
{
    DEFAULT,
    SELECTION
}
