using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Inventory/Item/Create New ItemInfo")]
public class InventoryItemInfo : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private string _indentificator;
    [SerializeField] private Sprite _spriteIcon; 
    [SerializeField] private bool _isOnInventory;

    public string title => _title;
    public string description => _description;
    public string indentificator => _indentificator;
    public Sprite spriteIcon => _spriteIcon; // свойство дл€ получени€ изображени€ предмета
    public bool isOnInventory => _isOnInventory;

    public void Initialize(string title, string description, string identificator, Sprite sprite, bool isOnInventory)
    {
        _title = title;
        _description = description;
        _indentificator = identificator;
        _spriteIcon = sprite;
        _isOnInventory = isOnInventory;
    }

    
    public static InventoryItemInfo CreateKeyInfo(string title, string description, string identificator, Sprite sprite, bool isOnInventory)
    {
        InventoryItemInfo keyInfo = CreateInstance<InventoryItemInfo>();
        keyInfo.Initialize(title, description, identificator, sprite, isOnInventory);

        return keyInfo;
    }
}
