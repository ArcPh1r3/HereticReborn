using System;
using System.Collections.Generic;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace HereticReborn.Modules
{
    internal class ContentPacks
    {
        internal static ContentPack contentPack;

        internal void CreateContentPack()
        {
            contentPack = new ContentPack()
            {
                artifactDefs = new ArtifactDef[0],
                bodyPrefabs = HereticPlugin.bodyPrefabs.ToArray(),
                buffDefs = new BuffDef[0],
                effectDefs = new EffectDef[0],
                eliteDefs = new EliteDef[0],
                entityStateConfigurations = new EntityStateConfiguration[0],
                entityStateTypes = new Type[0],
                equipmentDefs = new EquipmentDef[0],
                gameEndingDefs = new GameEndingDef[0],
                gameModePrefabs = new Run[0],
                itemDefs = new ItemDef[0],
                masterPrefabs = new GameObject[0],
                musicTrackDefs = new MusicTrackDef[0],
                networkedObjectPrefabs = new GameObject[0],
                networkSoundEventDefs = new NetworkSoundEventDef[0],
                projectilePrefabs = new GameObject[0],
                sceneDefs = new SceneDef[0],
                skillDefs = Skills.skillDefs.ToArray(),
                skillFamilies = Skills.skillFamilies.ToArray(),
                surfaceDefs = new SurfaceDef[0],
                survivorDefs = Survivors.survivorDefinitions.ToArray(),
                unlockableDefs = new UnlockableDef[0]
            };

            On.RoR2.ContentManager.SetContentPacks += AddContent;
        }

        private void AddContent(On.RoR2.ContentManager.orig_SetContentPacks orig, List<ContentPack> newContentPacks)
        {
            newContentPacks.Add(contentPack);
            orig(newContentPacks);
        }
    }
}