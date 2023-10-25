using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[System.Serializable]
public class Spawner : MonoBehaviour
{
    public List<Phase> Phases;

    public List<GameObject> EnemyGroup;
    public List<GameObject> NPCGroup;

    public AudioClip Stage;
    public AudioClip BossBGM;

    public GameObject Boss;

    public float timer = 0f;

    void Start()
    {
        SoundManager.Instance.BGPlay("Stage1", Stage);
        SpawnManager();
        SpawnNPC();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    void Spawn(EnemyType type)
    {
        GameObject obj;
        switch (type)
        {
            case EnemyType.Bacteria:
                obj = Instantiate(EnemyGroup[0]);
                obj.transform.position = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(7f, 12f));
                if (obj.transform.position.x > 2.5f) { obj.GetComponent<Enemy>().Direction = Direction.Left; }
                else if (obj.transform.position.x < -2.5f) { obj.GetComponent<Enemy>().Direction = Direction.Right; }
                else
                {
                    obj.GetComponent<Enemy>().Direction = Direction.Center;
                    obj.GetComponent<Enemy>().Speed = 1.5f;
                }
                break;
            case EnemyType.Germ:
                obj = Instantiate(EnemyGroup[1]);
                obj.transform.position = new Vector2(Random.Range(-7.7f, 7.7f), Random.Range(7f, 11f));
                break;
            case EnemyType.Virus:
                obj = Instantiate(EnemyGroup[2]);
                obj.transform.position = new Vector2(Random.Range(-7.8f, 7.8f), Random.Range(-7f, -11f));
                break;
            case EnemyType.Cancer:
                obj = Instantiate(EnemyGroup[3]);
                obj.transform.position = new Vector2(Random.Range(-7.7f, 7.7f), Random.Range(7f, 11f));
                if (transform.position.x > 4.5f) { obj.GetComponent<Enemy>().Direction = Direction.Left; }
                else if (transform.position.x < -4.5f) { obj.GetComponent<Enemy>().Direction = Direction.Right; }
                else
                {
                    obj.GetComponent<Enemy>().Direction = Direction.Center;
                }
                break;
        }
    }

    void Spawn(NPCType type)
    {
        GameObject obj;
        switch (type)
        {
            case NPCType.White:
                obj = Instantiate(NPCGroup[0]);
                obj.transform.position = new Vector2(Random.Range(-8f, 8f), Random.Range(7f, 11f));
                break;
            case NPCType.Red:
                obj = Instantiate(NPCGroup[1]);
                obj.transform.position = new Vector2(Random.Range(-8f, 8f), Random.Range(-7f, -11f));
                break;
        }
    }

    void SpawnEnemy(int PhaseNum)
    {
        if (PhaseNum == 6)
        {
            //Spawn Boss
            Debug.Log("Boss Spawn");
            SpawnBoss();
            return;
        }
        for (int i = 0; i < Phases[PhaseNum].Enemy.Count; i++)
        {
            Spawn(Phases[PhaseNum].Enemy[i]);
        }
        StopCoroutine(WaitForSpawnTime());
        StartCoroutine(WaitForSpawnTime());
    }

    void SpawnBoss()
    {
        Destroy(SoundManager.Instance.BGSound);
        SoundManager.Instance.BGPlay("BossBGM", BossBGM);
        GameObject obj = Instantiate(Boss);
        obj.transform.position = new Vector2(0, 11f);
    }

    void SpawnManager()
    {
        if (timer > 100f) { SpawnEnemy(6); }
        else if (timer > 70f) { SpawnEnemy(Random.Range(3, 7)); }
        else if (timer > 60f) { SpawnEnemy(Random.Range(2, 6)); }
        else if (timer > 50f) { SpawnEnemy(Random.Range(1, 5)); }
        else if (timer > 40f) { SpawnEnemy(Random.Range(0, 4)); }
        else if (timer > 30f) { SpawnEnemy(Random.Range(0, 3)); }
        else if (timer > 15f) { SpawnEnemy(Random.Range(0, 2)); }
        else { SpawnEnemy(0); }
    }

    void SpawnNPC()
    {
        if (Random.Range(0, 4) > 0)
        {
            Spawn(NPCType.White);
        }
        else
        {
            Spawn(NPCType.Red);
        }
        StopCoroutine(WaitforSpawnNPC());
        StartCoroutine(WaitforSpawnNPC());
    }

    IEnumerator WaitforSpawnNPC()
    {
        yield return new WaitForSeconds(3.5f);
        SpawnNPC();
    }

    IEnumerator WaitForSpawnTime()
    {
        yield return new WaitForSeconds(7.5f);
        SpawnManager();
    }
}

[System.Serializable]
public class Phase
{
    public List<EnemyType> Enemy;
}
