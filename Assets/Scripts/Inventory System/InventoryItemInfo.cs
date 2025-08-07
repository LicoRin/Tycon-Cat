using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Inventory/Item/Create New ItemInfo")]
public class InventoryItemInfo : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private string _indentificator;
    [SerializeField] private Sprite _spriteIcon; 
    [SerializeField] private bool _isOnInventory;
    [SerializeField] private int _amount;

    public string title => _title;
    public string description => _description;
    public string indentificator => _indentificator;
    public Sprite spriteIcon => _spriteIcon; // свойство для получения изображения предмета
    public bool isOnInventory => _isOnInventory;
    public int amount => _amount;
    public void DecreaseAmount(int value)
    {
        _amount = Mathf.Max(0, _amount - value); // чтобы не ушло в минус
    }

    public void Initialize(string title, string description, string identificator, Sprite sprite, bool isOnInventory, int amount)
    {
        _title = title;
        _description = description;
        _indentificator = identificator;
        _spriteIcon = sprite;
        _isOnInventory = isOnInventory;
        _amount = amount;
    }

    
    public static InventoryItemInfo CreateKeyInfo(string title, string description, string identificator, Sprite sprite, bool isOnInventory, int amount)
    {
        InventoryItemInfo keyInfo = CreateInstance<InventoryItemInfo>();
        keyInfo.Initialize(title, description, identificator, sprite, isOnInventory, amount);

        return keyInfo;
    }
}
