import { Schema, type } from "@colyseus/schema";
import { clamp } from "../../../utility/clamp";

export class Bar extends Schema {
    @type('number') public current: number
    @type('number') public max: number
    @type('number') public regenerationSpeed: number

    private lastRegeneration: number = 0;

    remove(value: number): void {
        this.current = clamp(this.current - value, 0, this.max)
    }

    add(value: number): void {
        this.current = clamp(this.current + value, 0, this.max)
        console.log(this.current)
    }

    reset(): void {
        this.current = this.max
    }

    regenerate(elapsedTime: number) {
        const isRegenerationReady = this.lastRegeneration <= (elapsedTime - 1000)
        if (!isRegenerationReady) return

        this.lastRegeneration = elapsedTime
        this.add(this.regenerationSpeed)
    }
}