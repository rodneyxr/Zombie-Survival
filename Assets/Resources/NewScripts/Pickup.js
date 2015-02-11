/*
This script must be attached on GO, which will represent any of these pick up types. 
Don't forget to set layer on this GO (in my case it must be "Ammo"). Tag for every GO also must be "Ammo".
This script works together with "WeaponManager".

Few notes:
Health - to increase health in "PlayerDamageNew". Nothing important here.
Rockets - for RPG. Uses "WeaponScriptNEW" and "firstMode" must be launcher.
Magazines - for every weapon in WeaponScriptNEW, if "firstMode" isn't laucher.
SniperMagazines - for "SniperScript".
Shells - only for shotgun. Uses "ShotGunScriptNEW" 
Launcher - for "WeaponScriptNEW", and secondMode must be launcher.
*/


enum PickupType { Health, Rockets, Magazines, SniperMagazines, Shells, Projectiles, StickGrenades}
var pickupType = PickupType.Health;
var amount : int = 3;
var AmmoInfo : String = "";


