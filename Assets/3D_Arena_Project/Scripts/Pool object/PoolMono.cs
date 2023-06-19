using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T : MonoBehaviour
{
    public T Prefab { get; }
    public bool PutoExpand { get; set; }
    public Transform Pontainer { get; }
    public PlayerModel PlayerModel { get; }

    private List<T> _pool;
    public PoolMono(T prefab, int count)
    {
        Prefab = prefab;
        Pontainer = null;

        CreatePool(count);
    }

    public PoolMono(T prefab, int count, Transform container, PlayerModel playerModel)
    {
        Prefab = prefab;
        Pontainer = container;
        PlayerModel = playerModel;

        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createObject = Object.Instantiate(this.Prefab, this.Pontainer);
        createObject.transform.position = Pontainer.position;
        createObject.GetComponent<Bullet>().SetData(Pontainer, PlayerModel);
        createObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createObject);
        return createObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in _pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var freeElement))
            return freeElement;

        if (PutoExpand)
            return this.CreateObject(true);

        throw new System.Exception("null");
    }
}
