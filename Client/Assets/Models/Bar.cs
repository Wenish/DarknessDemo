// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.33
// 

using Colyseus.Schema;

namespace Game.Models {
	public class Bar : Schema {
		[Type(0, "number")]
		public float current = 0;

		[Type(1, "number")]
		public float max = 0;

		[Type(2, "number")]
		public float regenerationSpeed = 0;
	}
}
