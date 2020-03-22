// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.33
// 

using Colyseus.Schema;

namespace Game.Models {
	public class WeaponLoadout : Schema {
		[Type(0, "ref", typeof(Weapon))]
		public Weapon mainHand = new Weapon();

		[Type(1, "ref", typeof(Weapon))]
		public Weapon offHand = new Weapon();
	}
}
