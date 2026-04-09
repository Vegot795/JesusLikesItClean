using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class WindowScript : MonoBehaviour
{
    private float cellWidth = 0.1f;
    private float cellHeight = 0.1f;
    private int totalCells;
    private int dirtyCells;
    private Bounds bounds;
    private DirtCell[,] grid;
    private GameObject stainedCell;
    private GameObject glassObject;
    private SpriteRenderer glass;
    private GameUI gameUI;

    [SerializeField] bool showGizmos = false;
    [UnityEngine.Range(1, 5)]
    public int WindowLvl;
    public int rows;
    public int columns;
    public float clearingProgress = 0;
    public float cutsceneTriggerProgress;
    public string windowName;
    public bool isCleaned = false;
    public GameObject nextWindow;
    public bool firstTry = true;
    public Sprite windowSprite;
    public DirtData[] dirtTypes;
    public List<GameObject> stainedCells = new List<GameObject>();
    public GameObject JesusScare;
    public Vector2 JesusScareOffset;
    public GameObject Jessy;
    private int yes;

    public struct DirtCell
    {
        public bool hasDirt;
        public DirtData dirtType;
        public float cellX ;
        public float cellY;
    }

    //private void Start()
    //{
    //    OnStart();
    //}

    public void OnStart()
    {

        GameUI gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        glassObject = gameObject;
        glassObject.GetComponent<SpriteRenderer>().sprite = windowSprite;
        windowName = glassObject.name;

        bounds = GetComponent<SpriteRenderer>().bounds;

        columns = Mathf.RoundToInt(bounds.size.x / cellWidth);
        rows = Mathf.RoundToInt(bounds.size.y / cellHeight);

        Debug.Log($"Rozmiar okna: {bounds.size.x} x {bounds.size.y}");
        Debug.Log($"Rozmiar komórki: {cellWidth} x {cellHeight}");

        grid = new DirtCell[columns, rows];

        SpawnDirtOnWindows();
        JesusScareOffset = new Vector2(JesusScare.GetComponent<SpriteRenderer>().bounds.size.x / 2, 0f);
    }

    private void Update()
    {
        stainedCells.RemoveAll(go => go == null);
        CountProgress();
        if(isCleaned && yes == 0)
        {
            GameObject.Find("cloth").GetComponent<clothScript>().holdingCloth = false;
            yes++;
            Debug.Log(yes);
            nextWindow.GetComponent<WindowScript>().OnStart();
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
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

            for (int y = 0; y <= rows; y++)
            {
                Vector3 start = new Vector3(b.min.x, b.min.y + y * ch, 0);
                Vector3 end = new Vector3(b.max.x, b.min.y + y * ch, 0);
                Gizmos.DrawLine(start, end);
            }
        }
    }

    private void SpawnSmogOnWindow(int WindowLvl)
    {
        List<DirtCell> cells = GetAllWindowCells();
        GameObject smogPrefab = dirtTypes[2].dirtPrefab;

        // Stworzenie prefabów dla wszystkich komórek
        foreach (DirtCell cell in cells)
        {
            int ix = (int)cell.cellX;
            int iy = (int)cell.cellY;
            if (ix < 0 || ix >= columns || iy < 0 || iy >= rows) 
            { 
                continue; 
            }

            if (grid[ix, iy].hasDirt)
            {
                continue;
            }

            Vector3 spawnPosition = new Vector3(
                bounds.min.x + cell.cellX * cellWidth + cellWidth / 2,
                bounds.min.y + cell.cellY * cellHeight + cellHeight / 2,
                0);

            stainedCell = Instantiate(smogPrefab, spawnPosition, Quaternion.identity);
            stainedCells.Add(stainedCell);
            stainedCell.transform.SetParent(glassObject.transform);

            float targetStainLvl = Random.Range(dirtTypes[2].minAlpha * (WindowLvl * .1f + 1), (dirtTypes[2].maxAlpha / 5) * WindowLvl);

            // Skalowanie prefabów do rozmiaru komórki i ustawienie przezroczystości na podstawie poziomu plamy
            SpriteRenderer sr = stainedCell.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                Vector2 spriteSize = sr.sprite.bounds.size;
                Vector3 newScale = stainedCell.transform.localScale;
                newScale.x = cellWidth / spriteSize.x * newScale.x;
                newScale.y = cellHeight / spriteSize.y * newScale.y;
                stainedCell.transform.localScale = newScale;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, targetStainLvl);
            }

            DirtCell updated = grid[ix, iy];
            updated.hasDirt = true;
            grid[ix, iy] = updated;
        }
    }

    private void SpawnBirdsDirtOnWindow(int WindowLvl)
    {
        int stainCount = WindowLvl * 2;

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
        foreach (DirtCell cell in cellsToBeStained)
        {
            int ix = (int)cell.cellX;
            int iy = (int)cell.cellY;
            if (ix < 0 || ix >= columns || iy < 0 || iy >= rows)
            {
                continue;
            }

            if (grid[ix, iy].hasDirt)
            {
                continue;
            }

            Vector3 spawnPosition = new Vector3(
                bounds.min.x + cell.cellX * cellWidth + cellWidth / 2,
                bounds.min.y + cell.cellY * cellHeight + cellHeight / 2,
                0);

            GameObject birdPrefab = dirtTypes[0].dirtPrefab;

            stainedCell = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
            stainedCells.Add(stainedCell);
            stainedCell.transform.SetParent(glassObject.transform);

            float targetStainLvl = Random.Range(dirtTypes[0].minAlpha, dirtTypes[0].maxAlpha);

            // Skalowanie prefabów do rozmiaru komórki
            SpriteRenderer sr = stainedCell.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                Vector2 spriteSize = sr.sprite.bounds.size;
                Vector3 newScale = stainedCell.transform.localScale;
                newScale.x = cellWidth / spriteSize.x * newScale.x;
                newScale.y = cellHeight / spriteSize.y * newScale.y;
                stainedCell.transform.localScale = newScale;

                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, targetStainLvl);
            }

            DirtCell updated = grid[ix, iy];
            updated.hasDirt = true;
            grid[ix, iy] = updated;
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
            int ix = (int)cell.cellX;
            int iy = (int)cell.cellY;
            if (ix < 0 || ix >= columns || iy < 0 || iy >= rows)
            {
                continue;
            }

            if (grid[ix, iy].hasDirt)
            {
                continue;
            }

            Vector3 spawnPosition = new Vector3(
                bounds.min.x + cell.cellX * cellWidth + cellWidth / 2,
                bounds.min.y + cell.cellY * cellHeight + cellHeight / 2,
                0);
            GameObject mudPrefab = dirtTypes[1].dirtPrefab;

            stainedCell = Instantiate(mudPrefab, spawnPosition, Quaternion.identity);
            stainedCells.Add(stainedCell);
            stainedCell.transform.SetParent(glassObject.transform);

            float targetStainLvl = Random.Range(dirtTypes[1].minAlpha, dirtTypes[1].maxAlpha);

            // Skalowanie prefabów do rozmiaru komórki
            SpriteRenderer sr = stainedCell.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                Vector2 spriteSize = sr.sprite.bounds.size;
                Vector3 newScale = stainedCell.transform.localScale;
                newScale.x = cellWidth / spriteSize.x * newScale.x;
                newScale.y = cellHeight / spriteSize.y * newScale.y;
                stainedCell.transform.localScale = newScale;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, targetStainLvl);
            }

            DirtCell updated = grid[ix, iy];
            updated.hasDirt = true;
            grid[ix, iy] = updated;
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

    public void JumpScare()
    {
        Vector3 leftEdgeCenter = new Vector3(bounds.min.x, bounds.center.y, 0f);
        Vector3 spawnPosition = leftEdgeCenter + new Vector3(JesusScareOffset.x, JesusScareOffset.y, 0f);
        if (JesusScare != null)
        {
            JesusScare.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            Jessy = Instantiate(JesusScare, spawnPosition, Quaternion.identity);
        }
        else
        {
            return;
        }
    }

    public void CountProgress()
    {
        if (totalCells <= 0)
        {
            clearingProgress = (stainedCells.Count == 0) ? 1f : 0f;
            return;
        }

        float cellsLeft = (float)stainedCells.Count / (float)totalCells;
        clearingProgress = Mathf.Clamp01(1f - cellsLeft);
    }

    public void SpawnDirtOnWindows()
    {
        //StainedCells tworzą się w SpawnDirtOnWindow
        SpawnBirdsDirtOnWindow(WindowLvl);
        SpawnMudOnWindow(WindowLvl);
        SpawnSmogOnWindow(WindowLvl);
        totalCells = stainedCells.Count;
    }

    public void ResetGrid()
    {
        foreach (var cell in stainedCells)
        {
            if (cell != null)
                Destroy(cell);
        }
        stainedCells.Clear();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                DirtCell reset = grid[x, y];
                reset.hasDirt = false;
                reset.dirtType = null;
                grid[x, y] = reset;
            }
        }

        clearingProgress = 0f;
        isCleaned = false;
    }
}
