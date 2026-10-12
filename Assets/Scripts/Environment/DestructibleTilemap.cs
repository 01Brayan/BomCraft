using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTilemap : MonoBehaviour
{
    private Tilemap tilemap;

    [Header("Power-Ups")]
    [SerializeField] private GameObject[] powerUpPrefabs;

    [Range(0f, 1f)]
    [SerializeField] private float powerUpChance = 0.25f;

    [Header("Llave")]
    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private int minBlocksBeforeKey = 5;

    private int destroyedBlocks = 0;

    private Vector3Int keyCell;
    private bool keyCellSelected = false;
    private bool keySpawned = false;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public bool HasTileAt(Vector3 worldPosition)
    {
        Vector3Int cellPosition =
            tilemap.WorldToCell(worldPosition);

        return tilemap.HasTile(cellPosition);
    }

    public void DestroyTileAt(Vector3 worldPosition)
    {
        Vector3Int cellPosition =
            tilemap.WorldToCell(worldPosition);

        if (!tilemap.HasTile(cellPosition))
            return;

        // Eliminamos el bloque
        tilemap.SetTile(cellPosition, null);

        destroyedBlocks++;

        Debug.Log(
            "Bloques destruidos: "
            + destroyedBlocks
        );

        // Si este era el bloque que escondía la llave,
        // aparece la llave.
        if (
            keyCellSelected &&
            !keySpawned &&
            cellPosition == keyCell)
        {
            SpawnKey(cellPosition);
            return;
        }

        // Después de destruir mínimo 5 bloques,
        // elegimos aleatoriamente uno de los bloques
        // que todavía quedan.
        if (
            !keyCellSelected &&
            destroyedBlocks >= minBlocksBeforeKey)
        {
            ChooseRandomKeyCell();
        }

        // Si no había llave aquí,
        // puede aparecer un Power-Up.
        TrySpawnPowerUp(cellPosition);
    }

    private void ChooseRandomKeyCell()
    {
        List<Vector3Int> remainingBlocks =
            new List<Vector3Int>();

        foreach (
            Vector3Int cell
            in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(cell))
            {
                remainingBlocks.Add(cell);
            }
        }

        if (remainingBlocks.Count == 0)
        {
            Debug.LogWarning(
                "No quedan bloques para esconder la llave."
            );

            return;
        }

        int randomIndex =
            Random.Range(
                0,
                remainingBlocks.Count
            );

        keyCell =
            remainingBlocks[randomIndex];

        keyCellSelected = true;

        Debug.Log(
            "La llave ha sido escondida " +
            "en un bloque aleatorio."
        );
    }

    private void SpawnKey(Vector3Int cellPosition)
    {
        if (keyPrefab == null)
        {
            Debug.LogWarning(
                "No se ha asignado el prefab Key."
            );

            return;
        }

        Vector3 spawnPosition =
            tilemap.GetCellCenterWorld(
                cellPosition
            );

        Instantiate(
            keyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        keySpawned = true;

        Debug.Log(
            "¡La llave ha aparecido!"
        );
    }

    private void TrySpawnPowerUp(Vector3Int cellPosition)
    {
        if (
            powerUpPrefabs == null ||
            powerUpPrefabs.Length == 0)
        {
            return;
        }

        if (Random.value > powerUpChance)
            return;

        int randomIndex =
            Random.Range(
                0,
                powerUpPrefabs.Length
            );

        Vector3 spawnPosition =
            tilemap.GetCellCenterWorld(
                cellPosition
            );

        Instantiate(
            powerUpPrefabs[randomIndex],
            spawnPosition,
            Quaternion.identity
        );
    }
}
