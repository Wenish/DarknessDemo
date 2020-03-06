import { Schema, type } from "@colyseus/schema";
import { clamp } from "../../../utility/clamp";

export class Bar extends Schema {
    @type('number') public current: number
    @type('number') public max: number
    @type('number') public regenerationSpeed: number

    remove(value: number): void {
        this.current = clamp(this.current - value, 0, this.max)
    }

    add(value: number): void {
        this.current = clamp(this.current + value, 0, this.max)
    }

    reset(): void {
        this.current = this.max
    }
}