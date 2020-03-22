import { Schema, type } from "@colyseus/schema";
import { Weapon } from "./weapon"

export class WeaponLoadout extends Schema {
    @type(Weapon) public mainHand: Weapon
    @type(Weapon) public offHand: Weapon
}