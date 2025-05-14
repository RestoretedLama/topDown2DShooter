# Bomb System for Top-Down 2D Shooter

This module adds throwable bombs to your top-down shooter game.

## Bomb Types

1. **Frag Bomb** - Deals explosive damage in a radius
2. **Flash Bomb** - Stuns enemies in a radius
3. **Molotov** - Creates a fire zone that deals damage over time

## Setup Instructions

### 1. Create Bomb Items

1. Right-click in Project window > Create > Inventory > Item
2. Set `itemType` to `Bomb`
3. Configure basic item properties (name, icon, isStackable)
4. Set bomb-specific properties:
   - **BombType**: Frag, Flash, or Molotov
   - **FuseTime**: How long until detonation
   - **BlastRadius**: Area of effect
   - **BombDamage**: Base damage amount (for Frag) or stun power (for Flash)
   - **DotDuration**: Duration of fire (for Molotov)
   - **BlindDuration**: Now used as stun duration (for Flash bombs)

### 2. Create Stun Effect Prefab

For Flash bombs, you need a visual stun effect:

1. Create a new GameObject called "StunEffect"
2. Add a SpriteRenderer with stars/swirls sprite
3. Add the StunEffect.cs script
4. Adjust rotation and bounce settings
5. Save as a prefab in Resources or Prefabs folder

### 3. Configure Enemy Scripts

For enemies to be affected by Flash bombs:

1. Make your enemy scripts implement IStunnable interface
2. Implement the Stun method and IsStunned property
3. Use the provided StunnableEnemy.cs as a reference implementation
4. Assign the stunEffectPrefab field or use the dynamic stun effect creation

### 4. Configure Bomb Prefabs

1. Create a base bomb prefab:
   - Create an empty GameObject and name it based on type (e.g., "FlashBomb")
   - Add a SpriteRenderer with appropriate sprite
   - Add a Rigidbody2D (mass ~1.0, gravity scale ~1.0)
   - Add a CircleCollider2D (set as trigger)
   - Add the BombController script
   - Add children for particles and sounds
2. Configure BombController in the Inspector:
   - Assign explosion prefab (particle effect)
   - Assign fire zone prefab (for Molotov)
   - Configure sound effects and particles
   - Adjust throw force and arc factor

### 5. Complete the Setup

1. Go back to your bomb Item ScriptableObjects 
2. Assign the appropriate bomb prefab to the bombPrefab field
3. Add the InventoryBombExtension script to your player
4. Assign the throwPoint transform (usually the same as weaponHolder)

## Usage

- Players can select bombs from inventory slots
- Left-click to throw the bomb
- Bombs will detonate after their fuse time
- Flash bombs stun enemies in their radius (enemies turn blue and can't move)
- Frag bombs damage enemies in their radius
- Molotov bombs create fire zones that damage over time

## Extending the System

To add new bomb types:
1. Add a new entry to the BombType enum
2. Add handling for the new type in BombController.Explode() method
3. Create a new bomb prefab and ScriptableObject item 