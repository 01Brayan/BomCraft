using System.Collections.Generic;
using UnityEngine;

public static class GridPathfinder
{
    // Las cuatro direcciones posibles del tablero
    private static readonly Vector3Int[] directions =
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    public static Vector3Int? FindNextStep(
        Vector3 startWorldPosition,
        Vector3 targetWorldPosition,
        Grid grid,
        LayerMask obstacleLayer,
        LayerMask bombLayer)
    {
        Vector3Int startCell =
            grid.WorldToCell(startWorldPosition);

        Vector3Int targetCell =
            grid.WorldToCell(targetWorldPosition);

        if (startCell == targetCell)
            return null;

        Queue<Vector3Int> frontier =
            new Queue<Vector3Int>();

        Dictionary<Vector3Int, Vector3Int> cameFrom =
            new Dictionary<Vector3Int, Vector3Int>();

        frontier.Enqueue(startCell);
        cameFrom[startCell] = startCell;

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            if (current == targetCell)
                break;

            foreach (Vector3Int direction in directions)
            {
                Vector3Int next = current + direction;

                // Si ya revisamos esta celda, no repetimos
                if (cameFrom.ContainsKey(next))
                    continue;

                // Si existe pared, bloque o bomba, no podemos pasar
                if (IsCellBlocked(
                    next,
                    grid,
                    obstacleLayer,
                    bombLayer))
                {
                    continue;
                }

                frontier.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        // No existe camino hasta el jugador
        if (!cameFrom.ContainsKey(targetCell))
            return null;

        // Retrocedemos desde el Player hasta encontrar
        // cuál es el primer paso que debe dar el enemigo
        Vector3Int step = targetCell;

        while (cameFrom[step] != startCell)
        {
            step = cameFrom[step];
        }

        return step;
    }

    public static bool IsCellBlocked(
        Vector3Int cell,
        Grid grid,
        LayerMask obstacleLayer,
        LayerMask bombLayer)
    {
        Vector3 worldPosition =
            grid.GetCellCenterWorld(cell);

        bool obstacleBlocked =
            Physics2D.OverlapBox(
                worldPosition,
                Vector2.one * 0.8f,
                0f,
                obstacleLayer
            );

        bool bombBlocked =
            Physics2D.OverlapBox(
                worldPosition,
                Vector2.one * 0.6f,
                0f,
                bombLayer
            );

        return obstacleBlocked || bombBlocked;
    }
}
