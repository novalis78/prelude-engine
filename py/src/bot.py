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
		self.brainLocation = "mind.mdu"
		self.quantumRandomness = False
		self.avoidLearnByRepeating = False
		self.associater = MatchingAlgorithm.CosineTFIDF
		self.saveTracking = time.time()

	def initializeEngine(self):
		self.mindInstance = Mind()
		self.mindInstance.associater = self.associater
		self.mindInstance.brainLocation = self.brainLocation
		self.chatInitiated = time.strftime("%H:%M:%S")
		self.mindInstance.analyzeShortTermMemory()


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

		elapsed_time = time.time() - self.saveTracking
		if elapsed_time > 10:
			print "soft save"
			self.forceSaveMindFile()
			self.saveTracking = time.time()
		
		return answer	



	def setTimer(self):
		pass

	def monoLog(self):
		pass

	def speak(self, a):
		pass

	def stopEngine(self):
		if self.mindInstance:
			self.mindInstance.prepareCurrentMemoryForDisc()
		if self.isContributable:
			self.mindInstance.contributeClientMind()

	def forceUpload(self):
		pass

	def forceSaveMindFile(self):
		if self.mindInstance:
			self.mindInstance.prepareCurrentMemoryForDisc()

	def countMindMemory(self):
		if self.mindInstance:
			print "My memory contains " + str(len(self.mindInstance.botsMemory)) + " neurons"
		return str(len(self.mindInstance.botsMemory))

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

	def setMindFileLocation(self, fileName):
		self.brainLocation = fileName

	def getVersionInfo(self):
		return "Prelude@# Engine, version 1.2.7, 2004-2015(c) by Lennart Lopin ";

	def setBotClient(self):
		pass

	def setProactiveMode(self, flag):
		pass







