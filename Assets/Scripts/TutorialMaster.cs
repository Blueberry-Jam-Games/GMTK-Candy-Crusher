using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialMaster : MonoBehaviour
{
    public delegate void OnComplete();

    public GameObject dark1;
    public GameObject dark2;
    public GameObject dark3;
    public GameObject dark4;
    public GameObject darkWait;
    public GameObject light1;
    public GameObject light2;

    public GameObject blackout;

    public TextMeshProUGUI text;
    public Image portrait;

    public RectTransform mouse;

    [Header("External but in scene")]
    public GameplayUI ui;
    private GameplayManager gm;

    int state = 0;

    void Start()
    {
        light1.SetActive(false);
        light2.SetActive(false);
        gm = GameObject.FindWithTag("GameController").GetComponent<GameplayManager>();
        mouse.gameObject.SetActive(false);
        StartCoroutine(DoDialogue(new List<string>(){
            "For years our cookies have been stolen from us. These men and women live in their ginger towns eating treats to no end.",
            "But no more, for it is you, General, who shall retrieve the cookies! Despite their best tower defenses, we shall march!"
        }, BattalionExample));

        // Init god towers
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i < towers.Length; i++)
        {
            TowerAttackScript current = towers[i].GetComponent<TowerAttackScript>();
            current.sprinklesFireRate = 0.25f;
            current.peppermintFireRate = 0.25f;
            current.laserFireRate = 0.5f;
            current.ammo = 1000;
        }
    }

    private IEnumerator DoDialogue(List<string> lines, OnComplete onComplete)
    {
        blackout.SetActive(true);

        int currentLine = 0;
        while(currentLine < lines.Count)
        {
            text.text = lines[currentLine];

            yield return new WaitForSeconds(0.5f);

            while(!Input.GetMouseButton(0))
            {
                yield return null;
            }
            currentLine++;
        }

        blackout.SetActive(false);
        text.text = string.Empty;
        onComplete?.Invoke();
    }

    public void BattalionExample()
    {
        dark1.SetActive(false);
        ui.UpdateUI();
        state = 0;
        StartCoroutine(MoveMouse());
        // Set event state
        ui.tutorialBatalionSpawned += SpawnComplete;
    }

    private IEnumerator MoveMouse()
    {
        mouse.gameObject.SetActive(true);
        text.text = "First, deploy a battalion by clicking in your command menu, and choosing a space on the back mint green row  to send them from";
        mouse.position = new Vector3(-122, -244, 0);

        while(state == 0)
        {
            mouse.position = Vector3.MoveTowards(mouse.position, new Vector3(-245, -220, 0), 1f);
            yield return null;
        }
        mouse.gameObject.SetActive(false);
    }

    public void SpawnComplete()
    {
        ui.tutorialBatalionSpawned -= SpawnComplete;
        state = 1;
        text.text = "Now let the troops do their march!";
        dark1.SetActive(true);
        // Troops die
        StartCoroutine(MonitorSoldiers());
    }

    private IEnumerator MonitorSoldiers()
    {
        Debug.Log("watching for all soldiers dead");
        yield return new WaitForSeconds(0.2f);
        while(GameObject.FindGameObjectsWithTag("Soldier").Length != 0)
        {
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Soldier"))
            {
                Debug.Log("Still alive " + go.name);
            }
            yield return null;
        }
        Debug.Log("All of them are dead");
        TroopsDead();
    }

    public void TroopsDead()
    {
        Debug.Log("Troops dead");
        StartCoroutine(DoDialogue(new List<string>(){
            "Blast! Those towers are too good!",
            "Don’t worry, we have a solution! Launch a rocket attack and target their tower!"
        }, StartRocket));
    }

    public void StartRocket()
    {
        Debug.Log("start Rocket");
        text.text = "Click the rocket icon, then click a tower to destroy.";
        dark1.SetActive(true);
        dark4.SetActive(false);
        light2.SetActive(true);
        light1.SetActive(false);

        // detect any tower destroyed
        StartCoroutine(PollRocketCount());
    }

    private IEnumerator PollRocketCount()
    {
        Debug.Log("Polling rocket count");
        yield return new WaitForSeconds(0.2f);
        while(gm.availableRockets > 0)
        {
            yield return null;
        }
        Debug.Log("rocket used");
        yield return new WaitForSeconds(4f);
        TowerDestroyed();
    }

    public void TowerDestroyed()
    {
        Debug.Log("Tower destroyed");
        text.text = "Now send another battalion!";
        gm.batalionCounts[0] = 1;
        ui.UpdateUI();
        light2.SetActive(false);
        dark4.SetActive(true);
        dark1.SetActive(false);
        // wait for death
        ui.tutorialBatalionSpawned += Batalion2Spawned;
    }

    private void Batalion2Spawned()
    {
        Debug.Log("Batalion 2 spawned");
        ui.tutorialBatalionSpawned -= Batalion2Spawned;
        StartCoroutine(MonitorSoldiers2());
    }

    private IEnumerator MonitorSoldiers2()
    {
        Debug.LogError("Stacktrace to polling");
        Debug.Log("Monitoring soldiers again");
        yield return new WaitForSeconds(0.2f);
        while(GameObject.FindGameObjectsWithTag("Soldier").Length != 0)
        {
            yield return null;
        }
        Debug.Log("All dead");
        SecondBattalionDestroyed();
    }

    public void SecondBattalionDestroyed()
    {
        StartCoroutine(DoDialogue(new List<string>(){
            "Darn, still too powerful. But these aren't the only forces you have access to.",
            "Blue soldiers are fast and plentiful, red soldiers are heartier but harder to come by, and stripey soldiers are battle hardened and ready for the front lines.",
            "Unfortunately, you don’t have any in your squad right now, so hit the hourglass to wait for reinforcements, we can send help."
        }, ReenforcementButtonReady));
    }

    public void ReenforcementButtonReady()
    {
        Debug.Log("Reenforcement button ready");
        text.text = "Unfortunately, you don’t have any in your squad right now, so hit the hourglass to wait for reinforcements, we can send help.";
        light1.SetActive(true);
        dark1.SetActive(true);
        darkWait.SetActive(false);
        // wait for button
        ui.tutorialNextWave += ReenfocementsPressed;
    }

    private void ReenfocementsPressed()
    {
        Debug.Log("Reenforcement pressed");
        ui.tutorialNextWave -= ReenfocementsPressed;

        ReinforcementsArrived();
    }

    public void ReinforcementsArrived()
    {
        Debug.Log("Reenforcements arrived");
        dark3.SetActive(false);
        darkWait.SetActive(true);
        StartCoroutine(DoDialogue(new List<string>(){
            "Waiting for reinforcements is a good way to replenish your troop supply.",
            "But watch out, enemy towers have limited ammo. While you wait for Reinforcements, they can fortify their positions.",
            "Your stripey soldier has arrived, give him the order to RETRIEVE THE COOKIES!"
        }, EndText));

        // Depower towers
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i < towers.Length; i++)
        {
            TowerAttackScript current = towers[i].GetComponent<TowerAttackScript>();
            current.sprinklesFireRate = 1f;
            current.peppermintFireRate = 0.5f;
            current.laserFireRate = 2.5f;
            current.damageDone = 0.0f;
            current.ammo = 5;
        }
    }

    public void EndText()
    {
        text.transform.parent.gameObject.SetActive(false);
        portrait.gameObject.SetActive(false);
        //GameObject winScreen = GameObject.FindWithTag("WinScreen");
        //winScreen.GetComponent<Canvas>().enabled = true;
    }
}
