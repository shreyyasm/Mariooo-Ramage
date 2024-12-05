﻿// Copyright 2021, Infima Games. All Rights Reserved.

using System;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        #region FIELDS
        
        /// <summary>
        /// Array of all weapons. These are gotten in the order that they are parented to this object.
        /// </summary>
        private WeaponBehaviour[] weapons;
        
        /// <summary>
        /// Currently equipped WeaponBehaviour.
        /// </summary>
        private WeaponBehaviour equipped;
        /// <summary>
        /// Currently equipped index.
        /// </summary>
        private int equippedIndex = -1;

        #endregion

        #region METHODS
        public WeaponSelectionManager weaponSelectionManager;
        public bool Tutorial;
        public override void Init(int equippedAtStart = 0)
        {
            
            //Cache all weapons. Beware that weapons need to be parented to the object this component is on!
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            //weaponSelectionManager.CheckWeaponUnlocks();
            //Disable all weapons. This makes it easier for us to only activate the one we need.
            foreach (WeaponBehaviour weapon in weapons)
                weapon.gameObject.SetActive(false);
            
            if(Tutorial)
            {
                Equip(equippedAtStart);
            }
            else
            {
                if (weaponSelectionManager.currentSelectedWeaponIndex == 0)
                {
                    //LeanTween.delayedCall(0.5f, () => {  character.RefreshWeaponSetup(); character.RefreshWeaponSetup(); });
                    Equip(equippedAtStart);
                    weapons[0].gameObject.SetActive(true);
                    character.RefreshWeaponSetup();
                }
                else
                {
                    LeanTween.delayedCall(0.5f, () => { Equip(weaponSelectionManager.currentSelectedWeaponIndex); weapons[weaponSelectionManager.currentSelectedWeaponIndex].gameObject.SetActive(true); character.RefreshWeaponSetup(); });
                }
            }
            
            //Equip.
           
           
        }
        public void ChecksForNewWeapon(int index)
        {
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            //weaponSelectionManager.CheckWeaponUnlocks();
            //Disable all weapons. This makes it easier for us to only activate the one we need.
            foreach (WeaponBehaviour weapon in weapons)
                weapon.gameObject.SetActive(false);
            LeanTween.delayedCall(0.2f, () => {
                Equip(index);
                weapons[index].gameObject.SetActive(true);
                character.RefreshWeaponSetup();
            });
           
        }
        public Character character;
        protected virtual void Tick() { }
        private void Update()
        {
            
        }
        public override WeaponBehaviour Equip(int index)
        {
            //If we have no weapons, we can't really equip anything.
            if (weapons == null)
                return equipped;
            
            //The index needs to be within the array's bounds.
            if (index > weapons.Length - 1)
                return equipped;

            //No point in allowing equipping the already-equipped weapon.
            if (equippedIndex == index)
                return equipped;
            
            //Disable the currently equipped weapon, if we have one.
            if (equipped != null)
                equipped.gameObject.SetActive(false);

            //Update index.
            equippedIndex = index;
            //Update equipped.
            equipped = weapons[equippedIndex];
            //Activate the newly-equipped weapon.
            equipped.gameObject.SetActive(true);

            //Return.
            return equipped;
        }
        
        #endregion

        #region Getters

        public override int GetLastIndex()
        {
            //Get last index with wrap around.
            int newIndex = equippedIndex - 1;
            if (newIndex < 0)
                newIndex = weapons.Length - 1;

            //Return.
            return newIndex;
        }

        public override int GetNextIndex()
        {
            //Get next index with wrap around.
            int newIndex = equippedIndex + 1;
            if (newIndex > weapons.Length - 1)
                newIndex = 0;

            //Return.
            return newIndex;
        }

        public override WeaponBehaviour GetEquipped() => equipped;
        public override int GetEquippedIndex() => equippedIndex;

        #endregion
    }
}