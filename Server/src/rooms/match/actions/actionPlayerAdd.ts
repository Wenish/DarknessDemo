import { Action } from "./index";
import { IState } from "../state";
import { Client } from "colyseus";
import { Unit } from "../models/unit";
import { Player } from "../models/player";
import { PLAYER_ADD } from "./actionTypes";
import { actionFlagPlace } from "./actionFlagPlace";

export const actionPlayerAdd: Action<IState, Client> = (room, state, isServer, client, payload) => {
    try {
        if (!isServer) throw 'action can only be called from server'
        if (!client) throw 'no client was passed'

        const unit = Unit.generate()
    
        state.stateUnits.addUnit(unit)
    
        var player = new Player(client.sessionId)
        player.idUnit = unit.id
        state.statePlayers.addPlayer(player)
        actionFlagPlace(room, state, isServer, client, {
            x: unit.position.x,
            z: unit.position.z
        })
    } catch (err) {
        console.error('Error', PLAYER_ADD, err)
    }
}