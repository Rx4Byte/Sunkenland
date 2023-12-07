using UnityEngine;
using BepInEx;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MouseWheelForQuickSlots
{
    public class PluginInfo
    {
        public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Mouse Wheel for Quick slots";
        public const string PLUGIN_GUID = "com.Rx4Byte.MouseWheelForQuickSlots";
        public const string PLUGIN_VERSION = "1.0.0";
    }
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class MouseWheelForQuickSlots : BaseUnityPlugin
    {
        // prepare UICombat and PlayerCharacter
        private UICombat uicombat;
        private PlayerCharacter player;

        void Update()
        {
            // check/get instance of PlayerCharacter
            if (player == null)
                player = FindObjectOfType<PlayerCharacter>();
            // check/get instance of UICombat
            if (uicombat == null)
                if (FindObjectOfType<UICombat>())
                    uicombat = FindObjectOfType<UICombat>();
            // safety check
            if (player == null || uicombat == null) return;
            // get Mouse Wheel Scroll Delta
            float mouseScrollDelta = UnityEngine.Input.mouseScrollDelta.y;
            // check for up or down changes
            if (mouseScrollDelta != 0 && !UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt))
            {
                // check if QuickSlotIndex is valid for use
                if (!uicombat.QuickSlotIndex.HasValue)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        uicombat.QuickSlotIndex = i;
                        if (!uicombat.SelectedQuickSlotItem.GetComponent<Food>())
                            break;
                    }
                    // refresh quick slot
                    uicombat.RefreshQuickSlot();
                }
                // if up
                if (mouseScrollDelta > 0)
                {
                    // quick slot up
                    if (uicombat.QuickSlotIndex == 7)
                        uicombat.QuickSlotIndex = 0;
                    else
                        uicombat.QuickSlotIndex++;
                    // refresh quick slot
                    uicombat.RefreshQuickSlot();
                    // quick slot up again if slot is food item
                    if (uicombat.SelectedQuickSlotItem.GetComponent<Food>())
                    {
                        if (uicombat.QuickSlotIndex == 7)
                            uicombat.QuickSlotIndex = 0;
                        else
                            uicombat.QuickSlotIndex++;
                    }
                }
                // else down
                else
                {
                    // quick slot down
                    if (uicombat.QuickSlotIndex == 0)
                        uicombat.QuickSlotIndex = 7;
                    else
                        uicombat.QuickSlotIndex--;
                    // refresh quick slot
                    uicombat.RefreshQuickSlot();
                    // quick slot down again if slot is food item
                    if (uicombat.SelectedQuickSlotItem.GetComponent<Food>())
                    {
                        if (uicombat.QuickSlotIndex == 0)
                            uicombat.QuickSlotIndex = 7;
                        else
                            uicombat.QuickSlotIndex--;
                    }
                }
                // refresh quick slot
                uicombat.RefreshQuickSlot();
                // equip item
                if (uicombat.SelectedQuickSlotItem.GetComponent<BuildingPiece>() ||
                        uicombat.SelectedQuickSlotItem.GetComponent<WeaponRaycast>() ||
                        uicombat.SelectedQuickSlotItem.GetComponent<BuildingContainer>())
                    player.UseItem(uicombat.SelectedQuickSlotItem, false);

            }
        }
    }
}