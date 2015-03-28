import os
from threading import Timer
import time
from mind import Mind

class Prelude(object):


	def initializeEngine(self):
		self.timer = None
		self.isSpeaking = False
		self.proactiveMode = False
		self.loadedMind = "mind.mdu"
		self.quantumRandomness = False
		self.avoidLearnByRepeating = False
		self.mindInstance = Mind()



	def chatWithPrelude(self, question):
		if self.mindInstance is None:
			return "Error: Mind not initialized";
		if self.proactiveMode:
			self.idleTime = 0
			if self.timer is not None:
				timer.Stop()
		if quantumRandomness:
			mindInstance.quantumRandomness = True

		if avoidLearnByRepeating:
			mindInstance.avoidLearnByRepeating = True

		answer = ""
		answer = self.mindInstance.listenToInput(question)
		
		if isSpeaking:
			self.speak(answer)
		
		if proactiveMode:
			self.setTimer()
			self.autoSpeakInput = answer
			self.timer.start()
		
		return answer	



	def setTimer(self):
		pass

	def monoLog(self):
		pass

	def speak(self, a):
		pass

	def stopEngine(self):
		pass

	def forceUpload(self):
		pass

	def forceSaveMindFile(self):
		pass

	def countMindMemory(self):
		pass

	def setTimer(self):
		self.timer = Timer(20 * 60, autoAnswer)



	def autoAnswer(self):
		try:
			t = self.mindInstance.listenToInput(self.autoSpeakInput)
			print "You (away):\t" 
			print "Prelude (bored):\t" + t
		except:
			print "Error: autoAnswer"



	def getVersionInfo(self):
		return "Prelude@# Engine, version 1.2.7, 2004-2015(c) by Lennart Lopin ";

	def setBotClient(self):
		pass

	def setProactiveMode(self, flag):
		pass



class MatchingAlgorithm:
    Dice, Tanimoto, Jaccard, Leventhein = range(4)



