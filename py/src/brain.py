import os
import io
import sys
import logging
import time
import objectdistance


class Brain(object):

	def __init__(self):
		logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s', level=logging.INFO)
		self.logger = logging.getLogger('prelude_log')
		self.memoryVectorizerArray = None
		self.vectorizer = None
		self.tfidf_vectorizer = None

	def readBrainFile(self, fileName):
		try:
			lines = [line.strip() for line in open(fileName)]
			return lines
		except:
			print "readBrainFileError: %s - %s" % sys.exc_info()[:2]
			return []

	def writeBrainFile(self, memoryBuffer, fileName):
		try:
			f = open(fileName,'w')
			for memory in memoryBuffer:
				f.write(memory.decode('utf-8').encode('utf8')+'\n')
			f.close()
		except:
			print "writeBrainFileError: %s - %s" % sys.exc_info()[:2]



	# def calculateMatchRateCosineTFIDF(self, idea):

		# tfidf_vectorizer = TfidfVectorizer(stop_words = None)
		# count_vectorizer = CountVectorizer(stop_words = None)
		# idea_matrix = self.vectorizer.transform(list(idea))
		# inputVectorizerArray = idea_matrix.toarray()
		#print self.memoryVectorizerArray.shape
		#print idea_matrix.shape
		#print idea_matrix.toarray()
		#cosine = cosine_similarity(idea_matrix, self.memoryVectorizerArray[0:1])
		#print cosine

		#print "CosineTFIDF"
		#print vectorizer
	#transformer = TfidfTransformer()
		#print transformer
		#test_set = set()
		#test_set.add(idea)
		#print test_set
	#	test_set = []
	#	test_set.append(idea)
		#print test_set
	#	inputVectorizerArray = self.vectorizer.transform(test_set).toarray()
		#print inputVectorizerArray
		#sleep(50)
		# cx = lambda a, b : round(np.inner(a, b)/(LA.norm(a)*LA.norm(b)), 6)
		# for vector in self.memoryVectorizerArray:
		#     for testV in inputVectorizerArray:
		#     	#print "Vector: " + str(vector)
		#     	#print "testV: " + str(testV)
		#     	print "Part1:" + str(np.inner(self.memoryVectorizerArray, testV))
		#     	print "Part2: " + str(LA.norm(vector)*LA.norm(testV))
		#         cosine = cx(vector, testV)
		#         print "Cos: " + str(cosine)
		#         return float(cosine)

	# def setMemoryVector(self, botsMemory):
		#self.tfidf_vectorizer = TfidfVectorizer()
		#tfidf_matrix = self.tfidf_vectorizer.fit_transform(list(botsMemory.keys()))
		#np.set_printoptions(threshold='nan')
		#self.vectorizer = CountVectorizer(stop_words = None) #taking stopwords into account
		#v = self.vectorizer.fit_transform(list(botsMemory.keys()))
		#tfidf = TfidfTransformer(norm="l2")
		#tfidf.fit(v)
		#print "IDF:", tfidf.idf_
		#print v
		#sleep(50)
		#self.memoryVectorizerArray = v.toarray()
		#self.memoryVectorizerArray = tfidf_matrix
		#analyze = self.vectorizer.build_analyzer()
		#print analyze
		#print self.vectorizer.get_feature_names()
		#sleep(50)
		#testV = self.vectorizer.transform(['aaafuck aaahhhhhh aaahhhhhhhh aaahhhooo ababa abandon abbie abbreviation']).toarray()
		#print "Part1:" + str(np.inner(self.memoryVectorizerArray, testV))
		#print "Part2: " + str(LA.norm(self.memoryVectorizerArray)*LA.norm(testV))
		#sleep(50)
		
	def prepInput(self, a, b):
		stOne =  ' '.join(a)
		stTwo =  ' '.join(b)
		#print stOne
		#print stTwo
		nx = set()
		ny = set()
		for i in range(0, len(stOne)-1):
			x1 = stOne[i]
			x2 = stOne[i + 1]
			temp = x1 + x2
			nx.add(temp)
        
		for j in range(0, len(stTwo)-1):
			y1 = stTwo[j]
			y2 = stTwo[j + 1]
			temp = y1 + y2
			ny.add(temp)

		return nx, ny

	def calculateMatchRateTanimoto(self, a, b):
		"""
		In some case, each attribute is binary such that each bit represents the absence of presence of a characteristic, 
		thus, it is better to determine the similarity via the overlap, or intersection, of the sets. 
		Simply put, the Tanimoto Coefficient uses the ratio of the intersecting set to the union set as the measure of similarity. 
		"""
		nx, ny = self.prepInput(a, b)	
		c = set.intersection(nx, ny)
		#print "score: " + str(float(float(len(c))/float(len(nx)+len(ny)-len(c))))
		return float(float(len(c))/float(len(nx)+len(ny)-len(c)))


	def calculateMatchRateHamman(self, a, b):
		"""
		best selector needs tweaking to work with +/- values...
		"""
		nx, ny = self.prepInput(a, b)
		score = objectdistance.Hamann(nx, ny, 0, 'Set')
		#print score
		return score
		#c = set.intersection(nx, ny)
		#d = 0
		#print "score: " + str(float((len(nx) + d) - (len(ny) + len(c))) / float(len(nx) + len(ny) + len(c) + d))
		#return float((len(nx) + d) - (len(ny) + len(c))) / float(len(nx) + len(ny) + len(c) + d)

	def calculateMatchRateKulczynski(self, a, b):
		"""
		best selector needs tweaking to work with +/- values...
		"""
		nx, ny = self.prepInput(a, b)
		score = objectdistance.Kulczynski(nx, ny, 0, 'Set')
		#print score
		return score


	def calculateMatchRateSimpson(self, a, b):
		nx, ny = self.prepInput(a, b)	
		c = set.intersection(nx, ny)
		d = 0
		print "score: " + str(float( len(nx) ) / float( min(len(nx) + len(ny), len(nx) + len(c))) )
		return float( len(nx) ) / float( min(len(nx) + len(ny), len(nx) + len(c)) )



	def calculateMatchRateDice(self, idea, memory):
		"""
		Even though we did not need to separate the string 
        for the Levensthein distance other algorithm implementations need
        the word matrix... we will (for now) just join the words.
		"""
		i =  ' '.join(idea)
		m =  ' '.join(memory)
		return self.DiceCoefficient(i, m)


	def DiceCoefficient(self, stOne, stTwo):
		"""
		Dice's coefficient measures how similar a set and another set are. It can be used to measure how similar two strings are 
        in terms of the number of common bigrams (a bigram is a pair of adjacent letters in the string).
		"""
		try:
			#print stOne
			#print stTwo
			nx = set()
			ny = set()
			for i in range(0, len(stOne)-1):
				x1 = stOne[i]
				x2 = stOne[i + 1]
				temp = x1 + x2
				nx.add(temp)
            
			for j in range(0, len(stTwo)-1):
				y1 = stTwo[j]
				y2 = stTwo[j + 1]
				temp = y1 + y2
				ny.add(temp)
			
			intersect = set.intersection(nx, ny)
			
			dbOne = len(intersect)
			score = float((2 * float(dbOne)) / float(len(nx) + len(ny)))
			#print "Score: " + str(score)
			return score

		except Exception, e:
			print "Error: " + str(e)
			self.logger.debug("error dice scoring");
			return 0


