# CheckWinRaid
Commandline tool to check windows built-in software raid.

Basically, DISKPART parser.

[Download exe binary](https://github.com/timhok/CheckWinRaid/releases/download/v1/CheckWinRaid.zip)

[Source code in one module](https://github.com/timhok/CheckWinRaid/blob/master/CheckWinRaid/Module1.vb)

## Usage:
`CheckWinRaid.exe` must be run with administrator privileges (due to DISKPART usage) and [list_volume](https://github.com/timhok/CheckWinRaid/blob/master/list_volume) file placed in current working directory
#### Output:
`<Volume letter> <Boolean status>`
#### Example output:
```
D True
C True
F True
S True
```
True = Volume have "Healtly" state

## Alert url function:
If any volume change its status from "Healtly" - request will be made at defined url.

`CheckWinRaid.exe -c <url>`

### Example:
`CheckWinRaid.exe -c "https://api.telegram.org/bot<token>/sendMessage?chat_id=<id>&text=Alert!%20Your%20RAID%20Array%20on%20danger!"`

I can put this in Windows Task Scheduler and when something goes wrong i will recieve notification in telegram messenger.
