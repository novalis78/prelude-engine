import os
import re
import imp
import random
import logging
import webbrowser
from brain import Brain
from thought import Thought
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.feature_extraction.text import TfidfTransformer
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity 
from nltk.corpus import stopwords
import numpy as np
import numpy.linalg as LA



class Mind(Brain):


	def __init__(self):
		self.lastOutput = ""
		self.botsMemory = {}
		self.memorySize = -1
		self.allThoughts = []
		self.brainLocation = "mind.mdu"
		self.bestMatchesList = []
		self.AttentionBreadth = 5
		self.MAX_MATCHES_ALLOWED = 5
		self.matchedMemoryValues = {}
		self.quantumRandomness = False
		self.avoidLearnByRepeating = False
		self.associater = MatchingAlgorithm.CosineTFIDF
		logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s', level=logging.INFO)
		self.logger = logging.getLogger('prelude_log')
		super(Mind, self).__init__()


	def analyzeShortTermMemory(self):
		self.purifyBotsMind()			
		sc = []
		self.botsMemory = {}
		print "Location: " + self.brainLocation
		if self.brainLocation is "":
			sc = self.readBrainFile(self.brainLocation)
		else:
			sc = self.readBrainFile(self.brainLocation)
		
		for v in sc:
			if self.parseForThoughts(v) not in self.botsMemory:
				self.botsMemory[self.parseForThoughts(v)] = self.parseForWords(v)
			else:
				print self.parseForThoughts(v)

		self.logger.debug("Number of memo entries: " + str(len(self.botsMemory)))
		self.memorySize = len(self.botsMemory)


	def parseForWords(self, a):
		b = ""
		tag = "USER"
		b = self.parseXMLContent(a, tag)
		return b


	def parseForThoughts(self, a):
		b = ""
		tag = "BOT"
		b = self.parseXMLContent(a, tag)
		return b


	def parseXMLContent(self, xmlString, xmlTag ):
		startTag = "<" + xmlTag + ">"
		endTag   = "</" + xmlTag + ">"
		try:
			start = xmlString.index( startTag ) + len( startTag )
			end = xmlString.index( endTag, start )
			return xmlString[start:end]
		except ValueError:
			return ""

	def addNewInputToCurrentMemory(self, receivedInput):
		"""
		User's input becomes possible output. bot's last output becomes 
		possible input - hardcoded semantics.
		"""
		if self.lastOutput is not "":
			if receivedInput not in self.botsMemory:
				self.botsMemory[receivedInput] = self.lastOutput
				self.logger.debug("LAST OUTPUT", self.lastOutput)			
				self.logger.debug("NEW INPUT", receivedInput)
			else:
				print "Key: " + self.botsMemory[receivedInput]
		
		#update the counter to reflect
		#a more dynamic and accurate picture of
		#the memory growth when alive
		self.memorySize = len(self.botsMemory)
		print self.memorySize


	def prepareCurrentMemoryForDisc(self):
		a = self.joinWordsAndThoughts()
		if len(a) > 0:
			if self.brainLocation:
				self.writeBrainFile(a, self.brainLocation)
			else:
				self.writeBrainFile(a, "mind.mdu")


	def joinWordsAndThoughts(self):
		greyMatter = set()
		print len(self.botsMemory)
		for key, value in self.botsMemory.iteritems():
			greyMatter.add("<USER>" + value + "</USER>" + "<BOT>" + key + "</BOT>");
		return greyMatter


	def listenToInput(self, receivedInput):
		answer = ""
		self.addNewInputToCurrentMemory(receivedInput)
		self.checkInputForHiddenCommands(receivedInput)
		answer = self.thinkItOver(receivedInput)
		print answer
		if (answer == receivedInput and self.avoidLearnByRepeating):
			answer = self.getFillerAnswer();
		self.lastOutput = answer
		return answer


	def getFillerAnswer(self):
		nonembarrassingFiller = ["Really?", "What??", "hm... you sure?", "sure?", "sure.", "yeah..", "come on", "hey", " ;-P", " ;-{", " ;-)", " ^-^", " :X", "you are so funny!", "you are so cool too", "love that", "are you making fun of me?", "no", "yes", "why?", "why did you say that?", "k", "okay", "so?", "not interested...", "talk to me.", "stop that", "go on", "hmmmmm", "hah! I knew it!", "wait a sec...", "can you wait a moment?", "will be right back...just a sec", "sorry..did not mean you", "sorry wrong chat window...", "everbody makes mistakes, right?", "are you insane?", "waht does that even mean?", "how old are you?", "are you sure?", "childish", "grow up", "dont' waste my time", "you are so weeird", " ... ", "wtf", "wth", "OMG", "seriously?", "hilarious", "yawn", "B-)", "cool", "how cool is that", "and...?", "keep going :-)", "I like where this is leading...", "hang on, got a call", "nope", "(waving hello)", "=> (chilling)", "are you serious?", "you are not serious, are you?", "wait", "what did you say?", "stop it", "stop that", "can you talk to me like a normal person?", "ouch", "okay, I get it", "boring", "try again", "you are so hilarious", "hm....", "try harder...", "thx. not interested."]
		randomFiller = random.choice(nonembarrassingFiller)
		#make sure we keep learning new stuff, in case we started with an empty brain
		self.prepareCurrentMemoryForDisc()
		return randomFiller;



	def thinkItOver(self, idea):
		"""
		Kickstarts the 'thinking' process. First, deal with the edge case of an empty brain. Move up from there
		"""
		b = ""
		if len(self.botsMemory) <= 0:
			return idea

		self.loadAuxilliaryKnowledgeIntoMemory(idea);
		print "auxilliary: " 

		if len(self.bestMatchesList) <= self.MAX_MATCHES_ALLOWED:
			self.matchInputWithMemory(idea);
			self.findBestMatchWithinMemory();
		
		#testing real quantum state induced random fluctuation - cool!
		#Dr Penrose would be happy
		if self.quantumRandomness:
			b = self.randomQuantumSelectAnswer();
		else:
			b = self.randomSelectAnswer();

		#dont allow bot to repeate its last sentence
		if b is self.lastOutput: 
			b = idea;
		#bot echoes if it has no proper answer
		if len(self.bestMatchesList) <= 0: 
			b = idea;
		
		return b



	def loadAuxilliaryKnowledgeIntoMemory(self, idea):
		"""
		Prelude supports a plugin system where answers can be
		augmented with external modules evaluated at runtime
		"""
		self.bestMatchesList = []

		if self.botsMemory:
			externalAnswers = self.getExternalAnswers(idea);
			for a in externalAnswers:
				if a not in self.bestMatchesList:
					self.bestMatchesList.append(a);



	def getExternalAnswers(self, idea):
		"""
		Allowing external plugins to weigh in.
		"""
		externalAnswers = []
		try:
			class_inst = None
			expected_class = 'PreludePlugin'
			for root, dirs, files in os.walk('./plugins'):
				candidates = [fname for fname in files if fname.endswith('.py') or fname.endswith('.pyc') and not fname.startswith('__') and fname.startswith('pplugin_')]
				if candidates:
					for c in candidates:
						mod_name,file_ext = os.path.splitext(os.path.split(c)[-1])
						#print "Loaded: " + mod_name + " evaluating... "
						if file_ext.lower() == '.py':
							py_mod = imp.load_source(mod_name, './plugins/'+c)

						elif file_ext.lower() == '.pyc':
							py_mod = imp.load_compiled(mod_name, './plugins/'+c)

						if hasattr(py_mod, expected_class):
							instance = getattr(py_mod, expected_class)()
							#instance = class_inst()
							plugin_answer = instance.returnBestAnswer(idea)
							externalAnswers.append(plugin_answer)
		except Exception, e:
			print "Error loading plugins: " + str(e)

		return externalAnswers



	def matchInputWithMemory(self, idea):
		"""
		Position of best match for input in memory
		Cosine, Euclid, Hamman, Forbes, Kulczynski, Manhattan, Pearson, Simpson, Yule, Russell-Rao 
		"""
		self.matchRate = 0;
		self.matchedMemoryValues = {}
		self.allThoughts = []
		#experimental - google query addon
		#string foundKnowledge = ParseForKnowledge(a);
		#if (!String.IsNullOrEmpty(foundKnowledge))
		#    botsMemory.Add(a, foundKnowledge);

		lower_case = idea.lower() # Convert to lower case
		inputSentenceTokenized = lower_case.split()  # Split into words

		#matrix operations are handled separately
		if self.associater == MatchingAlgorithm.CosineTFIDF:
			self.calculateCosine(lower_case)
		else:
			#run through memory
			cntr = 0
			for key, value in self.botsMemory.iteritems():
				lower_case = value.lower()
				t = lower_case.split()
				cntr += 1
				if self.associater == MatchingAlgorithm.Levensthein:
					self.matchRate  = self.calculateMatchRateLS(inputSentenceTokenized, t)
				elif self.associater == MatchingAlgorithm.Dice:
					self.matchRate = self.calculateMatchRateDice(inputSentenceTokenized, t)
				elif self.associater == MatchingAlgorithm.Jaccard:
					self.matchRate = self.calculateMatchRateJ(inputSentenceTokenized, t)
				elif self.associater == MatchingAlgorithm.Tanimoto:
					self.matchRate = self.calculateMatchRateTanimoto(inputSentenceTokenized, t)
				elif self.associater == MatchingAlgorithm.Hamman:
					self.matchRate = self.calculateMatchRateHamman(inputSentenceTokenized, t)
				elif self.associater == MatchingAlgorithm.Simpson:
					self.matchRate = self.calculateMatchRateSimpson(inputSentenceTokenized, t)
				elif self.associater == MatchingAlgorithm.Kulczynski:
					self.matchRate = self.calculateMatchRateKulczynski(inputSentenceTokenized, t)	
				else:
					self.matchRate = self.calculateMatchRate(inputSentenceTokenized, t)

				tt = Thought();
				tt.MatchingMemory = value
				tt.MatchingRate = self.matchRate
				tt.PotentialResponse = key
				self.allThoughts.append(tt)

				if key not in self.matchedMemoryValues:
					if self.matchRate != 0: 
						self.matchedMemoryValues[key] = self.matchRate
						print "[" + str(cntr) + "]  @" + str(self.matchRate) + " Matching: " + key
		

	def calculateCosine(self, idea):
		self.matchedMemoryValues = {}
		self.allThoughts = []
		train_set = list(self.botsMemory.keys())
		#print len(train_set)
		#print idea
		test_set = []
		test_set.append(idea) #Query
		#print test_set
		vectorizer = CountVectorizer(stop_words = None)
		#print vectorizer
		transformer = TfidfTransformer()
		#print transformer

		trainVectorizerArray = vectorizer.fit_transform(train_set).toarray()
		testVectorizerArray = vectorizer.transform(test_set).toarray()
		#print 'Fit Vectorizer to train set', trainVectorizerArray
		#print 'Transform Vectorizer to test set', testVectorizerArray
		cx = lambda a, b : round(np.inner(a, b)/(LA.norm(a)*LA.norm(b)), 6)
		ctr = 0
		for vector in trainVectorizerArray:
			ctr += 1
			#print vector
			for testV in testVectorizerArray:
				#print testV
				cosine = cx(vector, testV)
				if cosine > 0.0:
					#print str(ctr) + "" + str(cosine)
					self.matchRate = cosine
					key = train_set[ctr-1]
					tt = Thought();
					tt.MatchingMemory = key
					tt.MatchingRate = cosine
					tt.PotentialResponse = self.botsMemory[key]
					self.allThoughts.append(tt)

					if key not in self.matchedMemoryValues:
						self.matchedMemoryValues[key] = self.matchRate
						#print "[" + str(cntr) + "]  @" + str(self.matchRate) + " Matching: " + key
		

	def parseForKnowledge(self, idea):
		pass



	def getThoughtsSorted(self):
		"""
		sort thoughts by similarity score
		"""
		if self.allThoughts and len(self.allThoughts) > 0:
			sortedThoughts = sorted(self.allThoughts, key=lambda x: x.MatchingRate, reverse=True)
			return sortedThoughts
		else:
			return None;



	def findBestMatchWithinMemory(self):
		"""
		cap sorted thoughts and retrieve answer
		"""
		sortedThoughts = self.getThoughtsSorted()
		max = 10
		if len(sortedThoughts) < 11:
			max = len(sortedThoughts)-1
		for x in range(0, max):
			print "[" + str(sortedThoughts[x].MatchingRate) + "] " +  sortedThoughts[x].MatchingMemory + " => " + sortedThoughts[x].PotentialResponse
		
		self.bestMatchesList = []
		for thought in sortedThoughts:
			if thought.MatchingRate > 0:
				self.bestMatchesList.append(thought.PotentialResponse)
				print "Added to best list: [" + str(thought.MatchingRate) + "]\t->" + thought.PotentialResponse
				if len(self.bestMatchesList) > self.AttentionBreadth:
					break;



	#deprecated 
	# def findBestMatchWithinMemoryOld(self):
	# 	i = 0
	# 	highestValue = 0
	# 	self.bestMatchesList = []
	# 	if len(self.matchedMemoryValues) > 0:
	# 		print "mm gt zero"
	# 		for key, value in self.matchedMemoryValues.iteritems():
	# 			print "checking highest: " + str(highestValue)
	# 			if highestValue <= float(value):
	# 				print "adding " + str(value)
	# 				highestValue = float(value)
		
	# 		#got the best matches, time to grab a set for our final answer
	# 		for i in xrange(highestValue, 0, -1):
	# 			self.logger.debug("Finding best match within memory: " + i); 
	# 			for k, v in self.matchedMemoryValues.iteritems():
	# 				if i == float(v):
	# 					self.bestMatchesList.add(re.Key)
	# 					logger.debug("Added to best list: [" + str(float(v)) + "]\t->" + k)
	# 					print "Added to best list: [" + str(float(v)) + "]\t->" + k
			
	# 				if len(self.bestMatchesList) > MAX_MATCHES_ALLOWED:
	# 					break						
		

	def randomSelectAnswer(self):
		b = "";
		if len(self.bestMatchesList) <= 0:
			return b
		else:
			return random.choice(self.bestMatchesList)


	def randomQuantumSelectAnswer(self, memoryBuffer):
		pass


	def purifyBotsMind(self):
		pass


	def killStickingPhrasesFromMemory(self):
		pass


	def checkInputForHiddenCommands(self, idea):
		if "google" in idea.lower():
			term = idea[idea.find("google"):]
			webbrowser.open("http://google.com?query=" + term)
		

	def contributeClientMind(self):
		pass



class MatchingAlgorithm:
    Dice, Tanimoto, Jaccard, Levensthein, Hamman, Simpson,Kulczynski, CosineTFIDF = range(8)














































