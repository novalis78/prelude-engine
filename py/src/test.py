from random import randint
import time
from bot import Prelude
from mind import MatchingAlgorithm

print "Prelude@# command line version, welcome user!"
print "if you want to stop chatting, enter: 'exit'"


#create instance
prelude = Prelude()

#configure prelude
#set path to mind file
prelude.setMindFileLocation("mind.mdu")

#set some options, such as...
#...enable TTS
prelude.isSpeaking = True
#...enable an element of true physical randomness
prelude.quantumRandomness = False
#...suppress repetitive learning mode
prelude.avoidLearnByRepeating = True
#...enable proactive response mode
prelude.setProactiveMode(True)
#...set the input match algorithm
prelude.setAssociationAlgorithm(MatchingAlgorithm.Kulczynski)

#now, start your engine ...
prelude.initializeEngine()

prelude.countMindMemory()

question = ""
#and here we go:
while question is not "exit":
	question = raw_input("You:\t\t ")
	answer = prelude.chatWithPrelude(question)


	#chunk answer to make it more chatroom like
	#if comma, split at comma into separate entities
	if "," in answer:
		for commapart in answer.split(","):
			print "Prelude:\t " + commapart
			time.sleep(randint(2,8))
	#if no comma, chunk the answer still to make it look
	#more natural
	else:		
		breaks = answer.split(" ")
		if breaks > 4:
			chunker = randint(4,10)
		c = 0	
		outp = ""
		remainder = ""
		#answers too fast, let's delay a bit
		time.sleep(randint(1,2))
		for part in answer.split(" "):
			outp += part + " "
			c = c+1
			if c == chunker:
				time.sleep(len(outp.split(" ")))
				print "Prelude:\t " + outp
				c = 0
				outp = ""

		print "Prelude:\t " + outp

	#normal, one line output
	#print "Prelude says: " + answer


prelude.stopPreludeEngine()
print "Thanks for chatting with Prelude"