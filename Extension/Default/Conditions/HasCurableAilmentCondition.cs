﻿using ImGuiNET;
using PoeHUD.Poe.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Menu;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Conditions
{
    internal class HasCurableAilmentCondition : ExtensionCondition
    {
        public bool RemFrozen { get; set; } = false;
        public string RemFrozenString { get; set; } = "RemFrozen";

        public bool RemBurning { get; set; } = false;
        public string RemBurningString { get; set; } = "RemBurning";

        public bool RemShocked { get; set; } = false;
        public string RemShockedString { get; set; } = "RemShocked";

        public bool RemCurse { get; set; } = false;
        public string RemCurseString { get; set; } = "RemCurse";

        public bool RemPoison { get; set; } = false;
        public string RemPoisonString { get; set; } = "RemPoison";

        public bool RemBleed { get; set; } = false;
        public string RemBleedString { get; set; } = "RemBleed";

        public int CorruptCount { get; set; } = 0;
        public string CorruptCountString { get; set; } = "CorruptCount";


        public HasCurableAilmentCondition(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Condition Info");
            ImGui.SetTooltip("This condition will return true if the player has any of the selected ailments or a minimum of the specified corrupted blood stacks.");

            base.Initialise(Parameters);

            RemFrozen = Boolean.Parse((String)Parameters[RemFrozenString]);
            RemBurning = Boolean.Parse((String)Parameters[RemBurningString]);
            RemShocked = Boolean.Parse((String)Parameters[RemShockedString]);
            RemCurse = Boolean.Parse((String)Parameters[RemCurseString]);
            RemPoison = Boolean.Parse((String)Parameters[RemPoisonString]);
            RemBleed = Boolean.Parse((String)Parameters[RemBleedString]);
            CorruptCount = Int32.Parse((String)Parameters[CorruptCountString]);

        }

        public override bool CreateConfigurationMenu(ref Dictionary<String, Object> Parameters)
        {
            base.CreateConfigurationMenu(ref Parameters);

            RemFrozen = ImGuiExtension.Checkbox("Frozen", RemFrozen);
            Parameters[RemFrozenString] = RemFrozen.ToString();

            RemBurning = ImGuiExtension.Checkbox("Burning", RemBurning);
            Parameters[RemBurningString] = RemBurning.ToString();

            RemShocked = ImGuiExtension.Checkbox("Shocked", RemShocked);
            Parameters[RemShockedString] = RemShocked.ToString();

            RemCurse = ImGuiExtension.Checkbox("Curse", RemCurse);
            Parameters[RemCurseString] = RemCurse.ToString();

            RemPoison = ImGuiExtension.Checkbox("Poison", RemPoison);
            Parameters[RemPoisonString] = RemPoison.ToString();

            RemBleed = ImGuiExtension.Checkbox("Bleed", RemBleed);
            Parameters[RemBleedString] = RemBleed.ToString();

            CorruptCount = ImGuiExtension.IntSlider("Corruption Count", CorruptCount, 0, 20);
            Parameters[CorruptCountString] = CorruptCount.ToString();
            return true;
        }

        public override Func<bool> GetCondition(ExtensionParameter profileParameter)
        {
            return () =>
            {
                if (RemFrozen && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Frozen))
                    return true;
                if (RemBurning && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Burning))
                    return true;
                if (RemShocked && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Shocked))
                    return true;
                if (RemCurse && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.WeakenedSlowed))
                    return true;
                if (RemPoison && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Poisoned))
                    return true;
                if (RemBleed && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Bleeding))
                    return true;
                if (CorruptCount > 0 && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Corruption, () => CorruptCount))
                    return true;
                if (RemBurning && hasAilment(profileParameter, profileParameter.Plugin.Cache.DebuffPanelConfig.Burning))
                    return true;

                return false;
            };
        }

        private bool hasAilment(ExtensionParameter profileParameter, Dictionary<string, int> dictionary, Func<int> minCharges = null)
        {
            var buffs = profileParameter.Plugin.Cache.SavedIngameState.Data.LocalPlayer.GetComponent<Life>().Buffs;
            foreach (var buff in buffs)
            {
                if (float.IsInfinity(buff.Timer))
                    continue;

                int filterId = 0;
                if (dictionary.TryGetValue(buff.Name, out filterId))
                {
                    // I'm not sure what the values are here, but this is the effective logic from the old plugin
                    return (filterId == 0 || filterId != 1) && (minCharges == null || buff.Charges >= minCharges());
                }
            }
            return false;
        }
    }
}
