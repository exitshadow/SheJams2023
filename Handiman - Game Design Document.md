# Handiman - Game Design Document

## General Information

##### Title

Handiman: The Busy Season

##### Pitch

Iman is a new computer science graduate and lives in a small village at the bottom of the desert’s valley. Because she is very smart, everyone asks her to solve their problems. That makes for interesting encounters.

##### Genre

Mini open-world narrative adventure.

##### Expected total playthrough duration

Around 15 minutes.

## Team

Assignments are given in order of importance

##### CaptainV

Lead Narrative Designer, Lead Producer, Sound Director, Programmer

##### exitshadow

Art Director, Lead Artist, Technical Artist, Lead Programmer, Producer, Additional Narrative Content

##### Reem

Programmer, QA Testing

##### Tylia

Lead Animator, 3D Artist, Programmer

##### Queenhope

Programmer, QA Testing, UI/UX Artist

## Features

Each aspect is summarized and split in features that have to be implemented.

### Core loop

The player goes around, interacts with objects and characters that unlock progression, short quests and/or minigames. Being narrative, the game’s core loop is essentially linear but the mini open-world form allows for a certain flexibility in the order of the tasks having to be done.

##### Features to implement

More details on those in the technical roadmap.

- [x] Game State Tracking
- [x] Quest Advancement System -> look for external tool?
- [x] Dialogue System (that can read quest states) -> yarn spinner?
- [x] Walking / Running animations
- [x] Phone messaging system

### Additional gameplay

Minigames add some gameplay to the core loop of walking and talking.

#### Get the butterfly tree branch

Viewing the tree in first person, the player must hit a branch by throwing stones a few times until it falls while making sure not to hit any birds.

**exitshadow’s note:** Maybe instead of having to "start again" if the birds are all hit, just the text can be prompted and the birds keep being knocked off for a while before returning to the tree. Or just remain on the ground because it’s safer.

##### Features to implement

###### 3D modelling & art

- [ ] Butterfly tree model in 3 states: intact, a bit broken, and broken
- [ ] Butterfly tree branch model
- [x] Butterfly
- [ ] Birds (rigged)
- [ ] Stone model

###### Programming

- [ ] Stone picking system
- [ ] Aiming system (decision making: 1st or 3rd person?)
  - [ ] trajectory finder (3rd person) / shooting aim (1st person)
  - [ ] transition between normal & shooting controls
- [ ] Collision adjustments & conditions
- [ ] Correct integration of throwing & pickup animations & sounds
- [ ] Integration of bird animations & sounds

###### UI

- [ ] Target indicator
- [ ] Trajectory / Aim indicator

###### Animation

- [ ] Iman

  - [ ] Throwing
  - [ ] Picking up

- [ ] Butterfly

  - [x] Idle
  - [ ] Flying

- [ ] Birds

  - [ ] Idle, on a branch
  - [ ] Flying
  - [ ] Landing
  - [ ] Taking off

  ###### Sounds

  - [ ] Stone
    - [ ] picking
    - [ ] throwing
    - [ ] hitting
  - [ ] Bird
    - [ ] flapping wing sounds
    - [ ] chirping
  - [ ] Branch
    - [ ] crack 1
    - [ ] crack 2
    - [ ] crack 3
    - [ ] falling on the ground
  - [ ] Victory music / sound effect

#### Catching all the butterflies inside the neighbour’s house

Holding the branch, the player must attract all the butterflies to it by going around the room, that is covered in butterflies. Once a butterfly is close enough, it automatically flies to to the branch. The minigame is completed when all butterflies are caught and the neighbour’s decoration, that consists also of butterflies, is revealed.

##### Features to implement

###### 3D modelling & art

(see Locations)

###### Programming

That’s going to be a *terrible* pain in the ass, and will probably affect performance. Not thousands of butterflies here.

WITH TRUE AI FLYING MODEL (Difficult)

- [ ] Sensitivity of butterflies to branch decreasing from distance

- [ ] Assign destination on the tree branch, per butterfly
  - [ ] smart ass solution: use the mesh’s vertices + separation between positions

- [ ] Butterfly attraction (AI pathfinding)
  - [ ] Voxel spacing of coordinates
  - [ ] Pathfinding algorithm
  - [ ] Bezier trajectory smoothing


Another technical solution could be more of a sweep alternative—Iman brushes the living room and the butterflies get stuck to it upon contact, at their point of collision. Could also have much better performance. However, a few butterflies are bound to remain.

WITH COLLISION MODEL (Intermediate)

- [ ] Create colliders that correspond to the tree branch (can get them from previous minigame)
- [ ] Butterflies get stuck (parent to the tree branch) on contact point with the collider

###### Animation

- [ ] Iman cleaning butterflies
  - [ ] Holding
  - [ ] Brushing
  - [ ] Sweeping

###### Sound effects

- [ ] Branch
  - [ ] Brushing
  - [ ] Sweeping
- [ ] Butterflies
  - [ ] Flap
  - [ ] Pop

#### Driving the jeep to the farm and back

Nour wants to get on the jeep so she can show off her new camo hijab (and help her aunt), but she actually doesn’t know how to drive. She asks Iman to drive the jeep up to her aunt’s farm, who needs a lift to get her duck to the veterinary. We drive to the farm, then back to the vet in the village.

##### Features to implement

###### 3D modelling & art

- [ ] Stylized Jeep Wrangler (rigged wheels)

###### Programming

- [ ] 4x4 vehicle driving system
  - [ ] Chassis has to balance (approximately) on top of the two wheels axis
    - [ ] possible use of hinged joints
  - [ ] Increase gas
    - [ ] Turning wheels
  - [ ] Toggle direction forward / backward
  - [ ] Breaking
- [ ] Compass system that indicates where the aunt farm is

###### Animations

- [ ] Sat idle animations
  - [ ] Nour
  - [ ] Auntie with duck
- [ ] Iman driving animations
  - [ ] Turn left
  - [ ] Turn right
  - [ ] Looking back
  - [ ] Honking
- [ ] Duck quacking / honking / flapping wings

###### Sound effects

- [ ] Jeep sounds
  - [ ] Motor sound
  - [ ] Tires screeching sound
  - [ ] Honking
- [ ] Duck
  - [ ] Quacking
  - [ ] Flapping

#### Catch Mister Duck

Mister Duck is hidden in the middle of similarly-looking ducks. All ducks are quite wary of humans, so they will not let anyone approach. However, they do not see Nour since she’s camouflaged. The aim of the minigame is to push Mister Duck towards Nour who will then catch him by surprise.

##### Features to implement

###### 3D modelling & art

- [ ] Mister Duck (rigged, including wings)
- [ ] Other ducks (same model with textures or colour variations)

###### Programming

- [ ] Create nav mesh system that *avoids* the player and other agents

​		https://forum.unity.com/threads/navmesh-agents-to-avoid-other-agents.500416/

- [ ] Set trigger event on corner where Mister Duck has to go
- [ ] Set conditions to make Mister Ducks’ movements somewhat predictable
  - [ ] Sphere Trigger Colliders of "sensing areas" around Iman & the duck
  - [ ] When both colliders collide, use Iman’s collider normal at point of contact to use as direction to project the duck, Y axis neutralized
- [ ] Add noise & Bezier smoothing to Mister’s trajectory

## Locations

### Open-word map

- [ ] Iman’s Valley Village
  - [ ] Iman’s house
  - [ ] Neighbour’s house
  - [ ] Nour’s house
  - [ ] Vet’s dispensary
  - [ ] Park
  - [ ] Other random houses & walls
  - [ ] Flowers
  - [ ] Trees
  - [ ] Butterfly tree
  - [ ] Bushes
  - [ ] Benches
  - [ ] Light posts
  - [ ] Wires



- [ ] Nour Auntie’s Duck Farm
  - [ ] Main house
  - [ ] Duck house
  - [ ] Duck enclosure
  - [ ] Sheep enclosure
  - [ ] Irrigated farm / small canals
  - [ ] Green stuff growing
  - [ ] Cute alleys
  - [ ] Date tree
  - [ ] Ducks + idle, walking animations
  - [ ] Sheep + idle, laying down, walking animations
  - [ ] Show card with "Welcome to the Happy Valley Farm" written in Arabic



- [ ] Secret Base Tarmac at the Top of the Ridge

### Specific scenes maps

- [ ] Iman’s house interior (living room)
  - [ ] Dining table
  - [ ] Chair
  - [ ] Sofa
  - [ ] Library with many many books
  - [ ] Open books and magazines
  - [ ] Coffee pot & mugs
  - [ ] Desk
  - [ ] Computer
  - [ ] Computer chair
  - [ ] Framed poster of a rocket with "Future is now!" written in Arabic
  - [ ] A TV
  - [ ] A framed portrait of Iman’s family with baby Iman, mom and dad
  - [ ] Entrance door
  - [ ] Corridor door



- [ ] The neighbour’s living room
  - [ ] Furniture
    - [ ] Table
    - [ ] Sofa
    - [ ] Chairs
    - [ ] TV Console
    - [ ] Frame with "Butterfly" written on it in Arabic
    - [ ] Tea pot, mugs and dishes
    - [ ] Kitsch clock
    - [ ] Family portraits
    - [ ] Lustre light
    - [ ] Vases with butterflies
    - [ ] Wall dishes with butterflies
    - [ ] Kitsch curtains
  - [ ] Butterfly wallpaper texture
  - [ ] Entrance door
  - [ ] Corridor door



- [ ] Veterinarian’s dispensary
  - [ ] Examination table
  - [ ] Front desk
  - [ ] Computer
  - [ ] Cages on the side
  - [ ] Cat in the cage
  - [ ] Entrance glass door
  - [ ] Private area door
  - [ ] Bathroom door
  - [ ] Waiting room
  - [ ] Waiting Bench
  - [ ] Someone with a sheep
  - [ ] Posters
    - [ ] "Do not forget the deworming pills or you’ll regret it" with a screaming emoji
    - [ ] "The secret of perfect wool is love" with a flock of happy sheep
    - [ ] "Stop giving tuna to your cats!"