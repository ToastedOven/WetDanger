﻿using BepInEx;
using R2API.Utils;
using RoR2;
using R2API;
using R2API.MiscHelpers;
using System.Reflection;
using static R2API.SoundAPI;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions;
using AK;
using System.Collections;
using MoistureUpset.NetMessages;
using R2API.Networking.Interfaces;

namespace MoistureUpset
{
    public static class BigToasterClass // Why do we still call this "Big Toaster Class"?
    {
        public static void RunAll()
        {
            Somebody();
            BossMusic();
            BossMusicAndFanFare();
            OnHit();
            PreGame();
            DeathRespawn();
            PlayerDeath();
            DifficultyIcons();
            DoppelGangerFix();
            EnemyReplacements.LoadBNK("MusicReplacements");
        }
        public static void DeathRespawn()
        {
            if (BigJank.getOptionValue("Respawn SFX", "Audio"))
                EnemyReplacements.LoadBNK("Respawn");
        }

        public static void DoppelGangerFix()
        {
            On.EntityStates.GenericCharacterDeath.OnEnter += (orig, self) =>
            {
                orig(self);
                try
                {
                    if (BigJank.getOptionValue("Player death sound", "Audio"))
                        if (self.outer.gameObject.GetComponentInChildren<RoR2.PositionIndicator>() && self.outer.gameObject.GetComponentInChildren<RoR2.PositionIndicator>().name == "PlayerPositionIndicator(Clone)")
                        {
                            AkSoundEngine.PostEvent("PlayerDeath", self.outer.gameObject);
                        }
                }
                catch (Exception)
                {
                }
            };
        }

        public static void DifficultyIcons()
        {
            if (BigJank.getOptionValue("Difficulty Icons", "UI Changes"))
            {
                UImods.ReplaceUIBetter("Textures/DifficultyIcons/texDifficultyEasyIcon", "MoistureUpset.Resources.easy.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEasyIconDisabled", "MoistureUpset.Resources.easyDisabled.png");

                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyNormalIcon", "MoistureUpset.Resources.medium.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyNormalIconDisabled", "MoistureUpset.Resources.mediumDisabled.png");

                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyHardIcon", "MoistureUpset.Resources.hard.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyHardIconDisabled", "MoistureUpset.Resources.hardDisabled.png");

                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse1Icon", "MoistureUpset.Resources.e1.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse2Icon", "MoistureUpset.Resources.e2.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse3Icon", "MoistureUpset.Resources.e3.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse4Icon", "MoistureUpset.Resources.e4.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse5Icon", "MoistureUpset.Resources.e5.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse6Icon", "MoistureUpset.Resources.e6.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse7Icon", "MoistureUpset.Resources.e7.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEclipse8Icon", "MoistureUpset.Resources.e8.png");
            }

            if (BigJank.getOptionValue("Pizza Roll", "UI Changes"))
            {
                byte[] bytes = ByteReader.readbytes("MoistureUpset.Resources.pizzaroll.png");
                var r = Resources.LoadAll<GameObject>("prefabs/ui");
                foreach (var sex in r)
                {
                    foreach (var item in sex.GetComponentsInChildren<UnityEngine.UI.Image>())
                    {
                        try
                        {
                            //Debug.Log($"89-------{item.name}");
                            if (item.name == "Checkbox")
                            {
                                item.overrideSprite.texture.LoadImage(bytes);
                            }
                        }
                        catch (Exception)
                        {
                        }

                    }
                }
            }
        }
        public static void PlayerDeath()
        {
            if (BigJank.getOptionValue("Player death chat", "Misc"))
                On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, report, user) =>
            {
                orig(self, report, user);
                try
                {
                    if (!user)
                    {
                        return;
                    }
                    List<string> quotes = new List<string> { "I wasn't even trying", "If ya'll would help me I wouldn't have died...", "Nice one hit protection game", "HOW DID I DIE?????", "The first game was better", "Whatever", "Yeah alright, thats cool" };
                    if (BigJank.getOptionValue("NSFW", "Misc"))
                    {
                        quotes.Add("I fucking hate this game");
                    }
                    if (report.attackerMaster.name.ToUpper().Contains("MAGMAWORM"))
                    {
                        for (int i = 0; i < 3; i++)
                            quotes.Add("The magma worm is such bullshit");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("ELECTRICWORM"))
                    {
                        for (int i = 0; i < 3; i++)
                            quotes.Add("Why does it get lightning? It's already strong enough");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("BROTHERHURT"))
                    {
                        for (int i = 0; i < 3; i++)
                            quotes.Add("This final phase sucks so much");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("WISPMASTER"))
                    {
                        for (int i = 0; i < 3; i++)
                            quotes.Add("Unfucking dodgeable");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("VAGRANT"))
                    {
                        for (int i = 0; i < 3; i++)
                            quotes.Add("How are you supposed to dodge that????");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("LEMURIANBRUISERMASTER"))
                    {
                        for (int i = 0; i < 3; i++)
                            quotes.Add("The fire breath is so annoying");
                    }
                    //else if (UnityEngine.Random.Range(0, 1000) == 5)//maybe have a dummy super rare easter egg?
                    //{
                    //    quotes.Add("");
                    //}
                    Chat.SendBroadcastChat(new Chat.UserChatMessage
                    {
                        sender = user.gameObject,
                        text = quotes[UnityEngine.Random.Range(0, quotes.Count - 1)],
                    });
                }
                catch (Exception)
                {
                }
            };
        }
        public static void PreGame()
        {
            On.RoR2.UI.PregameCharacterSelection.Awake += (orig, self) =>
            {
                orig(self);
                AkSoundEngine.SetRTPCValue("MainMenuMusic", 0);
            };
            On.RoR2.SceneCatalog.OnActiveSceneChanged += (orig, oldS, newS) =>
            {
                brother = 0;
                var sugondeez = Resources.Load<RoR2.InteractableSpawnCard>("spawncards/interactablespawncard/iscChest1");
                if (sugondeez.prefab.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.name != "smallchest")
                {
                    //Debug.Log($"--------reloading chests");
                    InteractReplacements.Interactables.ReloadChests();
                }

                EnemyReplacements.kindlyKillYourselfRune = true;
                AkSoundEngine.SetRTPCValue("Dicks", 0);
                if (BigJank.getOptionValue("Nyan Cat", "Enemy Skins"))
                {
                    var fab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/BeetleWard");
                    fab.GetComponentsInChildren<SkinnedMeshRenderer>()[0].sharedMesh = Resources.Load<Mesh>("@MoistureUpset_beetlequeen:assets/bosses/Poptart.mesh");
                    fab.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = Resources.Load<Material>("@MoistureUpset_beetlequeen:assets/bosses/nyancat.mat");
                }
                if (BigJank.getOptionValue("Taco Bell", "Enemy Skins"))
                    EnemyReplacements.ReplaceMeshRenderer(EntityStates.Bell.BellWeapon.ChargeTrioBomb.preppedBombPrefab, "@MoistureUpset_tacobell:assets/toco.mesh", "@MoistureUpset_tacobell:assets/toco.png");
                if (BigJank.getOptionValue("Toad", "Enemy Skins"))
                {
                    EnemyReplacements.ReplaceParticleSystemmesh(EntityStates.MiniMushroom.SporeGrenade.chargeEffectPrefab, "@MoistureUpset_toad1:assets/toadbombfull.mesh", 1);
                    var skin = EntityStates.MiniMushroom.SporeGrenade.chargeEffectPrefab.GetComponentsInChildren<ParticleSystemRenderer>()[1];
                    skin.sharedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    skin.sharedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    skin.sharedMaterial.SetInt("_ZWrite", 0);
                    skin.sharedMaterial.DisableKeyword("_ALPHATEST_ON");
                    skin.sharedMaterial.DisableKeyword("_ALPHABLEND_ON");
                    skin.sharedMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    skin.sharedMaterial.renderQueue = 3000;
                }

                if (BigJank.getOptionValue("Roblox Titan", "Enemy Skins"))
                    EnemyReplacements.ReplaceTexture("prefabs/characterbodies/TitanBody", "@MoistureUpset_roblox:assets/robloxtitan.png");
                if (BigJank.getOptionValue("Sans", "Enemy Skins"))
                    EntityStates.ImpBossMonster.GroundPound.slamEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().mesh = null;
                StopBossMusic(new UInt32[] { 311764514, 405315856, 829504566, 1557982612, 4106775434 });
                StopBossMusic(new UInt32[] { 3605238269, 3605238270, 3605238271, 3605238264, 3179516522, 4044558886, 2244734173, 2339617413, 3772119855, 2493198437, 291592398, 2857659536, 3163719647, 1581288698, 974987421, 2337675311, 696983880, 454706293, 541788247 });
                orig(oldS, newS);
                try
                {
                    switch (newS.name)
                    {
                        case "logbook":
                            AkSoundEngine.SetRTPCValue("MainMenuMusic", 0f);
                            break;
                        case "title":
                            if (BigJank.getOptionValue("Shreks outhouse", "Misc"))
                            {
                                GameObject pod = GameObject.Find("SurvivorPod");
                                pod.GetComponentsInChildren<MeshFilter>()[0].sharedMesh = Resources.Load<Mesh>("@MoistureUpset_shreklet:assets/shreklet.mesh");
                                pod = GameObject.Find("EscapePodDoorMesh");
                                pod.GetComponentsInChildren<MeshFilter>()[0].sharedMesh = Resources.Load<Mesh>("@MoistureUpset_shreklet:assets/shrekletdoor.mesh");
                            }
                            if (BigJank.getOptionValue("Main menu music", "Audio"))
                                AkSoundEngine.PostEvent("PlayMainMenu", GameObject.FindObjectOfType<GameObject>());
                            AkSoundEngine.SetRTPCValue("MainMenuMusic", 1);
                            AkSoundEngine.SetRTPCValue("LobbyActivated", 1);
                            break;
                        case "lobby":
                            AkSoundEngine.SetRTPCValue("LobbyActivated", 1);
                            AkSoundEngine.SetRTPCValue("MainMenuMusic", 0f);
                            break;
                        case "splash":
                            AkSoundEngine.SetRTPCValue("MainMenuMusic", 0);
                            break;
                        case "eclipseworld":
                            AkSoundEngine.SetRTPCValue("LobbyActivated", 0);
                            break;
                        case "outro":
                            break;
                        default:
                            AkSoundEngine.SetRTPCValue("LobbyActivated", 0);
                            break;
                    }
                }
                catch (Exception)
                {
                }
                AkSoundEngine.ExecuteActionOnEvent(1462303513, AkActionOnEventType.AkActionOnEventType_Stop);
                AkSoundEngine.ExecuteActionOnEvent(816301922, AkActionOnEventType.AkActionOnEventType_Stop);
                AkSoundEngine.ExecuteActionOnEvent(1214003200, AkActionOnEventType.AkActionOnEventType_Stop);
                AkSoundEngine.ExecuteActionOnEvent(1593864692, AkActionOnEventType.AkActionOnEventType_Stop);
                AkSoundEngine.SetRTPCValue("BossMusicActive", 0);
                //logbook
                //title
                //lobby
                //
                //
                //
            };
            On.RoR2.MusicController.UpdateState += (orig, self) =>
            {
                //muMenu
                orig(self);
                var c = GameObject.FindObjectOfType<MusicController>();
                if (BigJank.getOptionValue("Replace Intro Scene", "UI Changes"))
                {
                    MusicAPI.StopSong(ref c, "muIntroCutscene");
                }
                if (BigJank.getOptionValue("Logo", "UI Changes"))
                    UImods.ReplaceUIObject("LogoImage", "MoistureUpset.Resources.MoistureUpsetFinal.png");
                if (BigJank.getOptionValue("Roblox Cursor", "UI Changes"))
                {
                    UImods.ReplaceUIObject("MousePointer", "MoistureUpset.Resources.robloxhover.png");
                    UImods.ReplaceUIObject("MouseHover", "MoistureUpset.Resources.roblox.png");
                }
                try
                {
                    string song = self.GetPropertyValue<MusicTrackDef>("currentTrack").cachedName;

                    if (BigJank.getOptionValue("Main menu music", "Audio"))
                        if (song == "muMenu" || song == "muLogbook")
                        {
                            self.GetPropertyValue<MusicTrackDef>("currentTrack").Stop();
                        }

                    //muFULLSong07
                    //muFULLSong18
                    //muSong04
                    if (BigJank.getOptionValue("Merchant", "Enemy Skins"))
                        if (MusicAPI.ReplaceSong(ref self, "muSong04", "PlayShopMusic"))
                        {
                            AkSoundEngine.SetRTPCValue("BossDead", 0f);
                        }
                    if (BigJank.getOptionValue("Creative Void Zone", "Audio"))
                        if (MusicAPI.ReplaceSong(ref self, "muSong08", "Play_Dicks"))
                        {
                            AkSoundEngine.SetRTPCValue("BossDead", 0f);
                        }
                    //Debug.Log($"--------------{song}");
                }
                catch (Exception)
                {
                }
            };
            On.RoR2.Run.OnClientGameOver += (orig, self, report) =>
            {
                orig(self, report);
                try
                {
                    StopBossMusic(new UInt32[] { 311764514, 405315856, 829504566, 1557982612, 4106775434 });
                    StopBossMusic(new UInt32[] { 3605238269, 3605238270, 3605238271, 3605238264, 3179516522, 4044558886, 2244734173, 2339617413, 3772119855, 2493198437, 291592398, 2857659536, 3163719647, 1581288698, 974987421, 2337675311, 696983880, 1214003200, 541788247 });
                    var c = GameObject.FindObjectOfType<Transform>();
                    if (BigJank.getOptionValue("Imposter", "Enemy Skins"))
                    {
                        var cs = GameObject.FindObjectsOfType<RoR2.CharacterMaster>();
                        foreach (var item in cs)
                        {
                            if (item.name.StartsWith("ScavLunar"))
                            {
                                if (report.gameEnding.ToString() == "StandardLoss (RoR2.GameEndingDef)")
                                {
                                    AkSoundEngine.PostEvent("ScavDefeat", c.gameObject);
                                    return;
                                }
                            }
                        }
                    }
                    if (BigJank.getOptionValue("End of game music", "Audio"))
                        if (report.gameEnding.ToString() == "StandardLoss (RoR2.GameEndingDef)")
                        {
                            AkSoundEngine.PostEvent("Defeat", c.gameObject);
                        }
                    if (BigJank.getOptionValue("Imposter", "Enemy Skins"))
                        if (report.gameEnding.ToString() == "LimboEnding (RoR2.GameEndingDef)")
                        {
                            AkSoundEngine.PostEvent("ScavVictory", c.gameObject);
                        }
                    var controller = GameObject.FindObjectOfType<MusicController>();
                    controller.GetPropertyValue<MusicTrackDef>("currentTrack").Stop();
                }
                catch (Exception)
                {
                }
                //StandardLoss
            };
            On.EntityStates.SpawnTeleporterState.OnEnter += (orig, self) =>
            {
                orig(self);
                try
                {
                    var c = GameObject.FindObjectOfType<MusicController>();
                    MusicAPI.StopCustomSong(ref c, "StopShopMusic");
                }
                catch (Exception)
                {
                }
            };
            //On.RoR2.CharacterBody.HasBuff += (orig, self, index) =>
            //{
            //    if (BuffIndex.NullSafeZone == index)
            //    {
            //        //NullSafeZone
            //        AkSoundEngine.SetRTPCValue("Dicks", (orig(self, index) ? 0f : 1f));
            //    }
            //    return orig(self, index);
            //};
            if (BigJank.getOptionValue("Logo", "UI Changes"))
                On.RoR2.CreditsController.OnEnable += (orig, self) =>
            {
                orig(self);
                UImods.ReplaceUIObject("Image", "MoistureUpset.Resources.MoistureUpsetFinal.png");
            };
            On.RoR2.UI.MainMenu.MainMenuController.Update += (orig, self) =>
            {
                orig(self);
                if (BigJank.getOptionValue("Logo", "UI Changes"))
                    UImods.ReplaceUIObject("LogoImage", "MoistureUpset.Resources.MoistureUpsetFinal.png");
                if (BigJank.getOptionValue("Roblox Cursor", "UI Changes"))
                {
                    UImods.ReplaceUIObject("MousePointer", "MoistureUpset.Resources.robloxhover.png");
                    UImods.ReplaceUIObject("MouseHover", "MoistureUpset.Resources.roblox.png");
                }
            };
            On.RoR2.UI.MainMenu.MainMenuController.SetDesiredMenuScreen += (orig, self, menu) =>
            {
                orig(self, menu);
                try
                {
                    if (menu.name != "TitleMenu")
                    {
                        AkSoundEngine.SetRTPCValue("MainMenuMusic", 0.3f);
                    }
                    else
                    {
                        AkSoundEngine.SetRTPCValue("MainMenuMusic", 1);
                    }
                }
                catch (Exception)
                {
                }
            };
            //On.RoR2.TeleporterInteraction.AttemptToSpawnAllEligiblePortals += (orig, self) =>
            //{
            //    self.shouldAttemptToSpawnShopPortal = true;
            //    orig(self);
            //};
        }
        public static void HitMarker(float _Vol)
        {
            //Debug.Log($"Set hitmarker volume {_Vol}");
            AkSoundEngine.SetRTPCValue("RuneBadNoise", _Vol);
        }
        public static void Modded_MSX(float _Vol)
        {
            AkSoundEngine.SetRTPCValue("Modded_MSX", _Vol);
        }
        public static void Modded_SFX(float _Vol)
        {
            AkSoundEngine.SetRTPCValue("Modded_SFX", _Vol);
        }
        public static void OnHit()
        {
            On.RoR2.UI.CrosshairManager.RefreshHitmarker += (orig, self, crit) =>
            {
                if (!crit)
                {
                    AkSoundEngine.PostEvent("HitMarker", RoR2Application.instance.gameObject);
                }
                else
                {
                    crit = false;
                    AkSoundEngine.PostEvent("CritMarker", RoR2Application.instance.gameObject);
                }
                orig(self, crit);
            };
        }
        public static void StopBossMusic(UInt32[] ids)
        {
            foreach (var item in ids)
            {
                AkSoundEngine.ExecuteActionOnEvent(item, AkActionOnEventType.AkActionOnEventType_Stop);
            }
        }
            static int brother = 0;
        public static void BossMusicAndFanFare()
        {
            On.EntityStates.Missions.BrotherEncounter.PreEncounter.OnEnter += (orig, self) =>
            {
                orig(self);
                if ((BigJank.getOptionValue("Thanos", "Enemy Skins") != true))
                    return;
                StopBossMusic(new UInt32[] { 3605238269, 3605238270, 3605238271, 3605238264, 3179516522, 4044558886, 2244734173, 2339617413, 3772119855, 2493198437, 291592398, 2857659536, 3163719647, 1581288698, 974987421, 2337675311, 696983880, 541788247 });
                var c = GameObject.FindObjectOfType<MusicController>();
                var mainBody = GameObject.FindObjectOfType<Transform>();
                MusicAPI.StopSong(ref c, "muSong25");
                AkSoundEngine.SetRTPCValue("BossMusicActive", 1);
                AkSoundEngine.PostEvent("PlayThanos1", mainBody.gameObject);
            };
            On.RoR2.CharacterBody.GetSubtitle += (orig, self) =>
            {
                try
                {
                    if (self.baseNameToken == "COMMANDO_BODY_NAME" || self.baseNameToken == "MERC_BODY_NAME" || self.baseNameToken == "ENGI_BODY_NAME" || self.baseNameToken == "HUNTRESS_BODY_NAME" || self.baseNameToken == "MAGE_BODY_NAME" || self.baseNameToken == "TOOLBOT_BODY_NAME" || self.baseNameToken == "TREEBOT_BODY_NAME" || self.baseNameToken == "LOADER_BODY_NAME" || self.baseNameToken == "CROCO_BODY_NAME" || self.baseNameToken == "CAPTAIN_BODY_NAME")
                    {
                        return orig(self);
                    }
                    if (self.master && self.master.isBoss)
                    {
                        var c = GameObject.FindObjectOfType<MusicController>();
                        bool resetThanos = true;
                        var mainBody = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody();
                        bool stop = false;
                        StopBossMusic(new UInt32[] { 3605238270, 3605238271, 3605238264, 3179516522, 4044558886, 2244734173, 2339617413, 3772119855, 2493198437, 291592398, 2857659536, 3163719647, 1581288698, 974987421, 2337675311, 696983880, 541788247 });
                        if (self.baseNameToken == "IMPBOSS_BODY_NAME" && (BigJank.getOptionValue("Sans", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlaySans", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "ARTIFACTSHELL_BODY_NAME" && (BigJank.getOptionValue("Cereal", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("ArtifactIntro", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "ROBOBALLBOSS_BODY_NAME" && (BigJank.getOptionValue("Obama Prism", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlayObama", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "SUPERROBOBALLBOSS_BODY_NAME" && (BigJank.getOptionValue("Obama Prism", "Enemy Skins")))
                        {

                        }
                        else if (self.baseNameToken == "TITANGOLD_BODY_NAME" && (BigJank.getOptionValue("Alex Jones", "Enemy Skins")))
                        {

                        }
                        else if (self.baseNameToken.StartsWith("SCAVLUNAR") && (BigJank.getOptionValue("Imposter", "Enemy Skins")))
                        {

                        }
                        else if (self.baseNameToken.StartsWith("DIRESEEKER_BOSS_BODY_NAME") && (BigJank.getOptionValue("DireSeeker", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("DireSeekerMusic", mainBody.gameObject);
                            stop = true;
                        }
                        else if ((self.baseNameToken == "BROTHER_BODY_NAME" || self.baseNameToken == "LUNARGOLEM_BODY_NAME" || self.baseNameToken == "LUNARWISP_BODY_NAME"))
                        {
                            if ((BigJank.getOptionValue("Thanos", "Enemy Skins")))
                            {
                                resetThanos = false;
                                brother++;
                                MusicAPI.StopSong(ref c, "muSong25");
                                switch (brother)
                                {
                                    case 1:
                                        break;
                                    case 2:
                                        AkSoundEngine.ExecuteActionOnEvent(3605238269, AkActionOnEventType.AkActionOnEventType_Stop);
                                        AkSoundEngine.PostEvent("PlayThanos2", mainBody.gameObject);
                                        break;
                                    case 3:
                                        AkSoundEngine.PostEvent("PlayThanos3", mainBody.gameObject);
                                        break;
                                    case 4:
                                        AkSoundEngine.PostEvent("PlayThanos4", mainBody.gameObject);
                                        break;
                                    default:
                                        break;
                                }
                                stop = true;
                            }
                        }
                        else if (self.baseNameToken == "ELECTRICWORM_BODY_NAME" && (BigJank.getOptionValue("Squirmles", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlaySquirmles", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "GRAVEKEEPER_BODY_NAME" && (BigJank.getOptionValue("Twitch", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlayTwitch", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "BEETLEQUEEN_BODY_NAME" && (BigJank.getOptionValue("Nyan Cat", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlayNyan", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "VAGRANT_BODY_NAME" && (BigJank.getOptionValue("WanderingAtEveryone", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlayDiscord", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "CLAYBOSS_BODY_NAME" && (BigJank.getOptionValue("Giga Puddi", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlayPudi", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "MAGMAWORM_BODY_NAME" && (BigJank.getOptionValue("Pool Noodle", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("PlayNoodle", mainBody.gameObject);
                            stop = true;
                        }
                        else if (self.baseNameToken == "TITAN_BODY_NAME" && (BigJank.getOptionValue("Roblox Titan", "Enemy Skins")))
                        {
                            AkSoundEngine.PostEvent("RobloxMusic", mainBody.gameObject);
                            stop = true;
                        }
                        else if (BigJank.getOptionValue("Generic boss music", "Enemy Skins"))
                        {
                            AkSoundEngine.PostEvent("PlayBossMusic", mainBody.gameObject);
                            stop = true;
                        }
                        if (resetThanos)
                        {
                            brother = 0;
                            AkSoundEngine.ExecuteActionOnEvent(3605238269, AkActionOnEventType.AkActionOnEventType_Stop);
                        }
                        try
                        {
                            if (stop)
                            {
                                //muEscape
                                //muSong25
                                //muSong05
                                MusicAPI.StopSong(ref c, "muSong05");
                                MusicAPI.StopSong(ref c, "muSong23");
                                MusicAPI.StopSong(ref c, "muSong13");
                                //MusicAPI.GetCurrentSong(ref c);
                                //AkSoundEngine.exec
                                AkSoundEngine.SetRTPCValue("BossMusicActive", 1);
                                var con = GameObject.FindObjectOfType<MusicController>();
                                MusicAPI.StopCustomSong(ref con, "StopLevelMusic");
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {

                }
                return orig(self);
            };
            On.RoR2.MusicController.UpdateTeleporterParameters += (orig, self, t, cT, tB) =>
            {
                try
                {
                    bool flag = true;
                    flag = t.holdoutZoneController.IsBodyInChargingRadius(tB);
                    AkSoundEngine.SetRTPCValue("isInPortalRange", (flag ? 1f : 0f));
                }
                catch (Exception)
                {

                }
                orig(self, t, cT, tB);
            };
            On.RoR2.UI.ObjectivePanelController.FindTeleporterObjectiveTracker.ctor += (orig, self) =>
            {
                orig(self);
                try
                {
                    AkSoundEngine.SetRTPCValue("BossDead", 0f);
                }
                catch (Exception)
                {

                }
            };
            On.RoR2.UI.ObjectivePanelController.ActivateGoldshoreBeaconTracker.ctor += (orig, self) =>
            {
                orig(self);
                try
                {
                    AkSoundEngine.SetRTPCValue("BossDead", 0f);
                }
                catch (Exception)
                {

                }
            };
            On.RoR2.UI.ObjectivePanelController.DestroyTimeCrystals.ctor += (orig, self) =>
            {
                orig(self);
                try
                {
                    AkSoundEngine.SetRTPCValue("BossDead", 0f);
                }
                catch (Exception)
                {

                }
            };
            On.RoR2.UI.ObjectivePanelController.AddObjectiveTracker += (orig, self, tracker) =>
            {
                orig(self, tracker);
                try
                {
                    AkSoundEngine.SetRTPCValue("Dicks", (tracker.ToString() == "RoR2.HoldoutZoneController+ChargeHoldoutZoneObjectiveTracker" ? 1f : 0f));
                }
                catch (Exception)
                {
                }
                if (tracker.ToString() == "RoR2.UI.ObjectivePanelController+FindTeleporterObjectiveTracker")
                    try
                    {
                        if (BigJank.getOptionValue("Interactables", "Interactables"))
                        {
                            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
                            foreach (var fab in objects)
                            {
                                if (fab.ToString() == "GoldChest (UnityEngine.GameObject)")
                                {
                                    if (!InteractReplacements.Interactables.particles)
                                    {
                                        InteractReplacements.Interactables.particles = Resources.Load<GameObject>("@MoistureUpset_moisture_chests:assets/arbitraryfolder/particles.prefab");
                                    }
                                    EnemyReplacements.ReplaceModel(fab, "@MoistureUpset_moisture_chests:assets/arbitraryfolder/goldchest.mesh", "@MoistureUpset_moisture_chests:assets/arbitraryfolder/goldchest.png");
                                    fab.GetComponentInChildren<SkinnedMeshRenderer>().material.shader = Resources.Load<GameObject>("prefabs/networkedobjects/chest/Chest2").GetComponentInChildren<SkinnedMeshRenderer>().material.shader;
                                    fab.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture.filterMode = FilterMode.Point;
                                    fab.GetComponentInChildren<ParticleSystem>().maxParticles = 0;
                                    fab.GetComponentInChildren<SfxLocator>().openSound = "GoldChest";

                                    var obj = GameObject.Instantiate(InteractReplacements.Interactables.particles, fab.transform);
                                    obj.transform.SetParent(fab.transform);
                                    obj.transform.localPosition = Vector3.zero;
                                }
                                else if (fab.ToString().StartsWith("NewtStatue"))
                                {
                                    if (BigJank.getOptionValue("Currency Changes", "UI Changes") && fab.GetComponentsInChildren<Fixers.robloxfixer>().Length == 0)
                                    {
                                        int num = UnityEngine.Random.Range(0, 3);
                                        GameObject g;
                                        if (num == 0)
                                        {
                                            g = GameObject.Instantiate(Resources.Load<GameObject>("@MoistureUpset_moisture_newtaltar:assets/testing/atoasteroven.prefab"));
                                        }
                                        else if (num == 1)
                                        {
                                            g = GameObject.Instantiate(Resources.Load<GameObject>("@MoistureUpset_moisture_newtaltar:assets/testing/kevinaltar.prefab"));
                                        }
                                        else
                                        {
                                            g = GameObject.Instantiate(Resources.Load<GameObject>("@MoistureUpset_moisture_newtaltar:assets/testing/RuneMasterGaming580808080808080ADHD.prefab"));
                                        }
                                        g.transform.parent = fab.transform;
                                        g.transform.localPosition = new Vector3(0, -1.15f, 0);
                                        g.transform.localScale = new Vector3(.5f, .5f, .5f);
                                        g.transform.localEulerAngles = Vector3.zero;
                                        Texture t = g.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture;
                                        g.GetComponentInChildren<SkinnedMeshRenderer>().material = fab.GetComponentInChildren<MeshRenderer>().material;
                                        g.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = t;
                                        foreach (var item in fab.GetComponentsInChildren<MeshRenderer>())
                                        {
                                            item.enabled = false;
                                        }
                                        fab.transform.Find("mdlNewtStatue").Find("HologramPivot").localPosition = new Vector3(0, -1.4f, 0);
                                        var fixer = fab.AddComponent<Fixers.robloxfixer>();
                                        fixer.g = fab.transform.Find("mdlNewtStatue").Find("HologramPivot").gameObject;
                                        fixer.a = g.GetComponentInChildren<Animator>();
                                        fab.GetComponent<Highlight>().targetRenderer = g.GetComponentInChildren<SkinnedMeshRenderer>();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
            };
            On.RoR2.UI.ObjectivePanelController.RemoveObjectiveTracker += (orig, self, tracker) =>
            {
                orig(self, tracker);
                try
                {
                    AkSoundEngine.SetRTPCValue("Dicks", (tracker.ToString() == "RoR2.HoldoutZoneController+ChargeHoldoutZoneObjectiveTracker" || tracker.ToString() == "RoR2.UI.ObjectivePanelController+ClearArena" ? 0f : 1f));
                }
                catch (Exception)
                {
                }
            };
            On.RoR2.UI.ObjectivePanelController.FinishTeleporterObjectiveTracker.ctor += (orig, self) =>
            {
                orig(self);
                try
                {
                    var c = GameObject.FindObjectOfType<MusicController>();
                    var mainBody = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody();
                    AkSoundEngine.ExecuteActionOnEvent(1462303513, AkActionOnEventType.AkActionOnEventType_Stop);
                    AkSoundEngine.SetRTPCValue("BossMusicActive", 0);
                    AkSoundEngine.PostEvent("StopFanFare", c.gameObject);
                    AkSoundEngine.SetRTPCValue("BossDead", 1f);
                    if (BigJank.getOptionValue("Fanfare", "Audio"))
                        AkSoundEngine.PostEvent("PlayFanFare", c.gameObject);
                }
                catch (Exception)
                {
                }
            };
        }
        public static void Somebody()
        {
            if (BigJank.getOptionValue("Shreks outhouse", "Misc"))
            {
                On.EntityStates.SurvivorPod.PreRelease.OnEnter += (orig, self) =>
                {
                    orig(self);
                    Util.PlaySound("somebody", self.outer.gameObject);
                };
                On.EntityStates.SurvivorPod.Landed.OnEnter += (orig, self) =>
                {
                    orig(self);
                    AkSoundEngine.PostEvent("SomebodyLoop", self.outer.gameObject);
                };
                On.EntityStates.SurvivorPod.Landed.OnExit += (orig, self) =>
                {
                    orig(self);
                    AkSoundEngine.PostEvent("SomebodyStop", self.outer.gameObject);
                };
                On.EntityStates.SurvivorPod.Release.OnEnter += (orig, self) =>
                {
                    orig(self);
                    AkSoundEngine.PostEvent("SomebodyStop", self.outer.gameObject);
                };
                EnemyReplacements.LoadResource("shreklet");
                foreach (var item in Resources.Load<GameObject>("prefabs/networkedobjects/SurvivorPod").GetComponentsInChildren<ChildLocator>())
                {
                    item.FindChild("ReleaseExhaustFX").gameObject.GetComponentsInChildren<MeshFilter>()[1].sharedMesh = Resources.Load<Mesh>("@MoistureUpset_shreklet:assets/shrekletdoorphysics.mesh");
                    item.FindChild("ReleaseExhaustFX").gameObject.GetComponentsInChildren<MeshRenderer>()[1].sharedMaterial.mainTexture = Resources.Load<Texture>("@MoistureUpset_shreklet:assets/shreklet.png");
                }
                EnemyReplacements.ReplaceMeshFilter("prefabs/networkedobjects/SurvivorPod", "@MoistureUpset_shreklet:assets/shreklet.mesh", "@MoistureUpset_shreklet:assets/shreklet.png", 1);
                EnemyReplacements.ReplaceMeshFilter("prefabs/networkedobjects/SurvivorPod", "@MoistureUpset_shreklet:assets/shrekletdoor.mesh", "@MoistureUpset_shreklet:assets/shreklet.png", 0);
            }
        }
        public static void BossMusic()
        {
            On.RoR2.WwiseUtils.CommonWwiseIds.Init += (orig) =>
            {
                orig();
                //RoR2.WwiseUtils.CommonWwiseIds.bossfight = AkSoundEngine.GetIDFromString("ooflongestloop");
                if (BigJank.getOptionValue("Generic boss music", "Audio"))
                    try
                    {

                        RoR2.WwiseUtils.CommonWwiseIds.alive = AkSoundEngine.GetIDFromString("ooflongestloop");
                        RoR2.WwiseUtils.CommonWwiseIds.dead = AkSoundEngine.GetIDFromString("ooflongestloop");
                    }
                    catch (Exception)
                    {
                    }
            };
        }
    }
}
