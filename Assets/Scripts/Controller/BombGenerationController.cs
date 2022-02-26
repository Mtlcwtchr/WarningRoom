using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Controller
{
    public class BombGenerationController : MonoBehaviour
    {
        private const int PoolIncreaseStep = 8;
        private const int InitialPoolCapacity = 32;
        
        [SerializeField] private List<BombController> bombsList;
        [SerializeField] private List<float> bombsGenerationChances;
        
        [SerializeField] private GameObject character;
        
        [SerializeField] private float bombsSpawnDelay;
        [SerializeField] private float bombsShortSpawnDelay;
        [SerializeField] private float bombsSpawnRange;
        [SerializeField] private float bombsSpawnScaleFactor;
        [SerializeField] private float bombsSpawnFactor;
        [SerializeField] private float bombsInitialHeight;

        private float _deltaTime = 0.0f;

        private List<BombController> _freeBombs = new List<BombController>();
        private List<BombController> _busyBombs = new List<BombController>();

        
        
        private void OnEnable()
        {
            FillPool(InitialPoolCapacity);
        }

        
        private void OnDisable()
        {
            foreach (BombController bomb in _freeBombs)
            {
                bomb.OnExploded -= FreeBomb;
            }
            foreach (BombController bomb in _busyBombs)
            {
                bomb.OnExploded -= FreeBomb;
            }
        }
        
        
        private void Update()
        {
            _deltaTime += Time.deltaTime;
            if (_deltaTime >= bombsSpawnDelay)
            {
                _deltaTime = 0.0f;
                
                StartCoroutine(SpawnBombs());
            }
        }


        private IEnumerator SpawnBombs()
        {
            Vector3 characterPosition = character.transform.position;
            for (int i = 0; i < bombsSpawnRange; ++i)
            {
                for (int j = 0; j < bombsSpawnRange; ++j)
                {
                    float x = (float) i / bombsSpawnRange * bombsSpawnScaleFactor;
                    float z = (float) j / bombsSpawnRange * bombsSpawnScaleFactor;
                    
                    float noise = Mathf.PerlinNoise(x, z);
                    if (noise >= bombsSpawnFactor)
                    {
                        SpawnBomb(new Vector3(i + characterPosition.x - bombsSpawnRange/2, bombsInitialHeight, j + characterPosition.z - bombsSpawnRange/2));
                    }
                    
                    yield return new WaitForSeconds(bombsShortSpawnDelay);
                }
            }
        }


        private void SpawnBomb(Vector3 position)
        {
            BombController bomb = GetBomb();
            bomb.transform.position = position;
        }

        
        private int GetBombIndex()
        {
            float range = Random.Range(0.0f, 1.0f);
            float bombsRange = 0.0f;
            for (int i = 0; i < bombsGenerationChances.Count; i++)
            {
                bombsRange += bombsGenerationChances[i];
                if (bombsRange >= range)
                {
                    return i;
                }
            }

            return -1;
        }

        
        private void FreeBomb(BombController bomb)
        {
            if (_busyBombs.Contains(bomb))
            {
                _busyBombs.Remove(bomb);
                _freeBombs.Add(bomb);

                bomb.gameObject.SetActive(false);
            }
        }


        private BombController GetBomb()
        {
            if (_freeBombs.Count <= 0)
            {
                FillPool(PoolIncreaseStep);
            }

            BombController bomb = _freeBombs[_freeBombs.Count-1];
            bomb.gameObject.SetActive(true);
            _freeBombs.Remove(bomb);
            _busyBombs.Add(bomb);

            return bomb;
        }


        private void FillPool(int objectsNum)
        {
            for (int i = 0; i < objectsNum; ++i)
            {
                BombController bomb = Instantiate(bombsList[GetBombIndex()].gameObject, transform).GetComponent<BombController>();
                bomb.OnExploded += FreeBomb;
                bomb.gameObject.SetActive(false);
                _freeBombs.Add(bomb);
            }
        }
    }
}