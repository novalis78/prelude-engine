import sys
from flask import Flask
from bot import Prelude
from mind import MatchingAlgorithm
import logging
from logging.handlers import RotatingFileHandler

app = Flask(__name__)

handler = RotatingFileHandler("chat.log", maxBytes=1000000, backupCount=100)
handler.setLevel(logging.INFO)
app.logger.addHandler(handler)

#create instance
prelude = Prelude()

#configure prelude
#first, set path to mind file
prelude.loadedMind = "mind.mdu"

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
prelude.setAssociationAlgorithm(MatchingAlgorithm.Tanimoto)

#now, start your engine ...
prelude.initializeEngine()
prelude.countMindMemory()


@app.route("/neurons")
def neurons():
	count = prelude.countMindMemory()
	if count is False:
		return "n/a"
	return str(count)

@app.route("/in/<question>")
def talk(question):
    try:
	question = question.replace("+", " ").encode('utf-8')
    	answer = prelude.chatWithPrelude(question.decode('utf-8'))	
    	app.logger.info("User: " + question.decode('utf-8'))
    	app.logger.info("Bot: " + answer.decode('utf-8'))
    	try:
		prelude.forceSaveMindFile()
	except:
		print "Force saving of the mind file failed. Probably because of"
		print "Error: %s - %s" % sys.exc_info()[:2]

	app.logger.info("Neurons: " + str(prelude.countMindMemory()))
	return answer.encode('utf-8')
    except:
	print "Error: %s - %s" % sys.exc_info()[:2]
	#prelude = Prelude()
	#prelude.loadedMind = "mind.mdu"
	#prelude.initializeEngine()
	return ""#e.encode("utf-8")
if __name__ == "__main__":
    app.run(host='0.0.0.0', port=1984)


