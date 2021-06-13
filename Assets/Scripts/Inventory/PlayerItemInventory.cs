using SketchFleets.ProfileSystem;

namespace SketchFleets.Inventory
{
    public class PlayerItemInventory : ItemInventory
    {
        public PlayerItemInventory(int slots) : base(slots)
        {
            
        }

        public override int AddItem(ItemStack stack)
        {
            if (!CanAddItem(stack))
                return stack.Amount;

            //Add id
            Profile.GetData().codex.AddItem(new CodexEntry(CodexEntryType.Item, CodexEntryRarity.Silver, stack.Id));

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = stack;
                    return 0;
                }

                if (items[i].Equals(stack))
                {
                    items[i].Amount += stack.Amount;
                    return 0;
                }
            }

            return stack.Amount;
        }
    }
}