using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeHit
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Transform LogSpawner;
        [SerializeField] Transform KnifeSpawner;

        [SerializeField] GameObject LogPrefab;
        [SerializeField] GameObject KnifePrefab;

        [SerializeField] GameObject CurrentLog;
        [SerializeField] GameObject CurrentKnife;

        private void SpawnLog()
        {

        }

        private void SpawnKnife() {
        }


    }
}