﻿using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoistureUpset.Skins
{
    // If you see this, this hasn't been fully implemented yet. Keep it a secret. - Rune
    // We don't have any assets made for this skin. I'm just doing some prototyping right now.
    // Please don't expect anything just because you see this.
    public static class JotaroCaptain 
    {
        private static readonly string Name = "Jotaro";
        private static readonly string NameToken = "MOISTURE_UPSET_JOTARO_CAPTAIN_SKIN";

        // Runs on Awake
        public static void Init()
        {
            PopulateAssets();
            On.RoR2.SurvivorCatalog.Init += RegisterSkin;
        }

        // Load assets here
        private static void PopulateAssets()
        {

        }

        // Skindef stuff here
        private static void RegisterSkin(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();

            var survivorDef = SurvivorCatalog.GetSurvivorDef(SurvivorIndex.Captain);
            var bodyPrefab = survivorDef.bodyPrefab;

            var renderers = bodyPrefab.GetComponentsInChildren<Renderer>();
            var skinController = bodyPrefab.GetComponentInChildren<ModelSkinController>();

            var mdl = skinController.gameObject;

            var skin = new LoadoutAPI.SkinDefInfo
            {
                Icon = LoadoutAPI.CreateSkinIcon(Color.black, Color.white, new Color(0.69F, 0.19F, 0.65F, 1F), Color.yellow),
                Name = Name,
                NameToken = NameToken,
                RootObject = mdl,
                BaseSkins = new SkinDef[0],
                UnlockableName = "",
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                RendererInfos = new CharacterModel.RendererInfo[0],
                MeshReplacements = new SkinDef.MeshReplacement[0],
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0]
            };

            Array.Resize(ref skinController.skins, skinController.skins.Length + 1);
            skinController.skins[skinController.skins.Length - 1] = LoadoutAPI.CreateNewSkinDef(skin);

            var skinsField = Reflection.GetFieldValue<SkinDef[][]>(typeof(BodyCatalog), "skins");
            skinsField[BodyCatalog.FindBodyIndex(bodyPrefab)] = skinController.skins;
            Reflection.SetFieldValue(typeof(BodyCatalog), "skins", skinsField);

            LanguageAPI.Add(NameToken, Name);
        }
    }
}