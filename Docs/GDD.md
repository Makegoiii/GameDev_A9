# Coin Quest 3D — Game Design Document
Magzhan Kydyrbai · Game Development Fundamentals · Narxoz University · RK2, weeks 9–13

**1. Title + pitch.** *Coin Quest 3D* — run, jump and shoot through three dangerous levels, collect every coin to open the gate, and defeat the fire boss.

**2. Genre · platform.** 3D action platformer · Android and iOS phones and tablets · **landscape**.

**3. 2D or 3D.** 3D. The hero is a rigged Mixamo character with Idle, Run and Jump animations (Animator with Speed / Grounded parameters).

**4. Core loop.** **Run → Collect coins → Dodge / Shoot enemies → Open the gate** → next level (harder) → run…

**5. Controls (on-screen).**
| Button | Place | What it does |
|---|---|---|
| Joystick | bottom-left | move the hero relative to the camera |
| JUMP | bottom-right | jump (only on the ground) |
| FIRE | bottom-right | shoot forward; hold for auto fire |
| II (Pause) | top-right | pause: Resume / Restart / Menu, time stops |

**6. Content.** 3 levels + boss: Level 1 (tutorial: coins, gate, exit), Level 2 (corridor: holes, spinning blades, falling rocks, patrol enemies), Level 3 (boss with 15 HP: fireballs, bombs from the sky, 2 phases). Items: coins (score, open the gate), hearts (+1 life, max 5). Enemies: patrol enemy, spinning blade, falling rock, bomb, boss.

**7. Art and sound.** Bright, simple low-poly look: Unity primitives with flat colour materials, one Mixamo hero, rounded orange/green/blue UI buttons with gold titles — one world, one style. Sounds and music: Pixabay (coin, game start, explosion, bonus, arcade music loop).

**8. MVP (week-15 build).** Main Menu, HUD and Pause working on all phones (safe area); 3 levels + Win screen; hearts and score; patrol enemy with simple AI; saving of best score and unlocked levels; sound and music with volume; basic effects (coin sparkle, hit flash); Android build.

**9. Cut list (NOT making).** Online multiplayer · shop and in-app purchases · character customisation · more than 3 levels · cutscenes · procedural levels · achievements / leaderboards.

**10. Plan (weeks 9–13).**
| Week | Ready |
|---|---|
| 9 | GDD, GitHub repo, Main Menu / HUD / Pause, safe area, Device Simulator tests |
| 10 | Enemy AI: patrol → chase → attack (NavMesh) |
| 11 | Saving: best score, unlocked levels, settings (PlayerPrefs / JSON) |
| 12 | Sound: music, SFX, volume sliders, audio mixer |
| 13 | Effects and polish: particles, camera shake, Android build test on a phone |

**Roles.** Solo until week 14: Magzhan Kydyrbai — design, programming, level building, UI, sound, testing. Teams for the final project form in week 14.
