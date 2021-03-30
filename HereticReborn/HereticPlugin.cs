using BepInEx;
using RoR2;
using System.Security;
using System.Security.Permissions;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.Networking;
using EnigmaticThunder.Modules;
using System.Collections.Generic;
using RoR2.Skills;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace HereticReborn
{
    [BepInDependency("com.EnigmaDev.EnigmaticThunder", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.birb.HereticReborn", "HereticReborn", "0.0.1")]

    public class HereticPlugin : BaseUnityPlugin
    {
        internal static GameObject bodyPrefab;
        internal static GameObject displayPrefab;
        internal static SurvivorDef hereticSurvivorDef;

        internal static List<GameObject> bodyPrefabs = new List<GameObject>();

        public void Awake()
        {
            InitializePrefab();
            InitializeSkins();
            InitializeSkills();
            InitializeSurvivor();

            new Modules.ContentPacks().CreateContentPack();
        }

        public void Start()
        {
            YoinkGlobalSkillDefs();
        }

        private static void InitializePrefab()
        {
            bodyPrefab = Prefabs.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/HereticBody"), "HereticRebornBody");
            bodyPrefabs.Add(bodyPrefab);

            displayPrefab = Prefabs.InstantiateClone(bodyPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "HereticRebornDisplay");
            displayPrefab.transform.localScale = Vector3.one * 0.7f;
            displayPrefab.AddComponent<NetworkIdentity>();
            displayPrefab.AddComponent<Components.HereticMenuAnimation>();
        }

        private static void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            // meh
            /*SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

            });*/
        }

        private static void YoinkGlobalSkillDefs()
        {
            Modules.Skills.AddPrimarySkill(bodyPrefab, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")));
            Modules.Skills.AddSecondarySkill(bodyPrefab, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")));
            Modules.Skills.AddUtilitySkill(bodyPrefab, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")));
            Modules.Skills.AddSpecialSkill(bodyPrefab, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarDetonatorSpecialReplacement")));
        }

        private static void InitializeSurvivor()
        {
            hereticSurvivorDef = Modules.Survivors.RegisterNewSurvivor(new Modules.Survivors.SurvivorDefInfo
            {
                bodyPrefab = bodyPrefab,
                displayPrefab = displayPrefab,
                primaryColor = Color.grey,
                displayNameToken = "Heretic",
                descriptionToken = "description",
                outroFlavorToken = "..and so they left, whatever the hell this ending line is",
                mainEndingEscapeFailureFlavorToken = "..something somethnig",
                desiredSortPosition = 30f,
                hidden = false,
                unlockableDef = null
            });
        }

        private static void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            DestroyImmediate(model.GetComponent<ModelSkinController>());
            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("Default",
                Loadouts.CreateSkinIcon(new Color(0.8f, 0.8f, 0.8f), new Color(0.8f, 0.8f, 0.8f), new Color(0.8f, 0.8f, 0.8f), new Color(0.8f, 0.8f, 0.8f)),
                defaultRenderers,
                mainRenderer,
                model);

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            SkinDef masterySkin = Modules.Skins.CreateSkinDef("Mastery",
                Loadouts.CreateSkinIcon(new Color(0.15f, 0.15f, 0.15f), new Color(0.15f, 0.15f, 0.15f), new Color(0.15f, 0.15f, 0.15f), new Color(0.15f, 0.15f, 0.15f)),
                defaultRenderers,
                mainRenderer,
                model);

            skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}