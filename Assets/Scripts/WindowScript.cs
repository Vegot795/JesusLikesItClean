using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    private SpriteRenderer glass;
    private Bounds bounds;
    private float cellWidth;
    private float cellHeight;
    private DirtCell[,] grid;
    private List<GameObject> stainedCells = new List<GameObject>();
    private GameObject stainedCell;

    public int WindowLvl = 1;
    public DirtData[] dirtTypes;
    public int columns = 180;
    public int rows = 240;
    public struct DirtCell
    {
        public bool hasDirt;
        public DirtData dirtType;
        public float cellX;
        public float cellY;
    }

    private void Start()
    {
        bounds = GetComponent<SpriteRenderer>().bounds;
        cellWidth = bounds.size.x / columns;
        cellHeight = bounds.size.y / rows;

        Debug.Log($"Rozmiar okna: {bounds.size.x} x {bounds.size.y}");
        Debug.Log($"Rozmiar komórki: {cellWidth} x {cellHeight}");
        
        grid = new DirtCell[columns, rows];

        SpawnBirdsDirtOnWindow(WindowLvl);
        SpawnMudOnWindow(WindowLvl);
        SpawnSmogOnWindow(WindowLvl);
    }

    
    void OnDrawGizmos()
    {
        Bounds b = GetComponent<SpriteRenderer>().bounds;
        float cw = b.size.x / columns;
        float ch = b.size.y / rows;

        Gizmos.color = Color.red;

        for (int x = 0; x <= columns; x++)
        {
            Vector3 start = new Vector3(b.min.x + x * cw, b.min.y, 0);
            Vector3 end = new Vector3(b.min.x + x * cw, b.max.y, 0);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= rows; y++ )
        {
            Vector3 start = new Vector3(b.min.x, b.min.y + y * ch, 0);
            Vector3 end = new Vector3(b.max.x, b.min.y + y * ch, 0);
            Gizmos.DrawLine(start, end);
        }
    }

    private void SpawnSmogOnWindow(int WindowLvl)
    {
        List<DirtCell> cells = new List<DirtCell>();
        cells = GetAllWindowCells();
        int maxStainLvl = WindowLvl;
        int targetStainLvl = Random.Range(0, maxStainLvl);

        GameObject smogPrefab = dirtTypes[2].dirtPrefab;

        // Stworzenie prefabów dla wszystkich komórek
        foreach (DirtCell cell in cells)
        {
            Vector3 spawnPosition = new Vector3(
                bounds.min.x + cell.cellX * cellWidth + cellWidth / 2,
                bounds.min.y + cell.cellY * cellHeight + cellHeight / 2,
                0);

            if (!cell.hasDirt)
            {
                stainedCell = Instantiate(smogPrefab, spawnPosition, Quaternion.identity);
                stainedCells.Add(stainedCell);

                // Skalowanie prefabów do rozmiaru komórki i ustawienie przezroczystości na podstawie poziomu plamy
                SpriteRenderer sr = stainedCell.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    Vector2 spriteSize = sr.sprite.bounds.size;
                    Vector3 newScale = stainedCell.transform.localScale;
                    newScale.x = cellWidth / spriteSize.x * newScale.x;
                    newScale.y = cellHeight / spriteSize.y * newScale.y;
                    stainedCell.transform.localScale = newScale;
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f * targetStainLvl);
                }
            }
        }
    }

    private void SpawnBirdsDirtOnWindow(int WindowLvl)
    {
        int stainCount = WindowLvl*2;

        List<DirtCell> randomCenters = new List<DirtCell>();
        List<DirtCell> cellsToBeStained = new List<DirtCell>();

        // Generowanie losowych punktów centralnych dla plam
        for (int s = 0; s < stainCount; s++)
        {
            int randomX = Random.Range(0, columns);
            int randomY = Random.Range(0, rows);

            DirtCell randomCell = grid[randomX, randomY];
            randomCell.cellX = randomX;
            randomCell.cellY = randomY;

            randomCenters.Add(randomCell);
        }

        // Tworzenie plam wokół losowych punktów centralnych
        foreach (var center in randomCenters)
        {
            int centerX = (int)center.cellX;
            int centerY = (int)center.cellY;

            int randomWide = Random.Range(1, WindowLvl);
            int randomHigh = Random.Range(1, WindowLvl);

            for (int x = -randomWide; x <= randomWide; x++)
            {
                for (int y = -randomHigh; y <= randomHigh; y++)
                {
                    float dx = (float)x / randomWide;
                    float dy = (float)y / randomHigh;

                    // Sprawdzenie, czy punkt (x, y) znajduje się wewnątrz elipsy o promieniach randomWide i randomHigh
                    if (dx * dx + dy * dy <= 1)
                    {
                        int cellX = centerX + x;
                        int cellY = centerY + y;
                        if (cellX >= 0 && cellX < columns && cellY >= 0 && cellY < rows)
                        {
                            DirtCell cellInsideStain = grid[cellX, cellY];
                            cellInsideStain.cellX = cellX;
                            cellInsideStain.cellY = cellY;

                            cellsToBeStained.Add(cellInsideStain);
                        }
                    }
                }
            }
        }

        // Stworzenie prefabów dla komórek, które mają być poplamione
        for (int x = 0; x < cellsToBeStained.Count; x++)
        {
            Vector3 spawnPosition = new Vector3(
                bounds.min.x + cellsToBeStained[x].cellX * cellWidth + cellWidth / 2,
                bounds.min.y + cellsToBeStained[x].cellY * cellHeight + cellHeight / 2,
                0);

            GameObject birdPrefab = dirtTypes[0].dirtPrefab;

            // Sprawdzenie, czy komórka nie jest już zabrudzona (np. przez smog)
            if (!cellsToBeStained[x].hasDirt)
            {
                stainedCell = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
                stainedCells.Add(stainedCell);

                // Skalowanie prefabów do rozmiaru komórki
                SpriteRenderer sr = stainedCell.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    Vector2 spriteSize = sr.sprite.bounds.size;
                    Vector3 newScale = stainedCell.transform.localScale;
                    newScale.x = cellWidth / spriteSize.x * newScale.x;
                    newScale.y = cellHeight / spriteSize.y * newScale.y;
                    stainedCell.transform.localScale = newScale;
                }
            }
        }
    }
    private void SpawnMudOnWindow(int WindowLvl)
    {
        List<DirtCell> cellsToBeStained = new List<DirtCell>();
        List<DirtCell> pointsForFunc = new List<DirtCell>();

        int minDirtHeight = WindowLvl * 5  ;
        int maxDirtHeight = WindowLvl * 9;
        AnimationCurve curve = new AnimationCurve();
        int pointsCount = 4;

        for (int c = 0; c < pointsCount; c++)
        {
            float t = (float)c / (pointsCount - 1);
            float height = Random.Range(minDirtHeight, maxDirtHeight);
            curve.AddKey(t, height);
        }

        for (int x = 0; x < columns; x++)
        {
            float t = (float)x / (columns - 1);
            float dirtHeight = curve.Evaluate(t);
            for (int y = 0; y < dirtHeight; y++)
            {
                DirtCell cell = grid[x, y];
                cell.cellX = x;
                cell.cellY = y;
                cellsToBeStained.Add(cell);
            }
        }

        foreach (DirtCell cell in cellsToBeStained)
        {
            Vector3 spawnPosition = new Vector3(
                bounds.min.x + cell.cellX * cellWidth + cellWidth / 2,
                bounds.min.y + cell.cellY * cellHeight + cellHeight / 2,
                0);
            GameObject mudPrefab = dirtTypes[1].dirtPrefab;
            // Sprawdzenie, czy komórka nie jest już zabrudzona (np. przez smog)
            if (!cell.hasDirt)
            {
                stainedCell = Instantiate(mudPrefab, spawnPosition, Quaternion.identity);
                stainedCells.Add(stainedCell);
                // Skalowanie prefabów do rozmiaru komórki
                SpriteRenderer sr = stainedCell.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    Vector2 spriteSize = sr.sprite.bounds.size;
                    Vector3 newScale = stainedCell.transform.localScale;
                    newScale.x = cellWidth / spriteSize.x * newScale.x;
                    newScale.y = cellHeight / spriteSize.y * newScale.y;
                    stainedCell.transform.localScale = newScale;
                }
            }
        }
    }

    private List<DirtCell> GetAllWindowCells()
    {
        List<DirtCell> cells = new List<DirtCell>();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                DirtCell cell = grid[x, y];
                cell.cellX = x;
                cell.cellY = y;
                cells.Add(cell);
            }
        }
        return cells;
    }




}
