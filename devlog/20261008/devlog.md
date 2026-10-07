# Murder Hobo of SPLORR!! now runs in yer browser, and remembers yer murders

(Draft for the itch.io devlog. Written in the repo; the user posts it. Cut freely.)

Murder Hobo of SPLORR!! is back, and this time you do not have to download 36 MB to commit a murder.

Here is what happened.

## The old one

In 2024 I wrote Murder Hobo in VB.NET on MonoGame. It was a small game with a big framework underneath it: menus, a pixel font, a layered pile of dialogs and choices, all so that you could press one button and get experience points. Numbers went up. A commenter was happy about that, and so was I.

It shipped as three zips (Windows, Linux, Mac) and it never saved anything. Close the window and yer murders were gone, like they never happened. Which, for a murder hobo, is arguably the point, but still.

## The new one

This week I rebuilt it in Odin, compiled to WebAssembly, so it runs in the page. Same game, same numbers, same menus, same deadpan. Differences:

- **It runs in the browser.** Click, murder.
- **It saves itself.** Every move is stored in yer browser. Close the tab, come back, hit Continue Game. If yer auto-murder was running while yer gone, it catches up (to a point: a thousand missed murders at most, the rest are forfeit, because a murder hobo needs a hobby).
- **The numbers no longer wrap.** The old game would have crashed at about 21 million murders (a 32-bit integer overflow in the success rate, found while checking the port). The new one stops counting at nine quadrillion, which I am confident nobody will reach.
- **A new font.** The old one was borrowed from somewhere it could not be shipped from. The new one is m5x7 by Daniel Linssen (managore), which is free to use and which I am happy to credit: https://managore.itch.io/m5x7. It is a little taller, so the screen has three fewer lines.
- **Mouse and touch work.** Tap a choice. Tap twice with a finger, once with a mouse.

The rules are the same, including the one that looks like a mistake and is not: failing a murder pays double the experience of succeeding at it, until yer streak catches up. Make of that what you will.

There is still no ending.

## How it was made (the honest part)

I built this one with Claude, an AI model, and I want to say so plainly. I decided what the game should do, what to keep and what to change, and I played it. Claude wrote the Odin. I made about fourteen small decisions about the old game's quirks before any code existed (what to do with the unbounded auto-murder catch-up, whether the cursor should remember where it was, whether failing should pay double), and every one of them is written down.

Because the old game still existed, it could be used as a referee. A little program drives the original VB.NET game with no window and records what it draws; the new game's tests then require every pixel of its menus and screens to match. When I swapped the font, the same program re-recorded the originals with the new font, and they still match. That is the part I am proudest of, and it is the dullest part to look at.

There is also a native desktop version (an SDL2 window, same code). It is not on this page; the browser build is the one that is.

## Housekeeping

The old Windows, Linux and Mac downloads are being removed from this page. They cannot be rebuilt and they do not save.

P.S. This game is still about crows.
