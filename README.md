# Sonic 4: Episode 1 Deluxe
A much better decompilation of Sonic 4 Episode 1 for Windows Phone 7

# Features
 - Basic controller/keyboard support in the game itself (menus, special stages, and Lost Labyrinth 2 still depend on touch/mouse)
 - Toggleable loop camera [🦀🦀](https://twitter.com/da_wamwoowam/status/1236706830962905089)
<!-- 
 - Discord rich presence
 - Accelerometer support on iOS and Windows Phone (emulation soon)
-->
 - 60 FPS support
 - Vastly better loading times than literally any other version of the game
 - Fully working animations and collision
 - Fully beatable, start to finish, including all emeralds!
 - Editable XML-based save file with additional properties

# Why?
After doing an awful lot of work with [TGE's initial decompile](https://github.com/TGEnigma/Sonic4Ep1-WindowsPhone-Decompilation), I realised that dnSpy introduced way too many subtle issues and bugs for me to find and subsequently fix. I also realised that the additional ports and features I'd been hacking on were simply introducing adding more bugs. So, I felt it was time to pull a Longhorn, so welcome to post-reset Sonic 4 

# Issues
## AppMain.cs is still way too big
Not as well split as previously, should be able to make it better this time.