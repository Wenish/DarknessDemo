import { Schema, type } from "@colyseus/schema";
import { Position } from "./position";

export class Flag extends Schema {
    @type('string') public idPlayer: string
    @type(Position) public position: Position

    static generate (): Flag {
        const flag = new Flag()
        flag.position = new Position(0, 0, 0)
        return flag;
    }
}