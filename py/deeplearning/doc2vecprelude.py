import pandas as pd
# Import various modules for string cleaning
from bs4 import BeautifulSoup
import re
from nltk.corpus import stopwords
import nltk.data
# Import the built-in logging module and configure it so that Word2Vec 
# creates nice output messages
import logging
logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s',\
    level=logging.INFO)

import gensim



class LabeledLineSentence(object):
    def __init__(self, filename):
        self.filename = filename
 
    def __iter__(self):
        for uid, line in enumerate(open(filename)):
            yield LabeledSentence(words=line.split(), labels=['SENT_%s' % uid])



#sentences = LabeledLineSentence("mind-sentences.txt")
sentences = gensim.models.doc2vec.LabeledLineSentence("mind-sentences.txt")

for a in sentences:
	print a

# model = gensim.models.Doc2Vec(alpha=0.025, min_alpha=0.025)  # use fixed learning rate
# model.build_vocab(sentences)
 
# for epoch in range(10):
#     model.train(sentences)
#     model.alpha -= 0.002  # decrease the learning rate
#     model.min_alpha = model.alpha  # fix the learning rate, no decay


# # store the model to mmap-able files
# model.save('mindmodel2.doc2vec')


# print model.most_similar("SENT_0")



