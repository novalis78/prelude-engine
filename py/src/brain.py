import os
import io

class Brain(object):

	def readBrainFile(self, fileName):
		lines = [line.strip() for line in open(fileName)]
		return lines

	def writeBrainFile(self, memoryBuffer, fileName):
		f = open(fileName,'w')
		for memory in memoryBuffer:
			f.write(memory+'\n')
		f.close()