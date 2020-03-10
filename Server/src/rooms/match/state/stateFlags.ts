import { Schema, type, MapSchema } from "@colyseus/schema"
import { Flag } from '../models/flag'

export class StateFlags extends Schema implements IStateFlags {
    @type({ map: Flag })
    public flags: MapSchema<Flag> = new MapSchema<Flag>();
    
    public addFlag (flag: Flag): void {
        this.flags[flag.idPlayer] = flag
        console.log('added flag')
    }

    public removeFlag (idPlayer: string): void {
        delete this.flags[idPlayer]
        console.log('removed flag')
    }
}

export interface IStateFlags extends Schema {
    flags: MapSchema<Flag>

    addFlag (flag: Flag): void
    removeFlag (idPlayer: string): void
}