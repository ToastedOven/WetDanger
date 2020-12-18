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

namespace MoistureUpset
{
    public static class BigToasterClass
    {
        public static void RunAll()
        {
            Somebody();
            BossMusic();
            BossMusicAndFanFare();
            OnHit();
            PreGame();
            INeedToSortThese();
            PlayerDeath();
            DifficultyIcons();
        }
        public static void INeedToSortThese()
        {
            //On.RoR2.BossGroup.UpdateBossMemories += (orig, self) =>
            //{
            //    orig(self);
            //    Debug.Log($"bossgroup--------{self.bestObservedName}");
            //};
            //On.RoR2.SceneDirector.Start += (orig, self) =>
            //{
            //    orig(self);
            //    Debug.Log($"asddddddddddddddddddddddddddd--------{self.teleporterSpawnCard.name}");
            //    foreach (var item in self.teleporterSpawnCard.prefab.GetComponents<Component>())
            //    {
            //        Debug.Log($"asddddddddddddddddddddddddddd--------{item}");
            //    }
            //    self.teleporterSpawnCard.prefab.GetComponent<BossGroup>().InvokeMethod("Start");
            //    Debug.Log($"bossgroup--------{self.teleporterSpawnCard.prefab.GetComponent<BossGroup>().bestObservedName}");
            //};
        }

        public static void DifficultyIcons()
        {
            if (float.Parse(ModSettingsManager.getOptionValue("Difficulty Icons")) == 1)
            {
                UImods.ReplaceUIBetter("Textures/DifficultyIcons/texDifficultyEasyIcon", "MoistureUpset.Resources.easy.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyEasyIconDisabled", "MoistureUpset.Resources.easyDisabled.png");

                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyNormalIcon", "MoistureUpset.Resources.medium.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyNormalIconDisabled", "MoistureUpset.Resources.mediumDisabled.png");

                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyHardIcon", "MoistureUpset.Resources.hard.png");
                UImods.ReplaceUIBetter("textures/difficultyicons/texDifficultyHardIconDisabled", "MoistureUpset.Resources.hardDisabled.png");
            }

            if (float.Parse(ModSettingsManager.getOptionValue("Pizza Roll")) == 1)
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
            //var font = Resources.Load<Font>("@MoistureUpset_robloxfont:assets/roblox_font.ttf");
            //var resources2 = Resources.LoadAll<Font>("");
            //for (int i = 0; i < resources2.Length; i++)
            //{
            //    if (resources2[i].name.Contains("Font Texture") || resources2[i].name.Contains("Atlas"))
            //    {
            //        Debug.Log($"-----{resources2[i]}");
            //        //resources2[i].
            //    }
            //}
            //byte[] bytes = ByteReader.readbytes("MoistureUpset.Resources.font.png");
            //var resources = Resources.LoadAll<UnityEngine.Object>("");
            //for (int i = 0; i < resources.Length; i++)
            //{
            //        Debug.Log($"-----{resources[i]}");
            //    if (resources[i].name.Contains("Font Texture") || resources[i].name.Contains("Atlas"))
            //    {
            //        //resources[i].LoadImage(bytes);
            //    }
            //}
        }

        public static void PlayerDeath()
        {
            if (float.Parse(ModSettingsManager.getOptionValue("Player death chat")) == 1)
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
                    if (float.Parse(ModSettingsManager.getOptionValue("NSFW")) == 1)
                    {
                        quotes.Add("I fucking hate this game");
                    }
                    if (report.attackerMaster.name.ToUpper().Contains("MAGMAWORM"))
                    {
                        quotes.Add("The magma worm is such bullshit");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("ELECTRICWORM"))
                    {
                        quotes.Add("Why does it get lightning? It's already strong enough");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("BROTHERHURT"))
                    {
                        quotes.Add("This final phase sucks so much");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("WISPMASTER"))
                    {
                        quotes.Add("Unfucking dodgeable");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("VAGRANT"))
                    {
                        quotes.Add("How are you supposed to dodge that????");
                    }
                    else if (report.attackerMaster.name.ToUpper().Contains("LEMURIANBRUISERMASTER"))
                    {
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
                if (float.Parse(ModSettingsManager.getOptionValue("Taco Bell")) == 1)
                    EnemyReplacements.ReplaceMeshRenderer(EntityStates.Bell.BellWeapon.ChargeTrioBomb.preppedBombPrefab, "@MoistureUpset_tacobell:assets/toco.mesh", "@MoistureUpset_tacobell:assets/toco.png");
                if (float.Parse(ModSettingsManager.getOptionValue("Toad")) == 1)
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

                if (float.Parse(ModSettingsManager.getOptionValue("Roblox Titan")) == 1)
                    EnemyReplacements.ReplaceTexture("prefabs/characterbodies/TitanBody", "@MoistureUpset_roblox:assets/robloxtitan.png");
                if (float.Parse(ModSettingsManager.getOptionValue("Sans")) == 1)
                    EntityStates.ImpBossMonster.GroundPound.slamEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().mesh = null;
                orig(oldS, newS);
                if (float.Parse(ModSettingsManager.getOptionValue("Main menu music")) == 1)
                    try
                    {
                        switch (newS.name)
                        {
                            case "logbook":
                                AkSoundEngine.SetRTPCValue("MainMenuMusic", 0f);
                                break;
                            case "title":
                                AkSoundEngine.SetRTPCValue("MainMenuMusic", 1);
                                AkSoundEngine.SetRTPCValue("LobbyActivated", 1);
                                break;
                            case "lobby":
                                AkSoundEngine.SetRTPCValue("LobbyActivated", 1);
                                AkSoundEngine.SetRTPCValue("MainMenuMusic", 0f);
                                break;
                            case "splash":
                                AkSoundEngine.PostEvent("PlayMainMenu", GameObject.FindObjectOfType<GameObject>());
                                AkSoundEngine.SetRTPCValue("MainMenuMusic", 0);
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
                try
                {
                    string song = self.GetPropertyValue<MusicTrackDef>("currentTrack").cachedName;

                    if (float.Parse(ModSettingsManager.getOptionValue("Main menu music")) == 1)
                        if (song == "muMenu" || song == "muLogbook")
                        {
                            self.GetPropertyValue<MusicTrackDef>("currentTrack").Stop();
                        }

                    //muFULLSong07
                    //muFULLSong18
                    //muSong04
                    if (float.Parse(ModSettingsManager.getOptionValue("Merchant")) == 1)
                        if (MusicAPI.ReplaceSong(ref self, "muSong04", "PlayShopMusic"))
                        {
                            AkSoundEngine.SetRTPCValue("BossDead", 0f);
                        }
                    if (float.Parse(ModSettingsManager.getOptionValue("Creative Void Zone")) == 1)
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
            On.RoR2.CharacterBody.HasBuff += (orig, self, index) =>
            {
                if (BuffIndex.NullSafeZone == index)
                {
                    //NullSafeZone
                    AkSoundEngine.SetRTPCValue("Dicks", (orig(self, index) ? 0f : 1f));
                }
                return orig(self, index);
            };
            if (float.Parse(ModSettingsManager.getOptionValue("Logo")) == 1)
                On.RoR2.CreditsController.OnEnable += (orig, self) =>
            {
                orig(self);
                UImods.ReplaceUIObject("Image", "MoistureUpset.Resources.MoistureUpsetFinal.png");
            };
            On.RoR2.UI.MainMenu.MainMenuController.Update += (orig, self) =>
            {
                orig(self);
                if (float.Parse(ModSettingsManager.getOptionValue("Logo")) == 1)
                    UImods.ReplaceUIObject("LogoImage", "MoistureUpset.Resources.MoistureUpsetFinal.png");
                if (float.Parse(ModSettingsManager.getOptionValue("Roblox Cursor")) == 1)
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
            Debug.Log($"Set hitmarker volume {_Vol}");
            AkSoundEngine.SetRTPCValue("RuneBadNoise", _Vol);
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
        public static void BossMusicAndFanFare()
        {
            int brother = 0;
            On.EntityStates.Missions.BrotherEncounter.PreEncounter.OnEnter += (orig, self) =>
            {
                StopBossMusic(new UInt32[] { 2369706651, 2369706648, 2369706649, 2369706654, 3179516522, 4044558886, 2244734173, 2339617413, 3772119855, 2493198437, 291592398, 2857659536, 3163719647, 1581288698, 974987421 });

                var mainBody = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody();
                AkSoundEngine.SetRTPCValue("BossMusicActive", 1);
                orig(self);
                AkSoundEngine.PostEvent("Thanos1", mainBody.gameObject);
            };
            On.RoR2.CharacterBody.GetSubtitle += (orig, self) =>
            {
                if (self.master && self.master.isBoss)
                {
                    bool resetThanos = true;
                    var mainBody = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody();
                    StopBossMusic(new UInt32[] { 2369706648, 2369706649, 2369706654, 3179516522, 4044558886, 2244734173, 2339617413, 3772119855, 2493198437, 291592398, 2857659536, 3163719647, 1581288698, 974987421 });
                    if (self.baseNameToken == "IMPBOSS_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Sans")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlaySans", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "ARTIFACTSHELL_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Cereal")) == 1))
                    {
                        AkSoundEngine.PostEvent("ArtifactIntro", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "ROBOBALLBOSS_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Obama Prism")) == 1))
                    {
                        AkSoundEngine.PostEvent("ArtifactIntro", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "SUPERROBOBALLBOSS_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Obama Prism")) == 1))
                    {

                    }
                    else if (self.baseNameToken == "TITANGOLD_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Alex Jones")) == 1))
                    {

                    }
                    else if ((self.baseNameToken == "BROTHER_BODY_NAME" || self.baseNameToken == "LUNARGOLEM_BODY_NAME" || self.baseNameToken == "LUNARWISP_BODY_NAME") && (float.Parse(ModSettingsManager.getOptionValue("Thanos")) == 1))
                    {
                        resetThanos = false;
                        brother++;
                        switch (brother)
                        {
                            case 1:
                                break;
                            case 2:
                                AkSoundEngine.ExecuteActionOnEvent(2369706651, AkActionOnEventType.AkActionOnEventType_Stop);
                                AkSoundEngine.PostEvent("Thanos2", mainBody.gameObject);
                                break;
                            case 3:
                                AkSoundEngine.PostEvent("Thanos3", mainBody.gameObject);
                                break;
                            case 4:
                                AkSoundEngine.PostEvent("Thanos4", mainBody.gameObject);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (self.baseNameToken == "ELECTRICWORM_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Squirmles")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlaySquirmles", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "GRAVEKEEPER_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Twitch")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlayTwitch", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "BEETLEQUEEN_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Nyan Cat")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlayNyan", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "VAGRANT_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Wandering @Everyone")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlayDiscord", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "CLAYBOSS_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Giga Puddi")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlayPudi", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "MAGMAWORM_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Pool Noodle")) == 1))
                    {
                        AkSoundEngine.PostEvent("PlayNoodle", mainBody.gameObject);
                    }
                    else if (self.baseNameToken == "TITAN_BODY_NAME" && (float.Parse(ModSettingsManager.getOptionValue("Roblox Titan")) == 1))
                    {
                        AkSoundEngine.PostEvent("RobloxMusic", mainBody.gameObject);
                    }
                    else if (float.Parse(ModSettingsManager.getOptionValue("Generic boss music")) == 1)
                    {
                        AkSoundEngine.PostEvent("PlayBossMusic", mainBody.gameObject);
                    }
                    if (resetThanos)
                    {
                        brother = 0;
                        AkSoundEngine.ExecuteActionOnEvent(2369706651, AkActionOnEventType.AkActionOnEventType_Stop);
                    }
                    try
                    {
                        //muEscape
                        //muSong25
                        var c = GameObject.FindObjectOfType<MusicController>();
                        MusicAPI.StopSong(ref c, "muSong25");
                        MusicAPI.StopSong(ref c, "muSong23");
                        MusicAPI.StopSong(ref c, "muSong13");
                        MusicAPI.GetCurrentSong(ref c);
                        //AkSoundEngine.exec
                        AkSoundEngine.SetRTPCValue("BossMusicActive", 1);
                        var con = GameObject.FindObjectOfType<MusicController>();
                        MusicAPI.StopCustomSong(ref con, "StopLevelMusic");
                    }
                    catch (Exception)
                    {
                    }
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
                    if (float.Parse(ModSettingsManager.getOptionValue("Fanfare")) == 1)
                        AkSoundEngine.PostEvent("PlayFanFare", c.gameObject);
                }
                catch (Exception)
                {
                }
            };
        }
        public static void Somebody()
        {
            if (float.Parse(ModSettingsManager.getOptionValue("Shreks outhouse")) == 1)
            {
                if (float.Parse(ModSettingsManager.getOptionValue("Shreks outhouse")) == 1)
                    On.EntityStates.SurvivorPod.PreRelease.OnEnter += (orig, self) =>
                {
                    orig(self);
                    Util.PlaySound("somebody", self.outer.gameObject);
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
