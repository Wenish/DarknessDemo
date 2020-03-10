import { Action } from "./index";
import { IState } from "../state";
import { Client } from "colyseus";
import { FLAG_PLACE } from "./actionTypes";
import { Flag } from "../models/flag";

export const actionFlagPlace: Action<IState, Client> = (room, state, isServer, client, payload) => {
    try {
        if (!client) throw 'no client was passed'
        if(!payload) throw 'payload is not defined'
        if(!payload.x) throw 'payload.x is not defined'
        if(!payload.z) throw 'payload.z is not defined'

        const flag = Flag.generate()
        flag.idPlayer = client.sessionId
        flag.position.x = payload.x
        flag.position.z = payload.z
    
        state.stateFlags.addFlag(flag)
    } catch (err) {
        console.error('Error', FLAG_PLACE, err)
    }
}