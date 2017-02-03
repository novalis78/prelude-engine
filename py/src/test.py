from bot import Prelude
from mind import MatchingAlgorithm

print "Prelude@# command line version, welcome user!"
print "if you want to stop chatting, enter: 'exit'"


#create instance
prelude = Prelude()

#configure prelude
#set path to mind file
prelude.setMindFileLocation("mind2.mdu")

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
prelude.setAssociationAlgorithm(MatchingAlgorithm.Hamman)

#now, start your engine ...
prelude.initializeEngine()

prelude.countMindMemory()

question = ""
#and here we go:
while question is not "exit":
	question = raw_input("You say: ")
	answer = prelude.chatWithPrelude(question)
	print "Prelude says: " + answer
	prelude.forceSaveMindFile()

prelude.stopPreludeEngine()

print "Thanks for chatting with Prelude"
