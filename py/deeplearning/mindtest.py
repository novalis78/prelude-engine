import pandas as pd
# Import various modules for string cleaning
from bs4 import BeautifulSoup
import re
from nltk.corpus import stopwords
import nltk.data
# Initialize and train the model (this will take some time)
import gensim

# Import the built-in logging module and configure it so that Word2Vec 
# creates nice output messages
import logging
logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s',\
    level=logging.INFO)


print "Testing model..."
#model = gensim.models.Word2Vec.load("300features_40minwords_10context")

# If you don't plan to train the model any further, calling 
# init_sims will make the model much more memory-efficient.
#model.init_sims(replace=True)

# It can be helpful to create a meaningful model name and 
# save the model for later use. You can load it later using Word2Vec.load()

#model.similarity('please', 'matter')
#model['matter']

#print model

#print model[0]

#model.most_similar(positive=['prelude'], negative=['you'], topn=1)

#model.doesnt_match("man woman child kitchen".split())




model_loaded = gensim.models.Doc2Vec.load('mindmodel.doc2vec')
similars = model_loaded.most_similar("SENT_406")

#print model_loaded.syn0["SENT_2"]
print model_loaded.syn0.shape

database = {}
for uid, line in enumerate(open("mind-sentences.txt")):
           database['SENT_' + str(uid)] = line
           
print "TARGET:"
print database["SENT_406"]
print "++++++++++++++++++++++++"
print "SIMILAR:"
for b in similars:
 	print database[b[0]].strip() + " ("+str(b[1])+")"

print similars