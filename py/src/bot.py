import os
import time
from threading import Timer
from mind import Mind
from mind import MatchingAlgorithm

class Prelude(object):

	def __init__(self):
		self.timer = None
		self.isSpeaking = False
		self.proactiveMode = False
		self.loadedMind = "mind.mdu"
		self.quantumRandomness = False
		self.avoidLearnByRepeating = False
		self.associater = MatchingAlgorithm.CosineTFIDF
		self.chatInitiated = time.strftime("%H:%M:%S")

	def initializeEngine(self):
		self.mindInstance = Mind()
		self.mindInstance.analyzeShortTermMemory()
		self.mindInstance.associater = self.associater
		self.chatInitiated = time.strftime("%H:%M:%S")


	def chatWithPrelude(self, question):
		if self.mindInstance is None:
			return "Error: Mind not initialized";
		if self.proactiveMode:
			self.idleTime = 0
			if self.timer is not None:
				timer.Stop()
		
		if self.quantumRandomness:
			self.mindInstance.quantumRandomness = True

		if self.avoidLearnByRepeating:
			self.mindInstance.avoidLearnByRepeating = True

		answer = ""
		answer = self.mindInstance.listenToInput(question)
		
		if self.isSpeaking:
			self.speak(answer)
		
		if self.proactiveMode:
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
		if self.mindInstance:
			print "My memory contains " + str(len(self.mindInstance.botsMemory)) + " neurons"

	def setTimer(self):
		self.timer = Timer(20 * 60, autoAnswer)



	def autoAnswer(self):
		try:
			t = self.mindInstance.listenToInput(self.autoSpeakInput)
			print "You (away):\t" 
			print "Prelude (bored):\t" + t
		except:
			print "Error: autoAnswer"

	def setAssociationAlgorithm(self, assocType):
		# if self.mindInstance:
		# 	self.mindInstance.associater = assocType
		self.associater = assocType

	def getVersionInfo(self):
		return "Prelude@# Engine, version 1.2.7, 2004-2015(c) by Lennart Lopin ";

	def setBotClient(self):
		pass

	def setProactiveMode(self, flag):
		pass







