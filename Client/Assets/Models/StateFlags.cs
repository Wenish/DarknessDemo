// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.33
// 

using Colyseus.Schema;

namespace Game.Models {
	public class StateFlags : Schema {
		[Type(0, "map", typeof(MapSchema<Flag>))]
		public MapSchema<Flag> flags = new MapSchema<Flag>();
	}
}
