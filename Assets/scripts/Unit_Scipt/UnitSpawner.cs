using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUnitCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnitCard unitCard; // Посилання на дані юніта

    private Vector3 startPosition; // Початкова позиція
    private Transform parentTransform; // Початковий батьківський об'єкт
    private Canvas canvas; // Головний Canvas
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private spawnZone_Not spawnzone_Not;

    void Start()
    {
        startPosition = transform.position;
        parentTransform = transform.parent;

        // Шукаємо найближчий Canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            
        }

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // Дозволяє клікати крізь карту
        canvasGroup.alpha = 0.8f; // Робимо карту трохи прозорою
        transform.SetParent(canvas.transform, true); // Переміщуємо карту, але збережемо позицію
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Перетворюємо координати миші в локальні координати UI
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out localPoint);
        rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Відновлюємо блокування
        canvasGroup.alpha = 1f; // Робимо карту повністю видимою

        // Отримуємо позицію миші у світовій системі координат
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        // Виконуємо 2D Raycast
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("SpawnZone"))
        {
            SpawnZone spawnZone = hit.collider.GetComponent<SpawnZone>();

            if (spawnZone != null && !spawnZone.IsOccupied()) // Перевіряємо, чи клітинка вільна
            {
                SpawnUnit(spawnZone);
            }
        }

        // Повертаємо карту на початкове місце
        transform.SetParent(parentTransform, false);
        transform.position = startPosition;
    }

    void SpawnUnit(SpawnZone spawnZone)
    {
        if (unitCard == null || unitCard.unitPrefab == null)
        {
            return;
        }

        // Отримуємо центр клітинки для точного розміщення
        Vector3 spawnZoneCenter = spawnZone.GetCenter();

        // Визначаємо точку спавну зліва за камерою, на висоті клітинки
        float offsetX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 2f; // Камера + відступ
        Vector3 spawnPosition = new Vector3(offsetX, spawnZoneCenter.y, 0);

        // Спавнимо юніта
        GameObject spawnzone_Not = Instantiate(unitCard.unitPrefab, spawnPosition, Quaternion.identity);
        spawnZone.SetUnit(spawnzone_Not); // Призначаємо юніта клітинці

        // Додаємо логіку очищення клітинки, коли юніт буде знищений
        spawnZone_Not unitScript = spawnzone_Not.GetComponent<spawnZone_Not>();
        if (unitScript != null)
        {
            unitScript.SetSpawnZone(spawnZone);
        }

        // Додаємо рух юніта до центру клітинки
        UnitMovement movement = spawnzone_Not.GetComponent<UnitMovement>();
       if (movement != null)
       {
    // Викликаємо SetTarget з однією позицією, а швидкість передаємо окремо
        movement.SetTarget(spawnZoneCenter);  // spawnZoneCenter має бути типом Vector3
       }

// Ініціалізуємо характеристики юніта
     UnitStats stats = spawnzone_Not.GetComponent<UnitStats>();
    if (stats != null)
   {
    stats.Initialize(unitCard.health, unitCard.damage);
   }

    }
}
