# Salvatalon Ideas

Random ideas for things that might be fun to add someday.
No roadmap, no promises, no particular order.

- Refactor command parsing to preserve original argument capitalization while keeping command and target matching case-insensitive.
- Make _sessions (MudServer.cs) thread-safe (currently using a normal list)
- Actually handle exceptions in MudServer HandleSession() and not only log them.
- Messages when players enter/leave rooms
- Allow `push` to target players
- Add stats
- Strength affects whether you can push something
- Suspicious Pigeon: easy to push
- Town Guard: not so easy to push
- NPCs might react to being pushed
- Add inventory for players
- Coins (cc, sc, gc and pc)
- Bank (store and change coins)
- Command inform on/off (show log in and log out and such)

Done:

- `say` command
- Players can see other players in rooms