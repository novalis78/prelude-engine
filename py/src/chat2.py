from flask import Flask
from bot import Prelude
from mind import MatchingAlgorithm
import logging
from logging.handlers import RotatingFileHandler
from flask import request

app = Flask(__name__)

handler = RotatingFileHandler("chat2.log", maxBytes=10000, backupCount=1)
handler.setLevel(logging.INFO)
app.logger.addHandler(handler)




@app.route("/in/", methods=['GET'])
def talk():
    question = request.args.get("question")
    print question
    app.logger.info("Q: " + question) 
    channel = request.args.get("channel")
    app.logger.info("C: " + channel)
    print channel
    prelude = Prelude()
    prelude.setMindFileLocation(channel+str(".mdu"))

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

    answer = prelude.chatWithPrelude(question)	
    
    app.logger.info("User: " + question)
    app.logger.info("Bot: " + answer)
    prelude.forceSaveMindFile()
    return answer

if __name__ == "__main__":
    app.run(host='104.24.107.12', port=1985)


