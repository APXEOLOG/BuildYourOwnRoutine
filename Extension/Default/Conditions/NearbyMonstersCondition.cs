﻿using ImGuiNET;
using PoeHUD.Models.Enums;
using PoeHUD.Poe.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Conditions
{
    internal class NearbyMonstersCondition : ExtensionCondition
    {
        private static Dictionary<String, Tuple<PlayerStats, PlayerStats>> resistanceTypes = new Dictionary<String, Tuple<PlayerStats, PlayerStats>>()
        {
            { "Cold", Tuple.Create(PlayerStats.ColdDamageResistancePct, PlayerStats.MaximumColdDamageResistancePct) },
            { "Fire", Tuple.Create(PlayerStats.FireDamageResistancePct, PlayerStats.MaximumFireDamageResistancePct) },
            { "Lightning",Tuple.Create(PlayerStats.LightningDamageResistancePct, PlayerStats.LightningDamageResistancePct) },
            { "Chaos", Tuple.Create(PlayerStats.ChaosDamageResistancePct, PlayerStats.MaximumChaosDamageResistancePct) }
        };

        private int MinimumMonsterCount { get; set; }
        private readonly String MinimumMonsterCountString = "MinimumMonsterCount";

        private float MaxDistance { get; set; }
        private readonly String MaxDistanceString = "MaxDistance";

        private int ColdResistanceThreshold { get; set; }
        private readonly String ColdResistanceThresholdString = "ColdResistanceThreshold";


        private int FireResistanceThreshold { get; set; }
        private readonly String FireResistanceThresholdString = "FireResistanceThreshold";

        private int LightningResistanceThreshold { get; set; }
        private readonly String LightningResistanceThresholdString = "LightningResistanceThreshold";

        private int ChaosResistanceThreshold { get; set; }
        private readonly String ChaosResistanceThresholdString = "ChaosResistanceThreshold";

        private bool CountWhiteMonsters { get; set; }
        private readonly String CountWhiteMonstersString = "CountWhiteMonsters";

        private bool CountRareMonsters { get; set; }
        private readonly String CountRareMonstersString = "CountRareMonsters";

        private bool CountMagicMonsters { get; set; }
        private readonly String CountMagicMonstersString = "CountMagicMonsters";

        private bool CountUniqueMonsters { get; set; }
        private readonly String CountUniqueMonstersString = "CountUniqueMonsters";

        private int MonsterHealthPercentThreshold { get; set; }
        private readonly String MonsterHealthPercentThresholdString = "MonsterHealthPercentThreshold";

        private bool MonsterAboveHealthThreshold { get; set; }
        private readonly String MonsterAboveHealthThresholdString = "MonsterAboveHealthThreshold";

        public NearbyMonstersCondition(string owner, string name) : base(owner, name)
        {
            MinimumMonsterCount = 0;
            MaxDistance = 0;
            ColdResistanceThreshold = 0;
            FireResistanceThreshold = 0;
            LightningResistanceThreshold = 0;
            ChaosResistanceThreshold = 0;
            CountWhiteMonsters = true;
            CountRareMonsters = true;
            CountMagicMonsters = true;
            CountUniqueMonsters = true;
            MonsterHealthPercentThreshold = 0;
            MonsterAboveHealthThreshold = false;
        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            base.Initialise(Parameters);

            MinimumMonsterCount = Int32.Parse((string)Parameters[MinimumMonsterCountString]);
            MaxDistance = Single.Parse((string)Parameters[MaxDistanceString]);

            ColdResistanceThreshold = Int32.Parse((string)Parameters[ColdResistanceThresholdString]);
            FireResistanceThreshold = Int32.Parse((string)Parameters[FireResistanceThresholdString]);
            LightningResistanceThreshold = Int32.Parse((string)Parameters[LightningResistanceThresholdString]);
            ChaosResistanceThreshold = Int32.Parse((string)Parameters[ChaosResistanceThresholdString]);

            CountWhiteMonsters = Boolean.Parse((string)Parameters[CountWhiteMonstersString]);
            CountRareMonsters = Boolean.Parse((string)Parameters[CountRareMonstersString]);
            CountMagicMonsters = Boolean.Parse((string)Parameters[CountMagicMonstersString]);
            CountUniqueMonsters = Boolean.Parse((string)Parameters[CountUniqueMonstersString]);

            MonsterHealthPercentThreshold = Int32.Parse((string)Parameters[MonsterHealthPercentThresholdString]);

        }

        public override bool CreateConfigurationMenu(ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Condition Info");
            ImGuiExtension.ToolTip("This condition will return true if any of the selected player's resistances\nare reduced by more than or equal to the specified amount.\nReduced max resistance modifiers are taken into effect automatically (e.g. -res map mods).");


            base.CreateConfigurationMenu(ref Parameters);

            MinimumMonsterCount = ImGuiExtension.IntSlider("Minimum Monster Count", MinimumMonsterCount, 1, 50);
            Parameters[MinimumMonsterCountString] = MinimumMonsterCount.ToString();

            MaxDistance = ImGuiExtension.FloatSlider("Maximum Distance", MaxDistance, 1.0f, 100.0f);
            Parameters[MaxDistanceString] = MaxDistance.ToString();

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            ColdResistanceThreshold = ImGuiExtension.IntSlider("Cold Resist Above", ColdResistanceThreshold, 0, 75);
            Parameters[ColdResistanceThresholdString] = ColdResistanceThreshold.ToString();

            FireResistanceThreshold = ImGuiExtension.IntSlider("Fire Resist Above", FireResistanceThreshold, 0, 75);
            Parameters[FireResistanceThresholdString] = FireResistanceThreshold.ToString();

            LightningResistanceThreshold = ImGuiExtension.IntSlider("Lightning Resist Above", LightningResistanceThreshold, 0, 75);
            Parameters[LightningResistanceThresholdString] = LightningResistanceThreshold.ToString();

            ChaosResistanceThreshold = ImGuiExtension.IntSlider("Chaos Resist Above", ChaosResistanceThreshold, 0, 75);
            Parameters[ChaosResistanceThresholdString] = ChaosResistanceThreshold.ToString();

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            CountWhiteMonsters = ImGuiExtension.Checkbox("White Monsters", CountWhiteMonsters);
            Parameters[CountWhiteMonstersString] = CountWhiteMonsters.ToString();

            CountRareMonsters = ImGuiExtension.Checkbox("Rare Monsters", CountRareMonsters);
            Parameters[CountRareMonstersString] = CountRareMonsters.ToString();

            CountMagicMonsters = ImGuiExtension.Checkbox("Magic Monsters", CountMagicMonsters);
            Parameters[CountMagicMonstersString] = CountMagicMonsters.ToString();

            CountUniqueMonsters = ImGuiExtension.Checkbox("Unique Monsters", CountUniqueMonsters);
            Parameters[CountUniqueMonstersString] = CountUniqueMonsters.ToString();

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            MonsterHealthPercentThreshold = ImGuiExtension.IntSlider("Monster Health Percent", MonsterHealthPercentThreshold, 0, 100);
            Parameters[MonsterHealthPercentThresholdString] = MonsterHealthPercentThreshold.ToString();

            MonsterAboveHealthThreshold = ImGuiExtension.Checkbox("Above Health Threshold", MonsterAboveHealthThreshold);
            Parameters[MonsterAboveHealthThresholdString] = MonsterAboveHealthThreshold.ToString();

            return true;
        }

        public override Func<bool> GetCondition(ExtensionParameter extensionParameter)
        {
            return () =>
            {
                var mobCount = 0;
                var maxDistanceSquare = MaxDistance * MaxDistance;

                var playerPosition = extensionParameter.Plugin.GameController.Player.GetComponent<Positioned>();

                foreach (var monster in extensionParameter.Plugin.LoadedMonsters)
                {
                    if (!monster.IsValid || !monster.IsAlive)
                        continue;

                    var monsterType = monster.GetComponent<ObjectMagicProperties>().Rarity;

                    // Don't count this monster type if we are ignoring it
                    if (monsterType == MonsterRarity.White && !CountWhiteMonsters
                        || monsterType == MonsterRarity.Rare && !CountRareMonsters
                        || monsterType == MonsterRarity.Magic && !CountMagicMonsters
                        || monsterType == MonsterRarity.Unique && !CountUniqueMonsters)
                        continue;

                    if (MonsterHealthPercentThreshold > 0)
                    {
                        // If the monster is still too healthy, don't count it
                        var monsterLife = monster.GetComponent<Life>();
                        if (monsterLife.CurHP / monsterLife.MaxHP >= MonsterHealthPercentThreshold)
                            continue;
                    }

                    var monsterPosition = monster.GetComponent<Positioned>();
                    var xDiff = playerPosition.GridX - monsterPosition.GridX;
                    var yDiff = playerPosition.GridY - monsterPosition.GridY;
                    var monsterDistanceSquare = (xDiff * xDiff + yDiff * yDiff);

                    if (monsterDistanceSquare <= maxDistanceSquare)
                    {
                        if (ColdResistanceThreshold > 0 || FireResistanceThreshold > 0 || LightningResistanceThreshold > 0 || ChaosResistanceThreshold > 0)
                        {
                            // We care about resists. Only increment IF we are above the threshold
                            var monsterStats = monster.GetComponent<Stats>();
                            if (ColdResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.ColdDamageResistancePct, out int coldRes) && coldRes >= ColdResistanceThreshold)
                            {
                                mobCount++;
                            }
                            else if (FireResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.FireDamageResistancePct, out int fireRes) && fireRes >= FireResistanceThreshold)
                            {
                                mobCount++;
                            }
                            else if (LightningResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.LightningDamageResistancePct, out int lightningRes) && lightningRes >= LightningResistanceThreshold)
                            {
                                mobCount++;
                            }
                            else if (ChaosResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.ChaosDamageResistancePct, out int chaosRes) && chaosRes >= ChaosResistanceThreshold)
                            {
                                mobCount++;
                            }
                        } else mobCount++;

                    }

                    if (mobCount >= MinimumMonsterCount)
                        return true;
                }

                return false;
            };
        }
    }
}
