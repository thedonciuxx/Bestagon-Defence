﻿using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive;
        
    // Our enemy
    public Transform enemyPrefab;
    
    // Times before sending waves
    public float timeBetweenWaves = 5f;
    // Set to first countdown
    private float _countdown = 5f;
    
    // The countdown text timer
    public TMP_Text waveCountdownText;
    
    // The current wave we're on
    private int _waveIndex;
    
    // The point to spawn in the enemy.
    // Should be waypoint 0
    public Transform spawnPoint;
    
    [SerializeField]
    // Time to wait before spawning the next enemy in the wave.
    private float timeBetweenEnemies = 0.5f;
    
    private void Update()
    {
        // Only reduce the countdown if there are enemies remaining
        // TODO - Update countdown text to enemies remaining
        if (enemiesAlive > 0)
        {
            return;
        }

        // If we've reached the end of the countdown,
        // call the next wave
        if (_countdown <= 0f)
        {
            // Start spawning in the enemies
            StartCoroutine(SpawnWave());
            // Reset the timer
            _countdown = timeBetweenWaves;

            return;
        }

        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
        
        waveCountdownText.text = string.Format("<sprite=\"UI-Icons\" name=\"Clock\"> {0:0.00}", _countdown);

        GameStats.rounds = _waveIndex + 1;
    }
    
    // Spawns in our enemies
    private IEnumerator SpawnWave()
    {
        _waveIndex++;
        
        // For all the enemies we will spawn,
        // spawn one, then wait timeBetweenEnemies seconds
        for (var i = 0; i < _waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }
    
    // Spawns an enemy.
    // What else would it do!?
    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;
    }
}
