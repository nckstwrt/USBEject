# USBEject
 Eject USB Drives (Force closing any open handles as required by the user)

## The Problem
![](https://i.imgur.com/EgA4kyj.png)

^ This is/was the bane of my existence!

I use a **lot** of external hard drives on Windows. Even after only attaching the hard drive for a few seconds and not using it - Windows would still pop up to say the above when trying to remove it

## The Solution
Windows could tell you what is causing the hard drive to not eject...but it doesn't. It even writes the to System Event Log as ID 225 the process and PID that is stopping the ejection.

So I built this app in about 20 mins mainly by piecing together code from the Internet to do the following:
1. Try to eject the drive
2. If unable to eject the drive search the event log for event 225 and use RegEx to parse the Process and PID
3. Ask the user if they want to kill it? If so, go to 1. If not, exit.
4. If the Event Log just lists System (PID:3) as the process then a Windows Service is blocking the ejection
5. Loop over all processes for their open file handles and see if they are attached to the drive
6. If the process is a Windows Service ask the user if they want to stop that service? If so, go to 1. If not, exit

(on exit it will restart any services it stopped)

## Issues
* Because it is using just a simple RegEx on the EventLog this probably will not work for Non-English version of Windows
* Could be a whole host of other issues - I coded this extremely quickly and just really for my use, but throwing it up on here just in case someone else needs it
