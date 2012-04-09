===============================================
		
	        PRELUDE @ # 
		E N G I N E

   		 v. 0.9.6 

    Copyright (c) 2003-5 Lennart Lopin.
            see license.txt for
            license information 

      http://novalis78.topcities.com  

===============================================

	INSTALLATION
	REDISTRIBUTION
	DOKUMENTARY
	CONTACT INFO

===============================================
INSTALLATION
===============================================

Simply reference all three assembly file which
you will find in the "assemblies" folder of this
package. If you dont wont to support TTS you may
drop the "Interop.SpeechLib" - see below for 
documentary details. However, PreludeEngine.dll and
StringTokenizer.dll are essentiel

===============================================
REDISTRIBUTION
===============================================

See the license file for details. You are free to
redistribute the PreludeEngine according to the
license obligations...

===============================================
DOCUMENTATION?
===============================================

Prelude@# is based on Microsoft's .NET technology.
So in order to successfully run the program you
will need to download and install the latest Frame-
work (Framework 1.0 and higher)

Integration of the PreludeEngine is sooo simple,
i did not want to bother you with *.chm or *htm
files, so here follows a very comprehensive
discussion of what to do:

1.) Add the Prelude Engine's namespace to your
project,e.g. >using PreLudeEngine"

2.) Instantiate the PreludeEngine, e.g. 
>PreLudeInterface pi = new PreLudeInterface();

3.) Set all attributes, which qualify your bot.
There are only these two attributes to take care
of:
	a.)loadedMind - (absolute) Path to your mind file

	b.)isSpeaking - "true/false" (bool - if you want to
	enable TTS support. Make sure you add the
	Interop.SpeechLib to your project)

4.) Before using the PreludeEngine, you have to
initialize it. This will be necessary only ONCE, e.g.
>pi.initializeEngine();

Thats it! Now, if you wanna chat with Prelude, just
call this method:

5.)Supply a string (input) for the following method:
   >string OUTPUT = pi.chatWithPrelude(INPUT);
   This will return a string with the bot's answer.

6.)Whenever your program wants to stop the
bot chitchat, dispose the PreludeEngine Interface by calling:
>pi.stopPreludeEngine();

Stopping the bot will automatically invoke a saving
operation, which will take care of all new stuff
the Prelude bot learnt during your session.

However, if you want to initate a custom "save to long
term memory" simply call this method:
>pi.forcedSaveMindFile();

By the way, if you want to know, how much your bot
learnt, call 
>pi.countMindMemory() - this will return an "int"

you may also want to display version information:
>pi.getVersionInfo();



===============================================
CONTACT INFO
===============================================

Comments, suggestions, criticism & bugs welcome:
Just send an email to lenni_lop@yahoo.de


===============================================
Copyright (c) 2003 Lennart Lopin. 
Alle Rechte vorbehalten.

