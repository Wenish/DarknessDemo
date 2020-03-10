import { Schema, type } from "@colyseus/schema"

import { IStatePlayers, StatePlayers } from "./statePlayers"
import { IStateUnits, StateUnits } from "./stateUnits"
import { IStateFlags, StateFlags } from "./stateFlags"

export class State extends Schema implements IState {
    @type(StatePlayers) public statePlayers: IStatePlayers = new StatePlayers()
    @type(StateUnits) public stateUnits: IStateUnits = new StateUnits()
    @type(StateFlags) public stateFlags: IStateFlags = new StateFlags()
    public navMesh: any;
}

export interface IState extends Schema {
    statePlayers: IStatePlayers
    stateUnits: IStateUnits
    stateFlags: IStateFlags
    navMesh: any
}