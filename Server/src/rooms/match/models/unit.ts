import shortid from 'shortid'
import BigNumber from "bignumber.js"
import { Schema, type } from '@colyseus/schema'
import { Position } from './position'
import utility from '../../../utility'
import { Bar } from './bar'
import { WeaponLoadout } from './weaponLoadout'
import weapons from '../../../../data/weapons.json'
import { WeaponType, WeaponSlot, Weapon, CombatStyle } from './weapon'


export class Unit extends Schema {
    @type('string') public id: string
    @type('string') public name: string
    @type(Position) public position: Position
    @type('number') public moveSpeed: number
    @type('number') public rotation: number
    @type('number') public locomotionAnimationSpeedPercent: number
    @type('boolean') public isAlive: boolean
    @type(Bar) public health: Bar
    @type(Bar) public energy: Bar
    @type(WeaponLoadout) public weaponLoadout: WeaponLoadout

    public moveTo: Position[] = [];

    constructor (id?: string) {
        super();
        this.id = id || shortid.generate()
    }

    public setMoveTo (path: Position[]) {
        if (!this.isAlive) return
        
        this.moveTo = path
        this.rotate()
        console.log('Set Path', JSON.stringify(path))
    }

    public setRotation (x: number, z: number) {
        var angle = (Math.atan2(x - this.position.x, z - this.position.z) * (180/Math.PI))
        if (angle < 0) {
            angle = angle + 360
        }
        this.rotation = angle
    }

    public removeHealth(trueDamage: number) {
        console.log('is alive', this.isAlive)
        console.log('health', this.health)
        if (!this.isAlive) return

        /*
        const finalPhysicalDamage = physicalDamage * (100 / (100 + this.attributes.armor))
        const finalMagicDamage = magicDamage * (100 / (100 + this.attributes.magicResistance))
        const totalDamage = finalPhysicalDamage + finalMagicDamage + trueDamage
        */
        this.health.remove(trueDamage)

        const isAlive = this.health.current > 0 ? true : false

        if (!isAlive) {
            this.locomotionAnimationSpeedPercent = 0
            this.setMoveTo([])
            this.kill()
        }

        console.log('is alive', this.isAlive)
        console.log('health', this.health)
    }

    public addHealth (health: number) {
        if (!this.isAlive) return
        
        this.health.add(health)
    }

    public addEnergy (energy: number) {
        if (!this.isAlive) return

        this.energy.add(energy)
    }

    public removeEnergy (energy: number) {
        if (!this.isAlive) return

        this.energy.remove(energy)
    }

    public kill () {
        this.health.remove(this.health.max)
        this.energy.remove(this.energy.max)
        this.isAlive = false
    }

    public revive () {
        this.health.reset()
        this.energy.reset()
        this.isAlive = true
    }
    
    public equipWeaponLoadout (newWeaponLoadout: WeaponLoadout): WeaponLoadout {
        const oldLoadout = this.weaponLoadout
        this.weaponLoadout = newWeaponLoadout
        return oldLoadout
    }

    update(deltaTime: number, elapsedTime: number) {
        if (this.isAlive) {
            this.move(deltaTime)
            this.health.regenerate(elapsedTime)
            this.energy.regenerate(elapsedTime)
        }
    }

    move (deltaTime: number): void {
        if(!this.moveTo.length) return
        const destination = this.moveTo[0]

        const distance = utility.distanceBetween(this.position.x, this.position.z, destination.x, destination.z)
        const isEntityAtDestination = distance == 0
    
        if (isEntityAtDestination) {
            this.moveTo.shift()
            if (!this.moveTo.length) {
                this.locomotionAnimationSpeedPercent = 0
                return
            }

            // calls itself till nothing is left in the moveTo array
            this.move(deltaTime)
            this.rotate()
            return
        }
    
        const moveSpeedPerSec = new BigNumber(this.moveSpeed).dividedBy(60).toNumber()
        const distanceToTravel = new BigNumber(moveSpeedPerSec).dividedBy(1000).multipliedBy(deltaTime).toNumber()
        const t = utility.clamp(new BigNumber(distanceToTravel).dividedBy(distance).toNumber(), 0, 1)
        this.position.x = utility.lerp(this.position.x, destination.x, t)
        this.position.z = utility.lerp(this.position.z, destination.z, t)
        // TODO: calculate number from moveSpeedPerSec
        this.locomotionAnimationSpeedPercent = 0.6
    }

    rotate (): void {
        const lookTo: Position = this.moveTo[0]
        if (lookTo) {
            this.setRotation(lookTo.x, lookTo.z)
        }
    }

    static generate (): Unit {
        const unit = new Unit()
        unit.name = 'Unknown'
        unit.position = new Position(10, 0, 10)
        unit.moveSpeed = 300
        unit.rotation = 0
        unit.locomotionAnimationSpeedPercent = 0
        unit.health = new Bar()
        unit.health.max = 300
        unit.health.current = 200
        unit.health.regenerationSpeed = 3

        unit.energy = new Bar()
        unit.energy.max = 30
        unit.energy.current = 0
        unit.energy.regenerationSpeed = 1

        unit.isAlive = true

        const filteredWeapons = weapons.filter(obj => {
            return obj.type == WeaponType.Bow
        })
        const weaponToEquip = filteredWeapons[Math.floor(Math.random() * filteredWeapons.length)]
        const weaponMain = new Weapon()
        weaponMain.slot = WeaponSlot[weaponToEquip.slot]
        weaponMain.type = WeaponType[weaponToEquip.type]
        weaponMain.combatStyle = CombatStyle[weaponToEquip.combatStyle]
        weaponMain.attackRange = weaponToEquip.attackRange
        weaponMain.attackSpeed = weaponToEquip.attackSpeed
        const weaponLoadout = new WeaponLoadout()
        weaponLoadout.mainHand = weaponMain
        unit.weaponLoadout = weaponLoadout

        return unit;
    }
}