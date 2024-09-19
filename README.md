# Horde Slayer
 
Scripts respository of Horde slayer; a top down zombie shooter game. More info can be found here: https://bit-bandit98.github.io/Projects/HordeSlayer.html

Please note that some scripts are missing as they are not my property.

# Code Reflection

- Good attempt at using inheritance to reuse code for the enemy and player classes.
- Code could be split up a bit more, including the "BasicEnemy" class, which does a bit too much. Pause functionality should be it's own class, rather than in the "CharacterMovement" class.
- Code, at times, too inefficient and needlessly complex. One example is the "CharacterMovement" class' "MovementFunction" method, which ineffectively calculates which animation to play based on the player's direction and movement.
- Too much is hardcoded, leading to limited extensibility.
- Powerups should be created on a class by class basis. For example, the "Haste" powerful, which increases speed, should potentially be a class that inherents a powerup class or interface.
- Powerups should **not** alter the player's stats directly, but instead be a supplement to it. For example, a mediator class that returns the player's stats with powerups as a factor would work much better, lest there be side effects.
