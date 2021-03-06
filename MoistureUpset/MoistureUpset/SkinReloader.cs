﻿using RoR2;
using UnityEngine;

namespace MoistureUpset
{
    class SkinReloader : MonoBehaviour
    {
        // For some reason clients will sometimes see the incorrect skin with custom skins, so I just add this to all survivors and engi turrets, in the hopes that it ensures the correct skin is displayed.
        private void Start()
        {
            try
            {
                var skinController = GetComponentInChildren<ModelSkinController>();

                int skinIndex;

                if (GetComponentInChildren<CharacterBody>().master.minionOwnership.ownerMaster == null)
                {
                    skinIndex = (int)GetComponentInChildren<CharacterBody>().skinIndex;
                    skinController.ApplySkin(skinIndex);
                }
                else if (GetComponentInChildren<CharacterBody>().master.minionOwnership.ownerMaster.GetBody().isSkin("THE_TF2_ENGINEER_SKIN"))
                {
                    if (SkinHelper.engiTurretSkinIndex < 0)
                    {
                        var skins = GetComponentInChildren<CharacterBody>().modelLocator.modelTransform.GetComponentInChildren<ModelSkinController>().skins;

                        for (int i = 0; i < skins.Length; i++)
                        {
                            if (skins[i].name == "Level 1 Sentry" || skins[i].name == "Level 2 Sentry")
                            {
                                SkinHelper.engiTurretSkinIndex = i;
                            }
                        }
                    }

                    //skinIndex = (int)GetComponentInChildren<CharacterBody>().master.minionOwnership.ownerMaster.GetBody().skinIndex;
                    skinIndex = SkinHelper.engiTurretSkinIndex;
                    GetComponentInChildren<CharacterBody>().skinIndex = (uint)skinIndex; //I feel like I should be able to do this at a better time, like when initializing this stuff, but it don't work and/or I'm not smart enough to figure it out
                    skinController.ApplySkin(skinIndex);
                }

                if (GetComponentInChildren<CharacterBody>().isSkin("THE_TF2_ENGINEER_SKIN"))
                {
                    GetComponentInChildren<CharacterModel>().itemDisplayRuleSet = ItemDisplayPositionFixer.TF2_Engi_IDRS; // We apply our own display rule set so that items look correct on our skin.
                }
            }
            catch (System.Exception)
            {
            }
        }

        void FixedUpdate()
        {
        }
    }
}
