// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.33
// 

using Colyseus.Schema;

namespace Game.Models {
	public class Unit : Schema {
		[Type(0, "string")]
		public string id = "";

		[Type(1, "string")]
		public string name = "";

		[Type(2, "ref", typeof(Position))]
		public Position position = new Position();

		[Type(3, "number")]
		public float moveSpeed = 0;

		[Type(4, "number")]
		public float rotation = 0;

		[Type(5, "number")]
		public float locomotionAnimationSpeedPercent = 0;

		[Type(6, "boolean")]
		public bool isAlive = false;

		[Type(7, "ref", typeof(Bar))]
		public Bar health = new Bar();

		[Type(8, "ref", typeof(Bar))]
		public Bar energy = new Bar();

		[Type(9, "ref", typeof(WeaponLoadout))]
		public WeaponLoadout weaponLoadout = new WeaponLoadout();
	}
}
