using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerScript : MonoBehaviour
    {
        [Header("Public")]
        public PlayerStats playerStats;
        public Item itemHeld;
        public Inventory inventory;
        public PlayerAudio playerAudio;
        public PlayerMovement playerMovement;
        public PlayerAnimationController playerAnimationController;
        public PlayerInputChecker playerInputChecker;

        public int PlayerID;

        [Header("Serialize Field")]
        [SerializeField] Image draggableItem;
        [SerializeField] Image selectUISprite;

        private BaseItemSlot _dragItemSlot;

        private int lastNumHeld = 0;

        private void Awake()
        {
            PlayerID = ServiceLocator.Current.Get<PlayersManager>().AddPlayer(this);
            
            // Begin Drag
            inventory.OnBeginDragEvent += BeginDrag;
            
            // End Drag
            inventory.OnEndDragEvent += EndDrag;
            
            // Drag
            inventory.OnDragEvent += Drag;
            
            // Drop
            inventory.OnDropEvent += Drop;
        }

        // Selects players item from inventory based on parameter
        public void SelectItem(int itemSelectNum)
        {
            lastNumHeld = itemSelectNum;
            itemHeld = inventory.ItemSlots[itemSelectNum].Item;
            Vector3 selectUIPosition = inventory.ItemSlots[itemSelectNum].transform.position;
            selectUISprite.transform.position = selectUIPosition;
            //Debug.Log(itemSelectNum);
        }

        private void Update()
        {
            SelectItem(lastNumHeld);
        }

        private void BeginDrag(BaseItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                _dragItemSlot = itemSlot;
                draggableItem.sprite = itemSlot.Item.icon;
                draggableItem.transform.position = Input.mousePosition;
                draggableItem.gameObject.SetActive(true);
            }
        }
        
        private void Drag(BaseItemSlot itemSlot)
        {
            draggableItem.transform.position = Input.mousePosition;
        }

        private void EndDrag(BaseItemSlot itemSlot)
        {
            _dragItemSlot = null;
            draggableItem.gameObject.SetActive(false);
        }

        private void Drop(BaseItemSlot dropItemSlot)
        {
            if (_dragItemSlot == null) return;

            if (dropItemSlot.CanAddStack(_dragItemSlot.Item))
            {
                AddStacks(dropItemSlot);
            }
            else if (dropItemSlot.CanReceiveItem(_dragItemSlot.Item) && _dragItemSlot.CanReceiveItem(dropItemSlot.Item))
            {
                SwapItems(dropItemSlot);
            }
        }
        
        private void AddStacks(BaseItemSlot dropItemSlot)
        {
            int numAddableStacks = dropItemSlot.Item.maximumStack - dropItemSlot.Amount;
            int stacksToAdd = Mathf.Min(numAddableStacks, _dragItemSlot.Amount);

            dropItemSlot.Amount += stacksToAdd;
            _dragItemSlot.Amount -= stacksToAdd;
        }
        
        private void SwapItems(BaseItemSlot dropItemSlot)
        {
            Item draggedItem = _dragItemSlot.Item;
            int draggedItemAmount = _dragItemSlot.Amount;

            _dragItemSlot.Item = dropItemSlot.Item;
            _dragItemSlot.Amount = dropItemSlot.Amount;

            dropItemSlot.Item = draggedItem;
            dropItemSlot.Amount = draggedItemAmount;
        }
    }
}
