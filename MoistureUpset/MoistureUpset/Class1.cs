﻿using BepInEx;
using R2API.Utils;
using RoR2;
using R2API;
using R2API.MiscHelpers;
using System.Reflection;
using static R2API.SoundAPI;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace MoistureUpset
{
    [BepInDependency("com.bepis.r2api")]
    //Change these
    [BepInPlugin("com.WetBoys.WetGamers", "We are really wet.", "0.6.9")]
    [R2APISubmoduleDependency("SoundAPI", "PrefabAPI", "CommandHelper", "LoadoutAPI", "SurvivorAPI", "ResourcesAPI")]
    public class BigTest : BaseUnityPlugin
    {
        public void Awake()
        {
            Assets.PopulateAssets();

            SurvivorLoaderAPI.LoadSurvivors();

            BigToasterClass.RunAll();

            SoundAssets.RegisterSoundEvents();

            NetworkAssistant.InitSNA();

            EnemyReplacements.RunAll();
            //UnReady.Init();

            On.RoR2.UI.CharacterSelectController.SelectSurvivor += CharacterSelectController_SelectSurvivor;

            On.RoR2.TeleporterInteraction.Awake += TeleporterInteraction_Awake;

            //ligmaballs();
        }

        public static void ligmaballs()
        {
            var fortniteDance = Resources.Load<AnimationClip>("@MoistureUpset_fortnite:assets/dancemoves.anim");
            var fab = Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");

            //foreach (var item in fab.GetComponentsInChildren<Component>())
            //{
            //    Debug.Log($"--------------------------------------------------{item}");
            //}

            var anim = fab.GetComponentInChildren<Animator>();

            Debug.Log($"++++++++++++++++++++++++++++++++++++++++{anim}");

            //AnimatorController anim = new AnimatorController
            AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);

            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
            }
            aoc.ApplyOverrides(anims);
            anim.runtimeAnimatorController = aoc;
        }

        public void Start()
        {
            RoR2.Console.instance.SubmitCmd((NetworkUser)null, "set_scene title");
        }

        private void TeleporterInteraction_Awake(On.RoR2.TeleporterInteraction.orig_Awake orig, TeleporterInteraction self)
        {
            //self.shouldAttemptToSpawnShopPortal = true;
            //self.Network_shouldAttemptToSpawnShopPortal = true;
            //self.baseShopSpawnChance = 1;

            orig(self);

            //self.shouldAttemptToSpawnShopPortal = true;
            //self.Network_shouldAttemptToSpawnShopPortal = true;
        }

        private void CharacterSelectController_SelectSurvivor(On.RoR2.UI.CharacterSelectController.orig_SelectSurvivor orig, RoR2.UI.CharacterSelectController self, SurvivorIndex survivor)
        {
            self.selectedSurvivorIndex = survivor;

            if (survivor == SurvivorIndex.Commando)
            {
                AkSoundEngine.PostEvent("YourMother", self.characterDisplayPads[0].displayInstance.gameObject);
            }

            orig(self, survivor);
        }
    }
}
