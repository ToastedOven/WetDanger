﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;

namespace MoistureUpset
{
    class SkinReloader : MonoBehaviour
    {
        private void Start()
        {
            var skinController = GetComponentInChildren<ModelSkinController>();

            int skinIndex = 0;

            if (GetComponentInChildren<CharacterBody>().master.minionOwnership.ownerMaster == null)
            {
                skinIndex = (int)GetComponentInChildren<CharacterBody>().skinIndex;
            }
            else
            {
                skinIndex = (int)GetComponentInChildren<CharacterBody>().master.minionOwnership.ownerMaster.GetBody().skinIndex;
            }

            skinController.ApplySkin(skinIndex);

            foreach (var item in skinController.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials)
            {
                Debug.Log("replacing textures");
                item.mainTexture = Resources.Load<Texture>("@MoistureUpset_engi_turret2:assets/unified_turret_tex.png");
                item.SetTexture("_EmTex", Resources.Load<Texture>("@MoistureUpset_engi_turret2:assets/unified_turret_tex.png"));
            }
        }
    }
}
