# 1.2.1 #

  * Compatibility fix for interoperating with [DeskBeam Junior](http://www.kalamonsoft.com/dbjr)
# 1.2.0 #

  * [bug #65](https://code.google.com/p/mazio/issues/detail?id=65) - optimized working with multiple text widgets. Mazio no longer hangs, crashes or grinds to a halt when you add many texts
  * [bug #66](https://code.google.com/p/mazio/issues/detail?id=66) - text widget does not get stuck in editing mode any more
  * [bug #67](https://code.google.com/p/mazio/issues/detail?id=67) - you can save the annotated screenshot as editable file (with .maz extension). You can then reopen the file and continue editing
  * [bug #71](https://code.google.com/p/mazio/issues/detail?id=71) - window location and size detection fix for 64-bit systems

# 1.1.0 #

  * Got rid of non-intuitive "editing" mode for shapes. Now, you can instantly modify shapes by simply selecting them and then dragging or moving
  * [bug #24](https://code.google.com/p/mazio/issues/detail?id=24) - ability to add additional images on top of grabbed screenshots. This also includes copying and pasting these images - Jacek, I hope you will like this
  * [bug #50](https://code.google.com/p/mazio/issues/detail?id=50) - cropping to defined size/proportion
  * [bug #56](https://code.google.com/p/mazio/issues/detail?id=56) - size of grabbing region, screenshot coodrinates and crop size shown             during screenshotting and cropping
  * [bug #63](https://code.google.com/p/mazio/issues/detail?id=63) - option to fill rectangles and ovals
  * [bug #17](https://code.google.com/p/mazio/issues/detail?id=17) - hints for shape usage

# 1.0.7 #

  * [bug #55](https://code.google.com/p/mazio/issues/detail?id=55), [bug #57](https://code.google.com/p/mazio/issues/detail?id=57) and [bug #58](https://code.google.com/p/mazio/issues/detail?id=58) fixed - thanks to Ingo Kitzmann
  * better fonts in text shape (no, really)

# 1.0.6 #

  * [bug #54](https://code.google.com/p/mazio/issues/detail?id=54) fixed
  * improved fonts rendering in text shape
  * more in-your-face update notification

# 1.0.5 #

  * [bug #53](https://code.google.com/p/mazio/issues/detail?id=53) fixed

# 1.0.4 #

  * updated JIRA uploader to work with JIRA 4.1 - requires mazio-jira plugin version 0.6 to be installed on the JIRA server
  * sneak peak of the upcoming image transformation functionality (perspective and rotation). This is an experimental code, and the image quality as well as performance is not quite there yet

# 1.0.3 #

  * [bug #45](https://code.google.com/p/mazio/issues/detail?id=45) fixed (self-signed certs supported while uploading)

# 1.0.2 #

  * [bug #44](https://code.google.com/p/mazio/issues/detail?id=44) fixed (removable text shadow)
  * setting font for texts
  * remembering shape settings between restarts

# 1.0.1 #

  * It is now possibel to pass a JIRA SOAP session token on a command line or in the protocol handler, so that applications that use SOAP can invoke Mazio to make it upload attachment to a JIRA issue

# 1.0.0 #

  * Rejoice! Here comes a stable version!

# 0.104.0 #

  * multiline text boxes
  * free-floating shape settings dialog instead of hideable shape settings panel

# 0.103.0 #

  * Improved [SPARTEZ MeetingRoom](http://www.spartez.com/en/products-plugins-jira-idea/meeting-room.html) uploader

# 0.102.0 #

  * when launched from JIRA, using the [mazio-jira](https://plugins.atlassian.com/plugin/details/5178) plugin, uploadijng to JIRA does not require password any more
  * much better-looking fonts

# 0.101.0 #

  * Improved upload to JIRA, using multipart POST instead of SOAP. Should work with JIRA 4

# 0.100.0 #

  * I forgot what it was. It was a year ago :)

# 0.99.0 #

  * Fixed [bug #40](https://code.google.com/p/mazio/issues/detail?id=40) - crash on startup when fonts are 120DPI (seriously, the handling of resize with non-standard fonts sucks)
  * catch-all crash handler dialog with "semi-automatic" error reporting

# 0.98.0 #

  * Multi-monitor support (courtesy [Olyen2007](http://code.google.com/u/olyen2007/))
  * improved cropping
  * "Fit" button fits and centers the cropped area

# 0.97.0 #

  * "Save" and "Save As" implemented
  * Copy and Paste implemented
  * keyboard shortcuts for the above commands available

# 0.96.1 #

  * Fixed crash when uploading to Picasaweb and the server breaks connection - previously unhandled IOException is now cought and displayed

# 0.96.0 #

  * Launching mazio from web pages - use the "mazio:" pseudo-protocol, registered by program's installer. This currently only works for JIRA servers. The correct URL to use is: mazio:jira:[user@]jirahost:issuekey. Example: "mazio:user@http://myjira.org:ISSUE-1"
  * better installer: nicer looks, ability to launch Mazio at the end of installation

# 0.95.0 #

  * New and improved startup looks, with a "grab screenshot" and "start drawing" buttons and a clickable SPARTEZ company logo (a bit of advertising, I know, I hope it is not too obtrusive)
  * shape settings dialog now opens to the inside of the drawing pane. It also auto-hides when you start drawing the next shape
  * You can now edit texts after they are selected. Just select the text shape and double-click on it to change the text
  * text shapes are now finally drawn properly with outlined and filled graphics paths instead of the stupid thing that I did before.
  * you can now select two types of arrows to draw, using arrow shape settings panel

# 0.90.0 #

Well, we are getting really close to the 1.0 release now. 0.90 took me a while, but it was worth it. Details below:

  * Totally redesigned shape drawing framework - now uses a variation of MVC. Resulted in breakthrough in performance (you can now actually use magnifying glass tools without degrading performance)
  * zooming the whole picture in or out. Both during the actual drawing, and for saving/upload
  * you can save/upload both in 1:1 scale and zoomed/shrinked
  * settings panels for shapes, allowing tweaks to the looks of individual shapes:
    * magnifying glasses have individually adjustable zoom factor
    * all shapes can have or not have shadows
    * ponies can be either stars or hearts and you can switch them on the fly
  * ponies have variable star/heart size
  * entering texts finally does not suck
  * redesigned upload dialogs:
    * you can have multiple account definitions for picasaweb, skicth and JIRA
    * the looks of the dialogs are much nicer - flat and matching the look of mazio main window
  * cleanup of resources (icons and such) for easy "OEMing"

# 0.10.0 #

  * new icons for some buttons (borrowed from some open source projects)
  * z-ordering of shapes
  * versatile undo-redo framework
  * undo-redo for everything but z-ordering and shape deletion
  * pixelizing anonymizer
  * magnifying glass
  * numerous bugfixes, code cleanup and performance enhancements
  * usage hints are only shown if indicated in the settings

# 0.9.0 #

  * upload to Skitch, using Skitch MailDrop
  * changing colors and line widths/font size of shapes during edits
  * transparency support
  * Ponies! - Two of them!!!
  * improved look of the cropping frame
  * further performance improvements
  * displaying usage tips made optional
  * it is now possible to upload or drag a drawing without a captured screenshot (basically made Mazio a simple vector drawing utility with optional screen capturing :))

# 0.8.2 #

  * autoupdate fixes - truns out googlecode does not allow editig files that have already been uploaded there, so I had to move the latest-version.xml file from "downloads" section of the project page to SVN (where it probably belonged in the first place :))

# 0.8.1 #

  * b0rked, do not download :)

# 0.8.0 #

  * Editing and deleting shapes (not hooked up with undo/redo yet)
  * vastly improved performance of the pencil drawing

# 0.7.0 #

  * Automatic updates

# 0.6.0 #

  * more precise detection of mouse hovering over oval and pencil shapes
  * vast improvements of efficiency and design of the screen grabber code

# 0.5.0 #

  * shapes can be moved around (not reflected inundo/redo history yet)
  * texts have borders in black or white color
  * better handling of HSV->RGB conversions (they were absolutely lame before)

# 0.4.0 #

  * Support for cropping "tight"
  * saving tool presets in registry
  * Picasaweb upload support
  * cancelling grab on Esc key

# 0.3.0 #

  * cropping support
  * fixed resize so that shapes stay in place relative to the screengrab

# 0.2.0 #

  * New, cool look, got rid of toolbars
  * shapes have shadows
  * improved pencil drawing
  * undo, redo

# 0.1.0 #

  * opening image files (jpg and png)
  * support for dragging image files into mazio
  * usage hint
  * improved About box
  * rounded ends of the free-drawn (pencil) lines
  * commenting JIRA image uploads
  * descriptions of Flickr image uploads

# 0.0.6 #

  * New - upload to JIRA!
  * improvements to upload to Flickr (multithreading)

# 0.0.5 #

  * Basic upload to Flickr
  * Installer

# 0.0.4 #

  * much improved text input functionality
  * free-hand drawing ("pencil") functionality
  * line width dial

# 0.0.3 #

  * switched IDE from [SharpDevelop](http://sharpdevelop.net/OpenSource/SD/) to Visual Studio Express 2008

# 0.0.2 #

  * straight line drawing capability

# 0.0.1 - Initial Release #

  * grabbing a single window
  * grabbing a region of screen
  * drawing arrows
  * drawing ovals
  * drawing rectangles
  * crude text input capability
  * only one line-width setting
  * color selection
  * dragging a finished grab to desktop
  * PNG and JPG output file support
  * minimizing to task tray
