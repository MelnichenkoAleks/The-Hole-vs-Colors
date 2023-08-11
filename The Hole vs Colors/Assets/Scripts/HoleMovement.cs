using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    [Header("Hole vertices radius")]
    [SerializeField] Vector2 moveLimits;
    // Радиус вершин от центра отверстия
    [SerializeField] float radius;
    [SerializeField] Transform holeCenter;
    // Вращающийся круг вокруг отверстия (анимация)
    [SerializeField] Transform rotatingCircle;

    [Space]
    [SerializeField] float moveSpeed;

    Mesh mesh;
    List<int> holeVertices;
    // Смещения вершин от центра отверстия
    List<Vector3> offsets;
    int holeVerticesCount;

    float x, y;
    Vector3 touch, targetPos;

    void Start()
    {
        RotateCircleAnim();

        Game.isMoving = false;
        Game.isGameover = false;

        // Инициализация списков
        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        // Получение сетки из компонента meshFilter
        mesh = meshFilter.mesh;

        // Поиск вершин отверстия на сетке
        FindHoleVertices();
    }

    void RotateCircleAnim()
    {
        // Вращение круга вокруг оси Y на -90°
        // Длительность: 0.2 секунды
        // Начальное значение: Vector3 (90f, 0f, 0f)
        // Повторение: -1 (бесконечно)
        rotatingCircle
            .DORotate(new Vector3(90f, 0f, -90f), .2f)
            .SetEase(Ease.Linear)
            .From(new Vector3(90f, 0f, 0f))
            .SetLoops(-1, LoopType.Incremental);
    }

    void Update()
    {
        // Мышь
#if UNITY_EDITOR
        // isMoving=true при каждом нажатии мыши
        // isMoving=false при отпускании мыши
        Game.isMoving = Input.GetMouseButton(0);

        if (!Game.isGameover && Game.isMoving)
        {
            // Движение центра отверстия
            MoveHole();
            // Обновление позиций вершин отверстия
            UpdateHoleVerticesPosition();
        }

        // Сенсорный экран
#else
        // Используется TouchPhase.Moved, чтобы предотвратить прыжок отверстия при первом касании
        Game.isMoving = Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved;

        if (!Game.isGameover && Game.isMoving) {
            // Движение центра отверстия
            MoveHole ();
            // Обновление позиций вершин отверстия
            UpdateHoleVerticesPosition ();
        }
#endif
    }

    void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        // Плавное (lerp) движение
        touch = Vector3.Lerp(
            holeCenter.position,
            holeCenter.position + new Vector3(x, 0f, y), // Движение отверстия по X и Z
            moveSpeed * Time.deltaTime
        );

        targetPos = new Vector3(
            // Ограничение, чтобы отверстие не выходило за пределы земли
            Mathf.Clamp(touch.x, -moveLimits.x, moveLimits.x), // Ограничение по X
            touch.y,
            Mathf.Clamp(touch.z, -moveLimits.y, moveLimits.y) // Ограничение по Z
        );

        holeCenter.position = targetPos;
    }

    void UpdateHoleVerticesPosition()
    {
        // Обновление позиций вершин отверстия
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }

        // Обновление вершин сетки
        mesh.vertices = vertices;
        // Обновление сетки в meshFilter
        meshFilter.mesh = mesh;
        // Обновление коллайдера
        meshCollider.sharedMesh = mesh;
    }

    void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            // Расчет расстояния между центром отверстия и каждой вершиной
            float distance = Vector3.Distance(holeCenter.position, mesh.vertices[i]);

            if (distance < radius)
            {
                // Эта вершина принадлежит отверстию
                holeVertices.Add(i);
                // Смещение: насколько далеко вершина от центра отверстия
                offsets.Add(mesh.vertices[i] - holeCenter.position);
            }
        }
        // Сохранение количества вершин отверстия
        holeVerticesCount = holeVertices.Count;
    }

    // Визуализация радиуса вершин отверстия в режиме Scene
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holeCenter.position, radius);
    }
}